
using Newtonsoft.Json;

namespace StomatoloskaOrdinacija.Models
{
    public partial class User
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Role { get; set; }
        public string EmailID { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Password { get; set; }
        public string? Picture { get; set; }
        public bool IsEmailVerified { get; set; } = false;
        public System.Guid ActivationCode { get; set; }


        public US? Specialization { get; set; }
        
        public List<Rating>? RatingsFrom { get; set; }
        public List<Rating>? RatingsTo { get; set; }
        public List<Comment>? CommentsFrom { get; set; }
        public List<Comment>? CommentsTo { get; set; }
        public List<WorkingDay>? WorkingDays { get; set; }
        public List<Term>? Terms { get; set; }
        public CV? CV { get; set; }
    }
}
