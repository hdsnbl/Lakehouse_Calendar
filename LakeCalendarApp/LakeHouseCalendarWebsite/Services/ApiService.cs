

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


        //---------------------------DATABASE PORTION-------------------------------------------------------------

        private const string ConnectionString = "Host=localhost;Database=Lakehouse_Calendar;Username=postgres;Password=1234";
        public static CalendarItem AddAndGetCalendarEvent(string name, bool? exclusive, bool? approved, DateTime date, int request_id, string notes)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = @"
                    INSERT INTO calendar (name, exclusive, approved, date, request_id, notes)
                    VALUES (@name, @exclusive, @approved, @date, @request_id, @notes)
                    RETURNING id, name, exclusive, approved, date, request_id, notes;
                ";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@exclusive", (object?)exclusive ?? DBNull.Value);
                    command.Parameters.AddWithValue("@approved", (object?)approved ?? DBNull.Value);
                    command.Parameters.AddWithValue("@date", date);
                    command.Parameters.AddWithValue("@request_id", request_id);
                    command.Parameters.AddWithValue("@notes", notes);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new CalendarItem
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Exclusive = reader.IsDBNull(2) ? (bool?)null : reader.GetBoolean(2),
                                Approved = reader.IsDBNull(3) ? (bool?)null : reader.GetBoolean(3),
                                Date = reader.GetDateTime(4),
                                Request_id = reader.GetInt32(5),
                                Notes = reader.GetString(6)
                            };
                        }
                    }
                }
            }
            return null;
        }
        public static Request AddAndGetRequest(string name)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = @"
                    INSERT INTO requests (name)
                    VALUES (@name)
                    RETURNING id, name;
                ";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@name", name);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Request
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            };
                        }
                    }
                }
            }

            Console.WriteLine("Event added successfully!");
            return null;
        }
        public static List<Request> GetAllRequests()
        {
            var requests = new List<Request>();

            // Get all requests
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                string sql = "SELECT id, name, approved FROM requests";
                using (var command = new NpgsqlCommand(sql, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        requests.Add(new Request
                        {
                            Id = reader.GetInt32(0),
                            Name = reader.GetString(1),
                            FullRequest = new List<CalendarItem>(),
                            Approved = reader.IsDBNull(reader.GetOrdinal("approved"))
                                ? null
                                : reader.GetBoolean(reader.GetOrdinal("approved"))
                        });
                    }
                }

                // Get calendar items for each request
                foreach (var req in requests)
                {
                    string calendarSql = "SELECT id, name, exclusive, approved, date, request_id FROM calendar WHERE request_id = @request_id";
                    using (var calCmd = new NpgsqlCommand(calendarSql, connection))
                    {
                        calCmd.Parameters.AddWithValue("@request_id", req.Id);
                        using (var calReader = calCmd.ExecuteReader())
                        {
                            while (calReader.Read())
                            {
                                req.FullRequest.Add(new CalendarItem
                                {
                                    Id = calReader.GetInt32(0),
                                    Name = calReader.GetString(1),
                                    Exclusive = calReader.IsDBNull(2) ? (bool?)null : calReader.GetBoolean(2),
                                    Approved = calReader.IsDBNull(3) ? (bool?)null : calReader.GetBoolean(3),
                                    Date = calReader.GetDateTime(4),
                                    Request_id = calReader.GetInt32(5)
                                });
                            }
                        }
                    }
                }
            }

            return requests;
        }
        public static void UpdateRequest(Request request)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = @"
                    UPDATE requests
                    SET name = @name,
                    approved = @approved
                    WHERE id = @id;
                ";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", request.Id);
                    command.Parameters.AddWithValue("@name", request.Name);
                    command.Parameters.AddWithValue("@approved", (object?)request.Approved ?? DBNull.Value);

                    command.ExecuteNonQuery();
                }
            }
        }
        public static List<CalendarItem> GetCalendarEvents(DateTime dateFrom, DateTime dateTo)
        {
            var events = new List<CalendarItem>();

            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();

                string sql = @"
                    SELECT id, name, exclusive, approved, date, notes
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
                                Date = reader.GetDateTime(reader.GetOrdinal("date")),
                                Notes = reader.IsDBNull(reader.GetOrdinal("notes"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("notes"))
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

                string sql = "SELECT id, name, exclusive, approved, date, notes FROM calendar;";

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
                            Date = reader.GetDateTime(reader.GetOrdinal("date")),
                            Notes = reader.IsDBNull(reader.GetOrdinal("notes"))
                                ? null
                                : reader.GetString(reader.GetOrdinal("notes"))

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
                    date = @date,
                    notes = @notes
                    WHERE id = @id;
                ";

                using (var command = new NpgsqlCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@id", updatedItem.Id);
                    command.Parameters.AddWithValue("@name", updatedItem.Name);
                    command.Parameters.AddWithValue("@exclusive", (object?)updatedItem.Exclusive ?? DBNull.Value);
                    command.Parameters.AddWithValue("@approved", (object?)updatedItem.Approved ?? DBNull.Value);
                    command.Parameters.AddWithValue("@date", updatedItem.Date);
                    command.Parameters.AddWithValue("@notes", (object?)updatedItem.Notes ?? DBNull.Value);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0; // returns true if an existing row was updated
                }
            }
        }

    }
}
