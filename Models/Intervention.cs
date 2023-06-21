using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations;

namespace StomatoloskaOrdinacija.Models
{
    public class Intervention
    {
        [Key]
        public int InterventionID { get; set; }
        public string Name { get; set; }
        public int InterventionTimeMinutes { get; set; }
        public List<TermInt> TermInt { get; set; }

        public List<US>? US { get; set; }
    }
}
