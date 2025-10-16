using System.Reflection.Metadata;

namespace LakeHouseCalendarWebsite.Classes
{
    public class CalendarItem
    {
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public bool? Exclusive { get; set; }
        public List<Request> Requests { get; set; }
        public bool? Approved { get; set; }
        
        public CalendarItem() { 
            Requests = new List<Request>();

        }

        public CalendarItem(DateTime date, string name, bool? exclusive)
        {
            Date = date;
            Name = name;
            Exclusive = exclusive;
            Requests = new List<Request>();
            Approved = false;
        }
        public CalendarItem(DateTime date, string name, bool? exclusive, bool? approved)
        {
            Date = date;
            Name = name;
            Exclusive = exclusive;
            Requests = new List<Request>();
            Approved = approved;
        }
    }
}
