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

        public async Task<Like> GetLike(int userId, int recipientId)
        {
           return await _datacontext.Likes.FirstOrDefaultAsync(u=>u.LikerId==userId && u.LikeeId==recipientId); 
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
            if(userParams.likers)
            {
              var userLikers = await GetUserLikes(userParams.UserId, userParams.likers);
              users=users.Where(u => userLikers.Contains(u.id));
            }
            if(userParams.likees)
            {
               var userLikees = await GetUserLikes(userParams.UserId, userParams.likers);
              users=users.Where(u => userLikees.Contains(u.id));
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

        private async Task<IEnumerable<int>> GetUserLikes(int id , bool likers)
        {
            var user =  await _datacontext.Users
                    .Include("Likers").Include("Likees").FirstOrDefaultAsync(u => u.id == id);
            if(likers)
            {
                return  user.Likers.Where( u => u.LikeeId == id).Select(x => x.LikerId);
            } 
            else 
            return  user.Likees.Where( u => u.LikerId == id).Select(x => x.LikeeId);
        }

        async Task<bool> IDatingRepository.SaveAll()
        {
            return await _datacontext.SaveChangesAsync() > 0;
        }

        public async Task<Message> GetMessage(int id)
        {
            return await _datacontext.Messages.FirstOrDefaultAsync(m => m.Id == id);
        }


        public async Task<IEnumerable<Message>> GetMessageThread(int userId, int recipientId)
        {
            return await  _datacontext.Messages.Include( u => u.Sender).ThenInclude( p => p.Photos)
                         .Include(u => u.Recipient).ThenInclude ( p => p.Photos).AsQueryable()
                         .Where(m => m.RecipientId==recipientId && m.SenderDelated ==false && m.SenderId==userId 
                         || m.RecipientId==userId && m.RecipientDelated ==false && m.SenderId==recipientId).OrderByDescending(m => m.MessageSent).ToListAsync();

        }

        public async Task<PagedList<Message>> GetMessageForUser(MessageParams messageParams)
        {
          var messages = _datacontext.Messages.Include( u => u.Sender).ThenInclude( p => p.Photos)
                         .Include(u => u.Recipient).ThenInclude ( p => p.Photos).AsQueryable();

          switch(messageParams.MessageContainer) 
          {
              case "Inbox" : messages=messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDelated == false ) ;
              break;
              case "Outbox": messages=messages.Where(m => m.SenderId == messageParams.UserId && m.SenderDelated == false);
              break;
              default : messages=messages.Where(m => m.RecipientId == messageParams.UserId && m.RecipientDelated == false && m.IsRead == false);
              break;

          }
          messages=messages.OrderByDescending(d => d.MessageSent);
          return await PagedList<Message>.CreateAsync(messages, messageParams.PageNumber, messageParams.PageSize);              
        }
    }
}