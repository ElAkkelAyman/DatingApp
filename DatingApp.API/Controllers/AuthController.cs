using System.IdentityModel.Tokens.Jwt;
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
           var LoggedUser =  await _repo.Login(usert.UserName.ToLower(),usert.Password);
           if(LoggedUser==null)
            return Unauthorized();

            var claims = new []
            {
                new Claim(ClaimTypes.NameIdentifier,LoggedUser.id.ToString()),
                new Claim(ClaimTypes.Name,LoggedUser.UserName)
            };
           var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_conf.GetSection("AppSettings:Token").Value));

           var cred = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
           var tokenDescriptor = new SecurityTokenDescriptor
           {
               Subject=new ClaimsIdentity(claims),
               Expires= System.DateTime.Now.AddDays(1)
           };
           var tokenHandler = new JwtSecurityTokenHandler();
           var token = tokenHandler.CreateToken(tokenDescriptor);

           return Ok(new {
              token = tokenHandler.WriteToken(token) 
           });
        }

    }
}