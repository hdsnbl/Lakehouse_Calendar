using LakeHouseCalendarWebsite.Classes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor.Services;
using (var connection = new Npgsql.NpgsqlConnection("Host=localhost;Database=Lakehouse_Calendar;Username=postgres;Password=1234"))
{                                                                                             
    connection.Open();

    string sql = @"
        CREATE TABLE IF NOT EXISTS requests (
            id SERIAL PRIMARY KEY,
            name TEXT NOT NULL,
            approved BOOL
        );

        CREATE TABLE IF NOT EXISTS calendar (
            id SERIAL PRIMARY KEY,
            name TEXT NOT NULL,
            exclusive BOOL,
            approved BOOL,
            date TIMESTAMP NOT NULL,
            request_id INT NOT NULL,
            FOREIGN KEY (request_id) REFERENCES requests(id),
            notes TEXT NULL
        );

        CREATE TABLE IF NOT EXISTS users (
            id SERIAL PRIMARY KEY,
            username TEXT NOT NULL,
            password TEXT NOT NULL,
            admin BOOL NOT NULL,
            email TEXT NULL
        );
    ";

    using (var command = new Npgsql.NpgsqlCommand(sql, connection))
    {
        command.ExecuteNonQuery();
    }
}





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<IRequest, Request>();
//builder.Services.AddSingleton<ICalendarItem, CalendarItem>();
//builder.Services.AddSingleton<CalendarItem>();
builder.Services.AddScoped<ICalendar, Calendar>();
builder.Services.AddScoped<ApiService, ApiService>();
//builder.Services.AddScoped<IUser, User>();

builder.Services.AddHttpClient();

//builder.Services.AddSingleton<Request>();
//builder.Services.AddSingleton<CalendarItem>();
builder.Services.AddMudServices();

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
