using Etcd.Configuration.Extension.Extensions;
using Microsoft.Extensions.Configuration;

namespace Etcd.Configuration.Extension.API.Test._70
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Configuration.AddEtcdConfiguration(options =>
            {
                options.Hosts = "http://host.docker.internal";
                options.Port = 8098;
                options.Keys = "testapplication/test:string,testapplication/testjson:json";
                options.ReloadOnChange = true;
                options.OnClientCreationFailure = (x) =>
                {
                    Console.WriteLine(x.Message);
                };
                options.OnLoadFailure = (x) => { Console.WriteLine(x.Message); };
                options.OnWatchFailure = (x) => { Console.WriteLine(x.Message); };
                //options.UserName = "test";
                //options.Password= "test";
                options.UseFullPathForKeys = true;
            });
            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapGet("/allKey", (HttpContext httpContext) =>
            {
                return builder.Configuration.AsEnumerable();
            })
            .WithName("allKey");

            app.MapGet("/fullKey", (HttpContext httpContext) =>
            {
                return builder.Configuration.GetSection("testapplication/test").AsEnumerable()
                .Concat(builder.Configuration.GetSection("testapplication/testjson").AsEnumerable());
            })
            .WithName("fullKey");

            app.Run();
        }
    }
}