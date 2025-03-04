using NLog.Web;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IGreetingBL, GreetingBL>();
builder.Services.AddScoped<IGreetingRL,GreetingRL>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Logger statements
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

app.UseSwagger();//json file create karta hai
app.UseSwaggerUI();// colourfull UI create krta.


// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
