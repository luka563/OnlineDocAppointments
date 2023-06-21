namespace StomatoloskaOrdinacija.Models
{
    public class GetDoctorDTO
    {        
        public string fullName { get; set; }
        public string userName { get; set; }
        public float average { get; set; }
        public string picture { get; set; }
        public string specialization { get; set; }
        public List<InterventionDTO> interventions { get; set; }
        public List<CommentDTO> comments { get; set; }
    }
}
