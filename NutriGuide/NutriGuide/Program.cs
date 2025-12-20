
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NutriGuide.Hubs;
using NutriGuide.MapConfigs;
using NutriGuide.Models;
using NutriGuide.Services;
using NutriGuide.UnitOfWorks;
using System.Text;

namespace NutriGuide
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ===== Services =====
            builder.Services.AddControllers();
            builder.Services.AddOpenApi();

            builder.Services.AddDbContext<NutriGuideDbContext>(op =>
                op.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAutoMapper(op => op.AddProfile(typeof(MapConfig)));
            builder.Services.AddScoped<UnitOfWork>();
            builder.Services.AddScoped<IEmailService, EmailService>();

            // SignalR
            builder.Services.AddSignalR();

            // Background Service
            builder.Services.AddHostedService<MedicineReminderService>();

            var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins!)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });



            // ===== JWT Authentication =====
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    var key = Encoding.ASCII.GetBytes(
                        "ITI - Team5 Graduation Project Track Full Stack web Developer using .NET Minya Branch");

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };

                    // SignalR JWT Support
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var accessToken = context.Request.Query["access_token"];
                            var path = context.HttpContext.Request.Path;

                            if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/notificationHub"))
                            {
                                context.Token = accessToken;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            var app = builder.Build();


        // Configure the HTTP request pipeline.
        //if (app.Environment.IsDevelopment())
        //{

        //}
        // ===== HTTP Pipeline =====
        app.MapOpenApi();
            app.UseSwaggerUI(op => op.SwaggerEndpoint("/openapi/v1.json", "v1"));

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<NotificationHub>("/notificationHub");


            app.Run();
        }
    }
}


