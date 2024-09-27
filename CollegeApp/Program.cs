
using CollegeApp.Configurations;
using CollegeApp.Data;
using CollegeApp.Data.Repository;
using CollegeApp.Infrastructure;
using CollegeApp.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
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

            //#region Serilog Setttings
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Information()
            //    .WriteTo.File("Log/log.txt", rollingInterval: RollingInterval.Minute)
            //    .CreateLogger();
            //builder.Services.AddSerilog();
            //#endregion
            //Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the bearer scheme. Enter Bearer [space] add your token in the text input. Example: Bearer kdfjkdfjkdfj",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                }); 
            });


            builder.Services.AddScoped(typeof(ICollegeRepository<>), typeof(CollegeRepository<>));
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IRolePrivilegeService, RolePrivilegeService>();
            builder.Services.AddExceptionHandler<GlobalExceptionHandle>();
            builder.Services.AddProblemDetails();
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

            }).AddJwtBearer("LoginForGoogleUser", options =>
            {
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretForGoogle"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            })
            .AddJwtBearer("LoginForMicrosoftUser", options =>
            {
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretForMicrosoft"))),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            })
            .AddJwtBearer("LoginForLocalUser", options =>
            {
                //options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration.GetValue<string>("JWTSecretForLocal"))),
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

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapGet("api/testingendpoint",
            //        context => context.Response.WriteAsync("Test 1"))
            //        .RequireCors("AllowOnlyLocalhost");

            //    endpoints.MapControllers()
            //             .RequireCors("AllowAll");

            //    endpoints.MapGet("api/testingendpoint2",
            //        context => context.Response.WriteAsync("Test Response 2"));

            //});
            app.UseExceptionHandler();

            app.MapControllers();

            app.Run();
        }
    }
}
