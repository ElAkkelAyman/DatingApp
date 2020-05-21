using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    { 
     private readonly IAuthRepository _repo;
     private readonly IConfiguration _conf;
     public AuthController(IAuthRepository repo,IConfiguration conf)
    {
        this._repo=repo;
        this._conf=conf;
    }
    [HttpPost("register")]
       public async Task<IActionResult> Register(UserForRegisterDto user)
        {
            // validate request
            user.UserName = user.UserName.ToLower();
            if (await _repo.UserExists( user.UserName))
            return BadRequest("Username already exists");
            var userToCreate = new User 
            {
                UserName= user.UserName
            };
            var CreatedUser = await _repo.Register(userToCreate,user.Password);
            return StatusCode(201);
        }

      [HttpPost("login")]
       public async Task<IActionResult> Login(UserForLoginDto usert)
        {
           var userFromRepo = await _repo.Login(usert.UserName.ToLower(), usert.Password);

            if (userFromRepo == null)
                return Unauthorized();

            // generate token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_conf.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.NameIdentifier,userFromRepo.id.ToString()),
                     new Claim(ClaimTypes.Name,userFromRepo.UserName)
                }),
                Expires = DateTime.Now.AddDays(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha512Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var MainPhotoUrl=userFromRepo.Photos.Where(p => p.IsMain).FirstOrDefault().Url;

            return Ok(new { tokenString ,MainPhotoUrl });
        }

    }
}