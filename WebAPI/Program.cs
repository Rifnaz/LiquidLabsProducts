using DbLayer;
using ServiceLayer;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDataAccessServices(builder.Configuration);
builder.Services.AddBusinessLogicServices();

builder.Services.AddControllers(); 
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
