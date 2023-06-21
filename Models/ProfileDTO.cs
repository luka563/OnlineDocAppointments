namespace StomatoloskaOrdinacija.Models
{
    public class ProfileDTO
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailID { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Picture { get; set; }
        public bool isVerified { get; set; }
    }
}
