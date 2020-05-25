using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using DatingApp.API.Models.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
    public class DatingRepository : IDatingRepository
    {
        private  readonly DataContext _datacontext;
        public DatingRepository(DataContext datacontext)
        {
            this._datacontext = datacontext;

        }
        void IDatingRepository.Add<T>(T entity) where T : class
        {
            _datacontext.Add(entity);
        }

        void IDatingRepository.Delete<T>(T entity) where T : class
        {
            _datacontext.Remove(entity);
        }

        async Task<Photo> IDatingRepository.GetMainPhoto(int UserId)
        {
            var photo = await _datacontext.Photos.Where(p => p.UserId == UserId & p.IsMain==true).FirstOrDefaultAsync();
           return photo;
        }

        async Task<Photo> IDatingRepository.GetPhoto(int id,int UserID)
        {
           var photo = await _datacontext.Photos.FirstOrDefaultAsync(p => p.Id==id & p.UserId==UserID);
           return photo;
        }

        async Task<User> IDatingRepository.GetUser(int id)
        {
            var user = await _datacontext.Users.Include(p=>p.Photos).FirstOrDefaultAsync
            (u=>u.id==id);

            return user;
        }

        async Task<PagedList<User>> IDatingRepository.GetUsers(UserParams userParams)
        {
            var users =  _datacontext.Users.Include(p=>p.Photos).OrderByDescending(u=>u.LastActive).Where(u =>u.id != userParams.UserId && u.Gender == userParams.Gender);
            if(userParams.MinAge != 18 || userParams.MaxAge != 99)
            {
                var MinDateofBirth=DateTime.Today.AddYears(-userParams.MaxAge -1);
                var MaxDateofBirth = DateTime.Today.AddYears(-userParams.MinAge);

                users=users.Where(u => u.DateOfBirth >= MinDateofBirth && u.DateOfBirth <= MaxDateofBirth);
            }
            if(!string.IsNullOrEmpty(userParams.OrderBy))
            {
                switch (userParams.OrderBy) 
                {
                    case "created": users = users.OrderByDescending(u => u.Created);
                    break;
                    default : users = users.OrderByDescending(u => u.LastActive); break;
                }
            }
            return await PagedList<User>.CreateAsync(users,userParams.PageNumber, userParams.PageSize);
        }

        async Task<bool> IDatingRepository.SaveAll()
        {
            return await _datacontext.SaveChangesAsync() > 0;
        }
    }
}