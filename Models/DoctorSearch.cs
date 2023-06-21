namespace StomatoloskaOrdinacija.Models
{
    public class DoctorSearch
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string? Picture { get; set; }

        public float Rating { get; set; }

        public string Specialization { get; set; }
    }
}
