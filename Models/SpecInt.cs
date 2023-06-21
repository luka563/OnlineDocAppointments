using System.ComponentModel.DataAnnotations;

namespace StomatoloskaOrdinacija.Models
{
    public class SpecInt
    {
        [Key]
        public int SpecIntID { get; set; }
        public US? US { get; set; }
        public int USID { get; set; }
        public Intervention? Intervention { get; set; }
        public int InterventionID { get; set; }

    }
}
