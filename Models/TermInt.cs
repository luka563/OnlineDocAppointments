using System.ComponentModel.DataAnnotations;

namespace StomatoloskaOrdinacija.Models
{
    public class TermInt
    {
        [Key]
        public int TermIntID { get; set; }
        public Term Term { get; set; }
        public int TermID { get; set; }
        public Intervention Intervention { get; set; }
        public int InterventionID { get; set; }


    }
}
