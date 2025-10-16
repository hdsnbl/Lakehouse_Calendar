

using Newtonsoft.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;
//using static LakeHouseCalendarWebsite.Classes.CalendarItem;

namespace LakeHouseCalendarWebsite.Services
{
    public class ApiService
    {
        IConfiguration config;
        HttpClient client;

        public ApiService(IConfiguration configuration) 
        {
            config = configuration;
            client = new HttpClient();
            //client.BaseAddress = new Uri(config["ApiHost"]!);
            client.DefaultRequestHeaders.Add("x-api-key", config["Key"]);
        }

        public List<CalendarItem> GetCalendarItems(DateTime dateFrom, DateTime dateTo) 
        {
            var url = $"{config["ApiHost"]}CalendarItems?dateFrom={dateFrom}&dateTo={dateTo}";
            var results = client.GetAsync(url).Result;
            var calendarItems = new List<CalendarItem>();
            if (results.IsSuccessStatusCode)
            {
                calendarItems = JsonConvert.DeserializeObject<List<CalendarItem>>(results.Content.ReadAsStringAsync().Result);
            }



            calendarItems.Add(new CalendarItem(new DateTime(2025, 10, 28), "Bailey", true, true));
            calendarItems.Add(new CalendarItem(new DateTime(2025, 10, 29), "Bailey", true, true));
            calendarItems.Add(new CalendarItem(new DateTime(2025, 10, 30), "Bailey", true, true));
            calendarItems.Add(new CalendarItem(new DateTime(2025, 10, 31), "Bailey", true, true));

            calendarItems.Add(new CalendarItem(new DateTime(2025, 10, 18), "Allen", false, true));

            return calendarItems;
        }

        public List<CalendarItem> GetAllCalendarItems()
        {
            var calendarItems = new List<CalendarItem>();


            return calendarItems;
        }

        //public List<Request> GetRequests()
        //{
        //    var requestItems = new List<Request>();
        //    requestItems.Add(new Request(1, new DateTime(2025, 10, 28), "Bailey", true));
        //    requestItems.Add(new Request(2, new DateTime(2025, 10, 29), "Bailey", true));
        //    requestItems.Add(new Request(3, new DateTime(2025, 10, 30), "Bailey", true));
        //    requestItems.Add(new Request(4, new DateTime(2025, 10, 31), "Bailey", true));
        //    return requestItems;
        //}
        public List<CalendarItem> GetRequests()
        {
            var url = $"{config["ApiHost"]}CalendarItems";
            var results = client.GetAsync(url).Result;
            var calendarItems = new List<CalendarItem>();
            if (results.IsSuccessStatusCode)
            {
                calendarItems = JsonConvert.DeserializeObject<List<CalendarItem>>(results.Content.ReadAsStringAsync().Result);
            }


            calendarItems.Add(new CalendarItem(new DateTime(2025, 10, 27), "Bailey", true));        //auto sets Approved to null
            calendarItems.Add(new CalendarItem(new DateTime(2025, 10, 26), "Bailey", true));
            calendarItems.Add(new CalendarItem(new DateTime(2025, 10, 25), "Bailey", true));

            calendarItems.Add(new CalendarItem(new DateTime(2025, 10, 18), "Allen", false, true));

            calendarItems.Add(new CalendarItem(new DateTime(2025, 10, 09), "Allen", false, false));

            return calendarItems;
        }
    }
}
