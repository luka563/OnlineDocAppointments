using System.ComponentModel.DataAnnotations;

namespace StomatoloskaOrdinacija.Models
{
    public class SI
    {
        [Key]
        public int SIID { get; set; }
        public int SpecId { get; set; }
        public int IntId { get; set; }
    }
}
