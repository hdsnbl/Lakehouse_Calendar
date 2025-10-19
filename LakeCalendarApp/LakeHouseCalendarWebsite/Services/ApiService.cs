

using Newtonsoft.Json;
using Npgsql;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        //---------------------------DATABASE PORTION-------------------------------------------------------------

        private const string ConnectionString = "Host=localhost;Database=Lakehouse_Calendar;Username=postgres;Password=1234";
        public static void AddCalendarEvent(string name, bool? exclusive, bool? approved, DateTime date)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO calendar (name, exclusive, approved, date)
                VALUES (@name, @exclusive, @approved, @date);
            ";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@exclusive", (object?)exclusive ?? DBNull.Value);
                    command.Parameters.AddWithValue("@approved", (object?)approved ?? DBNull.Value);
                    command.Parameters.AddWithValue("@date", date);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Event added successfully!");
        }
        public static void AddRequest(string name, bool? exclusive, bool? approved, DateTime date)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = @"
                INSERT INTO requests (name, exclusive, approved, date)
                VALUES (@name, @exclusive, @approved, @date);
            ";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@exclusive", (object?)exclusive ?? DBNull.Value);
                    command.Parameters.AddWithValue("@approved", (object?)approved ?? DBNull.Value);
                    command.Parameters.AddWithValue("@date", date);

                    command.ExecuteNonQuery();
                }
            }

            Console.WriteLine("Event added successfully!");
        }
        public static List<CalendarItem> GetAllRequests()
        {
            var events = new List<CalendarItem>();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "SELECT id, name, exclusive, approved, date FROM requests;";

                using (var command = new NpgsqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(new CalendarItem
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Exclusive = reader.GetBoolean(reader.GetOrdinal("exclusive")),
                            Approved = reader.IsDBNull(reader.GetOrdinal("approved"))
                                ? null
                                : reader.GetBoolean(reader.GetOrdinal("approved")),
                            Date = reader.GetDateTime(reader.GetOrdinal("date"))
                        });
                    }
                }
            }

            return events;
        }
        public static List<CalendarItem> GetCalendarEvents(DateTime dateFrom, DateTime dateTo)
        {
            var events = new List<CalendarItem>();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT id, name, exclusive, approved, date
                    FROM calendar
                    WHERE date BETWEEN @dateFrom AND @dateTo;
                ";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@dateFrom", dateFrom);
                    command.Parameters.AddWithValue("@dateTo", dateTo);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            events.Add(new CalendarItem
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Exclusive = reader.GetBoolean(reader.GetOrdinal("exclusive")),
                                Approved = reader.IsDBNull(reader.GetOrdinal("approved"))
                                    ? null
                                    : reader.GetBoolean(reader.GetOrdinal("approved")),
                                Date = reader.GetDateTime(reader.GetOrdinal("date"))
                            });
                        }
                    }
                }
            }

            return events;
        }
        public static List<CalendarItem> GetAllCalendarEvents()
        {
            var events = new List<CalendarItem>();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "SELECT id, name, exclusive, approved, date FROM calendar;";

                using (var command = new NpgsqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        events.Add(new CalendarItem
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                            Exclusive = reader.GetBoolean(reader.GetOrdinal("exclusive")),
                            Approved = reader.IsDBNull(reader.GetOrdinal("approved"))
                                ? null
                                : reader.GetBoolean(reader.GetOrdinal("approved")),
                            Date = reader.GetDateTime(reader.GetOrdinal("date"))
                        });
                    }
                }
            }

            return events;
        }
        public static bool DeleteCalendarEvent(int id)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = "DELETE FROM calendar WHERE id = @id;";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // true if something was deleted
                }
            }
        }
        public static bool UpdateCalendarEvent(CalendarItem updatedItem)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = @"
                    UPDATE calendar
                    SET name = @name,
                    exclusive = @exclusive,
                    approved = @approved,
                    date = @date
                    WHERE id = @id;
                ";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", updatedItem.Id);
                    command.Parameters.AddWithValue("@name", updatedItem.Name);
                    command.Parameters.AddWithValue("@exclusive", (object?)updatedItem.Exclusive ?? DBNull.Value);
                    command.Parameters.AddWithValue("@approved", (object?)updatedItem.Approved ?? DBNull.Value);
                    command.Parameters.AddWithValue("@date", updatedItem.Date);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // returns true if an existing row was updated
                }
            }
        }

    }
}
