using NLog.Web;
using BusinessLayer.Interface;
using BusinessLayer.Service;
using RepositoryLayer.Interface;
using RepositoryLayer.Service;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using RepositoryLayer.Context;
using Microsoft.EntityFrameworkCore;
using ModelLayer.Model;
using Middleware.GlobalExceptionHandler;
using Microsoft.AspNetCore.Mvc.Filters;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add<Middleware.GlobalExceptionHandler.GlobalExceptionFilter>(); // Register Global Exception Filter
})
.AddNewtonsoftJson();



builder.Services.AddScoped<IGreetingBL, GreetingBL>();
builder.Services.AddScoped<IGreetingRL,GreetingRL>();

builder.Services.AddScoped<IUserBL,UserBL>();
builder.Services.AddScoped<IUserRL,UserRL>();


builder.Services.AddScoped<GreetingModel>();





//databse connectivity
var connectionString = builder.Configuration.GetConnectionString("SqlConnection");
builder.Services.AddDbContext < HelloGreetingAppContext>(Options => Options.UseSqlServer(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HelloGreetingApplication",
        Version = "v1",
        Description = "API for Greeting App",
        Contact = new OpenApiContact
        {
            Name = "Ishwar Singh",
            Email = "ishwarmars@gmail.com" 
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});



//Logger statements
builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

app.UseSwagger();//json file create karta hai
app.UseSwaggerUI();// colourfull UI create krta.

app.UseExceptionHandler("/error");
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
