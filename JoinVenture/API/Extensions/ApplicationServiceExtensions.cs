using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Provider;
using API.Service;
using Application.Core;
using Application.Events;
using Application.Interface;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();


            //sqlserver

            DotNetEnv.Env.Load();
            string DefaultConnection = Environment.GetEnvironmentVariable("DB_CONNECTIONSTRING");
            services.AddDbContext<DataContext>(options => options.UseSqlServer(DefaultConnection));


            //CORS

            services.AddCors(opt 
                => {
                        opt.AddPolicy("CorsPolicy", policy => {
                                policy.AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://127.0.0.1:5500","https://fff5-2402-7500-4d5-a113-e930-21d7-3d9c-cf18.ngrok-free.app");
                                
                            });
                });


            //MediatorR

            services.AddMediatR(typeof(List.Handler));

            //BackGoundService

            // services.AddHostedService<ExpiredTicketCleanUpService>();


            //AutoMapper

            services.AddAutoMapper(typeof(MappingProfiles).Assembly);         


            //UserAccessor

            services.AddHttpContextAccessor();   
            services.AddScoped<IUserAccessor,UserAccessor>();

            //SignalR
            services.AddSignalR();

            //HttpClient

            services.AddHttpClient<LINEPayService>();

            //JsonProvider

            services.AddSingleton<JSONProvider>();

            //CutomService

            services.AddScoped<LINEPayService>();
            services.AddScoped<TicketBookingService>();


            return services;
        }
    }
}