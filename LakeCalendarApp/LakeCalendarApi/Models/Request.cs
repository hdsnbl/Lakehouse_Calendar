namespace LakeHouseCalendarWebsite.Classes
{
    public class Request
    {
        //public CalendarItem CalendarItem { get; set; }      //Should be a List of Calendar items    List<CalendarItem> CalendarItems
        //public List<CalendarItem> CalendarItems { get; set; }
        public int Id { get; set; }

        public Request(int id)
        {
            //CalendarItems = new List<CalendarItem>();
            //CalendarItem = request;
            Id = id;
        }

        public Request()
        {
            Id = 0;
        }
    }
}
