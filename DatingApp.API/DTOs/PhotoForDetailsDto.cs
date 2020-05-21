namespace DatingApp.API.DTOs
{
    public class PhotoForDetailsDto
    {
         public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; } 
        public int UserId { get; set; }
        public string PublicID { get; set; }

    }
}