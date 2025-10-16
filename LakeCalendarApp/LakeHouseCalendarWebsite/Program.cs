using LakeHouseCalendarWebsite.Classes;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
//builder.Services.AddSingleton<IRequest, Request>();
//builder.Services.AddSingleton<ICalendarItem, CalendarItem>();
//builder.Services.AddSingleton<CalendarItem>();
builder.Services.AddScoped<ICalendar, Calendar>();
builder.Services.AddScoped<ApiService, ApiService>();

builder.Services.AddHttpClient();

//builder.Services.AddSingleton<Request>();
//builder.Services.AddSingleton<CalendarItem>();


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
