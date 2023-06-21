using System.ComponentModel.DataAnnotations;

namespace StomatoloskaOrdinacija.Models
{
    public class Blog
    {
        [Key]
        public int BlogID { get; set; }
        [Required]
        public string TextHTML { get; set; }
        [Required]
        public string Details { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Image { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
