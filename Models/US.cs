using System.ComponentModel.DataAnnotations;

namespace StomatoloskaOrdinacija.Models
{
    public class US
    {
        [Key]
        public int USID { get; set; }

        public List<User>? User { get; set; }   
        
        [Required]
        public string? specialization { get; set; }
    }
}
