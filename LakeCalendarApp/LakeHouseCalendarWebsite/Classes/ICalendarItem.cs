
namespace LakeHouseCalendarWebsite.Classes
{
    public interface ICalendarItem
    {
        DateTime Date { get; set; }
        bool? Exclusive { get; set; }
        string Name { get; set; }
        List<Request> requests { get; set; }
        public enum StatusType { New, Approved, Rejected }
        public StatusType Status { get; set; }
    }
}