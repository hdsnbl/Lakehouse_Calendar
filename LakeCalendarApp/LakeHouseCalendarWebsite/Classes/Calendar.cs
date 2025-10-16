namespace LakeHouseCalendarWebsite.Classes
{
    public class Calendar : ICalendar
    {
        public List<CalendarItem> Items { get; set; } = new List<CalendarItem>();

        public Calendar() { }
        public Calendar(CalendarItem item)
        {
            Items.Add(item);
        }
        public void AddToCalendar(CalendarItem item)
        {
            Items.Add(item);
        }

    }
}
