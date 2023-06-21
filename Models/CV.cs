using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StomatoloskaOrdinacija.Models
{
    public class CV
    {
        [Key]
        public int CVId { get; set; }
        public User? Doctor { get; set; }

        [ForeignKey("Doctor")]
        public int DoctorID { get; set; }
        public int IsApproved { get; set; }
        public string? CvFile { get; set; }
    }
}
