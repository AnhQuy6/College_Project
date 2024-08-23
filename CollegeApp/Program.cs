
using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
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
            builder.Services.AddTransient<IStudentRepository, StudentRepository>();   
            builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>)); 
            builder.Services.AddDbContext<CollegeDBContext>(options =>  
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyConnect"));
            });
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
            builder.Services.AddCors(options => options.AddPolicy("MyTestCORS", policy =>
            {
                //policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
            }));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("MyTestCORS");

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
