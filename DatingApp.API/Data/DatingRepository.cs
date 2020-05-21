using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        async Task<IEnumerable<User>> IDatingRepository.GetUsers()
        {
            var users = await _datacontext.Users.Include(p=>p.Photos).ToListAsync();
            return users;
        }

        async Task<bool> IDatingRepository.SaveAll()
        {
            return await _datacontext.SaveChangesAsync() > 0;
        }
    }
}