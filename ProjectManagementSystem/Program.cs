
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ProjectManagementSystem.Authontication;
using ProjectManagementSystem.Data.Context;
using ProjectManagementSystem.Helper;
using ProjectManagementSystem.Repository.Interface;
using ProjectManagementSystem.Data.Entities;
using ProjectManagementSystem.Repository.Repository;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ProjectManagementSystem
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

            builder.Services.AddScoped(typeof(GenericRepository<>));
            builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));

            builder.Services.AddOptions<JwtOptions>()
           .BindConfiguration(JwtOptions.SectionName)
           .ValidateDataAnnotations()
           .ValidateOnStart();

            var jwtSettings =  builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

            builder.Services.AddAuthentication(opts =>
            {
                opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.SaveToken=true;
                  options.TokenValidationParameters = new TokenValidationParameters
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidateLifetime = true,
                      ValidateIssuerSigningKey = true,
                      ValidIssuer = jwtSettings?.Issuer,
                      ValidAudience = jwtSettings?.Audience, 
                      IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings?.Key!))
                  };
              });


            builder.Services.AddDbContext<ApplicationDBContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
                .LogTo(log => Debug.WriteLine(log), LogLevel.Information)
                .EnableSensitiveDataLogging(); ;
            });

            builder.Services.AddAutoMapper(typeof(MappingProfile));
            var app = builder.Build();

            MapperHandler.mapper = app.Services.GetService<IMapper>();
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
