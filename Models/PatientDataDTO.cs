namespace StomatoloskaOrdinacija.Models
{
    public class PatientDataDTO
    {
        public string startDateS { get; set; }
        public string endDateS { get; set; }        
        public string patientFullName { get; set; }
        public int startingTime { get; set; }
        public List<string> interventions { get; set; }
    }
}
