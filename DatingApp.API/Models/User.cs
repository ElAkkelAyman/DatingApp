using System;
using System.Collections.Generic;

namespace DatingApp.API.Models
{
    public class User
    {
        public int id { get; set; }
        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; } // key to recreate the hash

        public string Gender { get; set; }
        public DateTime DateOfBirth { get; set; }

        public string KnownAs { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastActive {get ; set;}

        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        public ICollection<Photo> Photos { get; set; }

        public ICollection<Like> Likers { get; set; }

        public ICollection<Like> Likees { get; set; }

        public ICollection<Message> MessagesSent { get; set; } 

        public ICollection<Message> MessagesReceived { get; set; }    
        }


    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsMain { get; set; } 
        public string PublicID { get; set; }

        public User User { get; set; }

        public int UserId { get; set; }

    }
}