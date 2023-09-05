using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using API.Provider;
using API.Service;
using API.Service.IService;
using Application.Core;
using Application.Events;
using Application.Interface;
using Infrastructure.Security;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using StackExchange.Redis;

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
            services.AddScoped<SaveUploadedFileService>();
            services.AddTransient<TicketIdToTicketConverter>();


            //S3
            services.AddScoped<IStorageService,StorageService>();

            //ImageHandling

            services.AddScoped<SaveUploadedFileService>();

            //JsonProvider
            services.AddScoped<JsonProvider>();


            //Redis for localTesting
            services.AddSingleton<IConnectionMultiplexer>(c => {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                var connection =  ConnectionMultiplexer.Connect(options);
                if (connection.IsConnected)
                {
                    Console.WriteLine("Redis connection established.@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@");
                }
                else
                {
                    Console.WriteLine("Failed to connect to Redis.xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx");
                }

                return connection;
            });



            //Redis for Elastic Cache

            // string ElasticCachePassword = Environment.GetEnvironmentVariable("AwsElasticCachePassword");

            // services.AddSingleton<IConnectionMultiplexer>(c => {
            //     var options = ConfigurationOptions.Parse("master.redis.cgmc5z.apne1.cache.amazonaws.com:6379");
            //     options.Password = ElasticCachePassword;
            //     options.Ssl = true;
            //     options.AllowAdmin = true; // 如果需要進行管理操作，可以設置為 true
            //     options.AbortOnConnectFail = false; // 如果要允許重試連接，可以設置為 false

            //     var connection =  ConnectionMultiplexer.Connect(options);
            //     if (connection.IsConnected)
            //     {
            //         Console.WriteLine("ElasticRedis connection established.");
            //     }
            //     else
            //     {
            //         Console.WriteLine("Failed to connect to Redis.");
            //     }

            //     return connection;
            // });

            services.AddSingleton<IResponseCacheService,ResponseCacheService>();

            return services;
        }
    }
}

