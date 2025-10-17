using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Xml.Linq;

namespace LakeHouseCalendarWebsite.Classes
{
    public class Request : IRequest
    {
        //public DateTime Date { get; set; }
        //public string Name { get; set; }
        //public bool? Exclusive { get; set; }
        //public enum StatusType { New, Approved, Rejected }
        //public StatusType Status { get; set; }

        //public int Id { get; set; }

        public List<CalendarItem> FullRequest { get; set; } = new List<CalendarItem>();
        public Request(CalendarItem item)
        {
            FullRequest.Add(item);
        }

        //public Request(int id, DateTime date, string name, bool? exclusive)
        //{
        //    Id = id;
        //    Date = date;
        //    Name = name;
        //    Exclusive = exclusive;
        //    Status = StatusType.New;
        //}

        //public Request(int id, DateTime date, string name, bool? exclusive, StatusType status)
        //{
        //    Id = id;
        //    Date = date;
        //    Name = name;
        //    Exclusive = exclusive;
        //    Status = status;
        //}

        //public Request()
        //{
        //    Id = 0;
        //}

    }
}
