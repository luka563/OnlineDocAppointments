namespace StomatoloskaOrdinacija.Models
{
    public class TermDTO
    {   
        public string startDate { get; set; }
        public int startHours { get; set; }
        public int startMinutes { get; set; }
        public int endHours { get; set; }
        public int endMinutes { get; set; }
        public bool isAvailable { get; set; }
        public bool isItMyTerm { get; set; }
        public int StartTime()
        {
            return startHours * 60 + startMinutes;
        }
    }
}
