
using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.MyLogging;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace CollegeApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Logging.ClearProviders();
            //builder.Logging.AddLog4Net();

            #region Serilog Setttings
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Information()
            //    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Minute)
            //    .CreateLogger();
            //builder.Services.AddSerilog();
            //Add services to the container.
            #endregion
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IMyLoger, LogToFile>();
            builder.Services.AddDbContext<CollegeDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnect"));
            });
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
            builder.Services.AddCors(options => options.AddPolicy("MyTestCORS", policy =>
            {
                //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                policy.WithOrigins("http://127.0.0.1:5510");
            }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            //app.UseCors("MyTestCORS");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
