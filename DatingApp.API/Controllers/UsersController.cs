using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
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
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userparams)
        {
            var CurrentUserID=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var CurrentUserFromRepo=await _repo.GetUser(CurrentUserID);
            userparams.UserId=CurrentUserID;
            if(string.IsNullOrEmpty(userparams.Gender))
            userparams.Gender = CurrentUserFromRepo.Gender == "male" ? "female" : "male";
            var users= await _repo.GetUsers(userparams);
            var UserstoReturn=_mapper.Map<List<UserForListDto>>(users);
           // Response.AddPagination(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);
            var PaginationBody = new PaginaionHeader(users.CurrentPage,users.PageSize,users.TotalCount,users.TotalPages);
            return Ok( new {UserstoReturn ,PaginationBody });
        }

        [HttpGet("{id}",Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            
            var user= await _repo.GetUser(id);
            var UsertoReturn=_mapper.Map<UserForDetailsDto>(user);
            return Ok(UsertoReturn); 
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id,UserForUpdateDto userForUpdate)
        {
            try 
        {
           if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
           return Unauthorized();
           var userFromepo =await _repo.GetUser(id);
           _mapper.Map(userForUpdate,userFromepo);
           if(await _repo.SaveAll())
           return NoContent();
        }
            catch(Exception ex)
            {
                throw ex;
            }
            return NoContent();
           

        }
        [HttpPost("{id}/like/{recipientId}")]
        public async Task<IActionResult> LikeUser(int id,int recipientId)
        {
           if(id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
           return Unauthorized();
           var like = await _repo.GetLike(id,recipientId);
           if(like != null)
           return BadRequest("you already liked this user");
           if(await _repo.GetUser(recipientId) == null)
           return NotFound();
           var Like = new Like {
           LikerId = id,
           LikeeId=recipientId            
           };
           _repo.Add(Like);
           if(await _repo.SaveAll())
           return Ok("you liked the user");
           else 
           return BadRequest("Failed to like user");

        }




    }
}