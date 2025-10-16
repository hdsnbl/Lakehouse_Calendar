namespace LakeHouseCalendarWebsite.Classes
{
    public class Day
    {
        public List<CalendarItem> calendarItems;

        public Day()
        {
            calendarItems = new List<CalendarItem>();
        }
        public Day(CalendarItem item)
        {
            calendarItems = new List<CalendarItem>();
            calendarItems.Add(item);
        }
    }
}
