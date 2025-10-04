
using System.Text;
using BLL;
using BLL.Manager;
using BLL.Manager.Interfaces;
using BLL.Services;
using DAL.Interfaces;
using DAL.Models;
using DAL.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RetouchAgency.Authorization;

namespace RetouchAgency
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<ApplicationContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }, ServiceLifetime.Scoped);

            // Register Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
            if (jwtOptions == null)
                throw new InvalidOperationException("JWT configuration is missing.");
            builder.Services.AddSingleton(jwtOptions);
            
            builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
                };
            });
            
            builder.Services.AddAuthorization(options =>
            {
                // Register AdminOrOwner policy for default "id" parameter
                options.AddPolicy("AdminOrOwner_id", policy =>
                    policy.Requirements.Add(new AdminOrOwnerRequirement("id")));
                    
                // You can add more policies for different parameter names if needed
                // options.AddPolicy("AdminOrOwner_userId", policy =>
                //     policy.Requirements.Add(new AdminOrOwnerRequirement("userId")));
            });
            
            // Register the authorization handler
            builder.Services.AddScoped<IAuthorizationHandler, AdminOrOwnerHandler>();

            // Register Services
            builder.Services.AddScoped<IFileUploadService, FileUploadService>();

            // Register Managers
            builder.Services.AddScoped<IUserManager, UserManager>();
            builder.Services.AddScoped<IEventBookingManager, EventBookingManager>();
            builder.Services.AddScoped<IEventManager, EventManager>();
            builder.Services.AddScoped<IOpportunityManager, OpportunityManager>();
            builder.Services.AddScoped<IApplicationManager, ApplicationManager>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // Enable static file serving
            app.UseStaticFiles();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
