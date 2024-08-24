
using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.MyLogging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

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
            builder.Services.AddCors(options =>
            {
                //options.AddDefaultPolicy(policy =>
                //{
                //    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                //});
                options.AddPolicy("AllowAll" ,policy =>
                {
                    policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
                options.AddPolicy("AllowOnlyLocalhost", policy =>
                {
                    policy.WithOrigins("http://localhost:4200").AllowAnyHeader().AllowAnyMethod();
                });
                options.AddPolicy("AllowOnlyMicrosoft", policy =>
                {
                    policy.WithOrigins("http://microsoft.com").AllowAnyHeader().AllowAnyMethod();
                });
            });
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecret"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("api/testingendpoint",
                    context => context.Response.WriteAsync("Test 1"))
                    .RequireCors("AllowOnlyLocalhost");

                endpoints.MapControllers()
                         .RequireCors("AllowAll");

                endpoints.MapGet("api/testingendpoint2",
                    context => context.Response.WriteAsync("Test Response 2"));

            });

            app.MapControllers();

            app.Run();
        }
    }
}
