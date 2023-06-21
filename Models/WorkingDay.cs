using System.ComponentModel.DataAnnotations;

namespace StomatoloskaOrdinacija.Models
{
    public class WorkingDay
    {
        [Key]
        public int WorkingDayID { get; set; }
        public User Doctor { get; set; }
        public DateTime Day { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public List<Term> Terms { get; set; }
    }
}
