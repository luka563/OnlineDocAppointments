using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StomatoloskaOrdinacija.Models
{
    public class Rating
    {
        [Key]
        public int RatingID { get; set; }
        public virtual User FromWho { get; set; }
        public int FromWhoID { get; set; }
        public virtual User ToWho { get; set; }
        public int ToWhoID { get; set; }
        [Range(0,5)]
        public float Rate { get; set; }
    }
}
