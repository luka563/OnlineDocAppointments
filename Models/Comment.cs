using System.ComponentModel.DataAnnotations;

namespace StomatoloskaOrdinacija.Models
{
    public class Comment
    {

        [Key]
        public int CommentID { get; set; }
        public virtual User? CommentFrom { get; set; }
        public int CommentFromID { get; set; }
        public virtual User? CommentTo{ get; set; }
        public int CommentToID { get; set; }

        [Required]
        [MaxLength(200)]
        public string CommentContent { get; set; } = "";

        [Required]
        public DateTime Date { get; set; }
    }
}
