using System;

namespace DatingApp.API.DTOs
{
    public class MessageForCreationDto
    {
        public  int SenderId { get; set; }
        public int RecipientId { get; set; } 
        public DateTime MessageSent { get; set; }
        public string Conetent {get ; set; }
        public MessageForCreationDto()
        {
            MessageSent= DateTime.Now;
        }

    }
}