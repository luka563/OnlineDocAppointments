namespace StomatoloskaOrdinacija.Models
{
    public class TermPatientDTO
    {
        public DateTime startDate { get; set; }
        public string startDateS { get; set; }
        public string endDateS { get; set; }
        public int workingDay{ get; set; }
        public string fullName { get; set; }

        public string doctorUserName { get; set; }
    }
}
