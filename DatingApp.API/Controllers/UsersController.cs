using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    [Authorize] // what kind of authentication : conf statrtup authentication middelware
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            this._repo=repo;
            this._mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            
            var users= await _repo.GetUsers();
            var UserstoReturn=_mapper.Map<List<UserForListDto>>(users);
            return Ok(UserstoReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            
            var user= await _repo.GetUser(id);
            var UsertoReturn=_mapper.Map<UserForDetailsDto>(user);
            return Ok(UsertoReturn); 
        }
    }
}