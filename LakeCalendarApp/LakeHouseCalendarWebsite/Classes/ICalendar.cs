
namespace LakeHouseCalendarWebsite.Classes
{
    public interface ICalendar
    {
        List<CalendarItem> Items { get; set; }

        void AddToCalendar(CalendarItem item);
    }
}