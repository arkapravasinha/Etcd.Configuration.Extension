using Etcd.Configuration.Extension.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etcd.Configuration.Extension.API.Test._50
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .AddEtcdConfiguration(options =>
            {
                options.Hosts = "http://host.docker.internal";
                options.Port = 8098;
                options.Keys = "testapplication/test:string,testapplication/testjson:json";
                options.ReloadOnChenge = true;
                options.OnClientCreationFailure = (x) =>
                {
                    Console.WriteLine(x.Message);
                };
                options.OnLoadFailure = (x) => { Console.WriteLine(x.Message); };
                options.OnWatchFailure = (x) => { Console.WriteLine(x.Message); };
                //options.UserName = "test";
                //options.Password= "test";
                options.UseFullPathForKeys = true;
            })
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
