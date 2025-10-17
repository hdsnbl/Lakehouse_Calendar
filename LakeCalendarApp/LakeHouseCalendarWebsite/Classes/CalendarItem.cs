using System.Reflection.Metadata;

namespace LakeHouseCalendarWebsite.Classes
{
    public class CalendarItem
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
        public bool? Exclusive { get; set; }
        public bool? Approved { get; set; }
        public CalendarItem()
        {
            Approved = false;
        }

        public CalendarItem(DateTime date, string name, bool? exclusive)
        {
            Date = date;
            Name = name;
            Exclusive = exclusive;
            Approved = null;
        }
        public CalendarItem(DateTime date, string name, bool? exclusive, bool? approved)
        {
            Date = date;
            Name = name;
            Exclusive = exclusive;
            Approved = approved;
        }
    }
}
