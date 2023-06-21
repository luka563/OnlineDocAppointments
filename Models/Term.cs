using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace StomatoloskaOrdinacija.Models
{
    public class Term
    {
        [Key]
        public int TermID { get; set; }
        public User User { get; set; }
        public int UserID { get; set; }
        public DateTime TermStartDate { get; set; }
        public DateTime TermEndDate { get; set; }
        public int WorkingDayID { get; set; } // foreign key property
        public WorkingDay WorkingDay { get; set; } // navigation property
        public List<TermInt> TermInt { get; set; }
    }
}
