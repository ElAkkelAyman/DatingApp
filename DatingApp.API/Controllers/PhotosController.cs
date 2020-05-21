using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/user/{Userid}/photos")]
    [ApiController]

    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
        private Cloudinary _cloudinary;
        public PhotosController(IDatingRepository repo,
                                IMapper mapper,
                                IOptions<CloudinarySettings> cloudinaryConfig)
        {
            this._cloudinaryConfig = cloudinaryConfig;
            this._mapper = mapper;
            this._repo = repo;

            Account account = new Account(
           _cloudinaryConfig.Value.CloudName,
           _cloudinaryConfig.Value.ApiKey,
           _cloudinaryConfig.Value.ApiSecret);

          _cloudinary = new Cloudinary(account);

        }
        [HttpGet("{Id}",Name="GetPhoto")]
        public async Task<IActionResult> GetPhoto(int Userid,int id)
        {
            var photoFromRepo=await this._repo.GetPhoto(id,Userid);
            if(photoFromRepo != null)
            {
            var photo =_mapper.Map<PhotoForDetailsDto>(photoFromRepo);
            return Ok(photo);
            }
            else 
            return BadRequest("photo does not exist");
        
        }
          

        [HttpPost]
        public async Task<IActionResult> AddPhotoForUser(int Userid,
                                                        [FromForm] PhotoForCreationDto photoDto)
        {
            if(Userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
           return Unauthorized();
           var userFromepo =await _repo.GetUser(Userid);
           var file = photoDto.File;
           var uploadResults = new ImageUploadResult();
           if(file.Length > 0)
           {
               using(var stream = file.OpenReadStream())
               {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.Name, stream),
                    Transformation = new Transformation().Width(500).Height(500).Crop("fill").Gravity("face")
                };
                uploadResults=_cloudinary.Upload(uploadParams);
                
               }
           }
           photoDto.Url=uploadResults.Uri.ToString();
           photoDto.PublicID=uploadResults.PublicId;
           var photo =_mapper.Map<Photo>(photoDto);
           if(!userFromepo.Photos.Any())
           photo.IsMain=true;
           userFromepo.Photos.Add(photo);
           if(await this._repo.SaveAll())
            return CreatedAtRoute("GetPhoto",new {Userid=Userid, id=photo.Id},photo);

            return BadRequest("Could not add the photo");
        }
        //update ressource ( set picture to main )
        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int Userid,int id)
        {
           if(Userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
           return Unauthorized();
           var user = await _repo.GetUser(Userid);
           if(!user.Photos.Any(p => p.Id==id))
           return Unauthorized();
           var photoFromReop = await _repo.GetPhoto(id,Userid);
           if(photoFromReop.IsMain)
           return BadRequest("this is already the main photo");
           var CurrentMainPhoto=await _repo.GetMainPhoto(Userid);
           CurrentMainPhoto.IsMain=false;
           photoFromReop.IsMain=true;
           if(await _repo.SaveAll())
           return NoContent();

           return BadRequest("photo was not changed to main");
           
           
        }
        [HttpDelete("{id}/DeletePicture")]
        public async Task<IActionResult> DeletePhoto(int Userid,int id)
        {
           if(Userid != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
           return Unauthorized();
           var user = await _repo.GetUser(Userid);
           if(!user.Photos.Any(p => p.Id==id))
           return Unauthorized();
           var photoFromReop = await _repo.GetPhoto(id,Userid);
           if(photoFromReop.IsMain)
           return BadRequest("You cannot delete the main photo");
           if(String.IsNullOrEmpty(photoFromReop.PublicID))
           _repo.Delete(photoFromReop);
           else 
           {
          var deleteParams = new DeletionParams(photoFromReop.PublicID);
         var result = _cloudinary.Destroy(deleteParams);
          if(result.Result.Equals("ok"))
          _repo.Delete(photoFromReop);
           }
           if(await _repo.SaveAll())
           return Ok();

           return BadRequest("photo was not deleted");
           
           
        }
}      
}