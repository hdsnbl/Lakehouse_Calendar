using LakeCalendarApi.Attributes;
using LakeHouseCalendarWebsite.Classes;
using Microsoft.AspNetCore.Mvc;

namespace LakeCalendarApi.Controllers
{
    [ApiKey]
    [ApiController]
    [Route("[controller]")]
    public class CalendarItemsController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<CalendarItem>> GetStringValue(DateTime dateFrom, DateTime dateTo)
        {
            var calendarItems = new List<CalendarItem>();

            //calendarItems.Add(new CalendarItem(new DateTime(2024, 07, 28), "Bailey", true));
            //calendarItems.Add(new CalendarItem(new DateTime(2024, 07, 29), "Bailey", true));
            //calendarItems.Add(new CalendarItem(new DateTime(2024, 07, 30), "Bailey", true));
            //calendarItems.Add(new CalendarItem(new DateTime(2024, 07, 31), "Bailey", true));

            return calendarItems;
        }
    }
}
