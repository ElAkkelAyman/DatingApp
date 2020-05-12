using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    { 
     private readonly IAuthRepository _repo;
     public AuthController(IAuthRepository repo)
    {
        this._repo=repo;
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

    [HttpGet("RegisterGet")]
       public IActionResult RegisterGet(UserForRegisterDto user)
        {
            var values= "test values good";
            return Ok(values);
        }

    }
}