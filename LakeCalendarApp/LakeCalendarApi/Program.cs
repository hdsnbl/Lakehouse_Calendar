using LakeCalendarApi.Attributes;

var builder = WebApplication.CreateBuilder(args);
var configuration = new ConfigurationBuilder() 
    .AddJsonFile("appsettings.json", optional: false)
    .Build();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //options.SwaggerDoc("LakehouseAPI", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Lakehouse Calendar API", Version = "1", Description = "The API to handle the Lakehouse Calendar Web App" });
    options.OperationFilter<ApiKeySwaggerAttribute>();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
