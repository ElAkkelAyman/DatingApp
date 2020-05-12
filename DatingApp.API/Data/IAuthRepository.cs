using System.Threading.Tasks;
using DatingApp.API.Models;
namespace DatingApp.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Register(User user , string passord);
         Task<User> Login(string username , string passord);
         Task<bool> UserExists(string username);
    }
}