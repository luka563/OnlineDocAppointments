namespace StomatoloskaOrdinacija.Models
{
    public class Appointment
    {
        public string fromWho { get; set; }
        public string ToWho { get; set; }
        public string startTime { get; set; }
        public List<string> interventions { get; set; }
    }
}
