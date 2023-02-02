# Etcd.Configuration.Extension
An Extension Library to Configure IConfiguration with ETCD Key-Value Pair.

# Build and Scan
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=arkapravasinha_Etcd.Configuration.Extension&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=arkapravasinha_Etcd.Configuration.Extension)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=arkapravasinha_Etcd.Configuration.Extension&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=arkapravasinha_Etcd.Configuration.Extension)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=arkapravasinha_Etcd.Configuration.Extension&metric=bugs)](https://sonarcloud.io/summary/new_code?id=arkapravasinha_Etcd.Configuration.Extension)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=arkapravasinha_Etcd.Configuration.Extension&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=arkapravasinha_Etcd.Configuration.Extension)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=arkapravasinha_Etcd.Configuration.Extension&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=arkapravasinha_Etcd.Configuration.Extension)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=arkapravasinha_Etcd.Configuration.Extension&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=arkapravasinha_Etcd.Configuration.Extension)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=arkapravasinha_Etcd.Configuration.Extension&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=arkapravasinha_Etcd.Configuration.Extension)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=arkapravasinha_Etcd.Configuration.Extension&metric=coverage)](https://sonarcloud.io/summary/new_code?id=arkapravasinha_Etcd.Configuration.Extension)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=arkapravasinha_Etcd.Configuration.Extension&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=arkapravasinha_Etcd.Configuration.Extension)
[![Build Status](https://dev.azure.com/arkaprava123040/Pipelines/_apis/build/status/ETCD.Configuration.Extension%20BUILD%20CI?branchName=master&jobName=Agent%20job%201)](https://dev.azure.com/arkaprava123040/Pipelines/_build/latest?definitionId=12&branchName=master)

# How to use the Library
## Installtion
Nuget Package Manager
```
Install-Package Etcd.Configuration.Extension
```
.NET CLI
```
dotnet add package Etcd.Configuration.Extension
```
## Usage
You can configure ETCD Configuration as below, if you want you can also confugure SSL settings and create a custom Handler

### For Example .Net 7 Minimal API
Program.cs
```c#
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
```

### For .Net 6 Non-Minimal API
Program.cs
```c#
using Etcd.Configuration.Extension.Extensions;

namespace Etcd.Configuration.Extension.API.Test._60
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

            builder.Services.AddControllers();
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


            app.MapControllers();

            app.Run();
        }
    }
}
```
Fetch the settings using below mechanism,
Controller
```c#
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Etcd.Configuration.Extension.API.Test._60.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeysController : ControllerBase
    {

        private readonly ILogger<KeysController> _logger;
        private readonly IConfiguration configuration;

        public KeysController(ILogger<KeysController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        [HttpGet("/fullKey")]
        public IActionResult Get()
        {
            return Ok(configuration.GetSection("testapplication/test").AsEnumerable()
                .Concat(configuration.GetSection("testapplication/testjson").AsEnumerable()));
        }

        [HttpGet("/allKey")]
        public IActionResult GetOnly()
        {
            return Ok(configuration.AsEnumerable());
        }
    }
}
```
### For .Net 5 API
Program.cs
```c#
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
            })
            .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}

```
Controllers
```c#
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Etcd.Configuration.Extension.API.Test._50.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class KeysController : ControllerBase
    {

        private readonly ILogger<KeysController> _logger;
        private readonly IConfiguration configuration;

        public KeysController(ILogger<KeysController> logger, IConfiguration configuration)
        {
            _logger = logger;
            this.configuration = configuration;
        }

        [HttpGet("/fullKey")]
        public IActionResult Get()
        {
            return Ok(configuration.GetSection("testapplication/test").AsEnumerable()
                .Concat(configuration.GetSection("testapplication/testjson").AsEnumerable()));
        }

        [HttpGet("/allKey")]
        public IActionResult GetOnly()
        {
            return Ok(configuration.AsEnumerable());
        }
    }
}

```
## Settings
```
        /// <summary>
        /// ETCD Grpc Client BackOffMultipler, Defaults to 1.5
        /// </summary>
        double BackoffMultiplier { get; set; }

        /// <summary>
        /// ETCD Grpc Client Host URL, Defaults to Empty String, Pass the hosts in commaseparated manner
        /// </summary>
        string Hosts { get; set; }

        /// <summary>
        /// ETCD Grpc Client Handler, useful to write custom handlers, defaults to null
        /// </summary>
        HttpClientHandler HttpClientHandlerForEtcd { get; set; }

        /// <summary>
        /// ETCD Grpc Client InitialBackoffSeconds, Defaults to 1
        /// </summary>
        int InitialBackoffSeconds { get; set; }

        /// <summary>
        /// Keys should in below format {key:string},{key:json} ie. "key1:string,key2:json"
        /// This is used to fetch the configurations, in ETCD , it should be the path to your values
        /// </summary>
        string Keys { get; set; }

        /// <summary>
        /// ETCD Grpc Client Max Attempt, defaults to 5
        /// </summary>
        int MaxAttempts { get; set; }

        /// <summary>
        /// ETCD Grpc Client MaxBackoffSeconds, defaults to 5
        /// </summary>
        int MaxBackoffSeconds { get; set; }

        /// <summary>
        /// ETCD Grpc Client MaxTokens, defaults to 10
        /// </summary>
        int MaxTokens { get; set; }

        /// <summary>
        /// This is used to configure the behavior during an exception while loading Configuration from ETCD 
        /// </summary>
        Action<EtcdConfigOnLoadException> OnLoadFailure { get; set; }

        /// <summary>
        /// This is used to configure the behavior during an exception while watching Configuration from ETCD 
        /// </summary>
        Action<EtcdConfigWatchException> OnWatchFailure { get; set; }

        /// <summary>
        /// This is used to configure the behavior during an exception while creating ETCD client 
        /// </summary>
        Action<EtcdGrpcClientException> OnClientCreationFailure { get; set; }

        /// <summary>
        /// Password for ETCD Client, Only configure it if ETCD Authentication is enabled
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Port for ETCD Client
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// If Reload is Required, this will change the configuration
        /// </summary>
        bool ReloadOnChenge { get; set; }

        /// <summary>
        /// ETCD Client Server Name
        /// </summary>
        string ServerName { get; set; }

        /// <summary>
        /// Whether SSL is Required, defaults to false
        /// </summary>
        bool Ssl { get; set; }

        /// <summary>
        /// ETCD Client Token Ratio, defaults to 0.1
        /// </summary>
        double TokenRatio { get; set; }

        /// <summary>
        ///  Password for ETCD Client, Only configure it if ETCD Authentication is enabled
        /// </summary>
        string UserName { get; set; }

        /// <summary>
        /// Whether to Have Full Key Path in Loaded Configuration
        /// </summary>
        bool UseFullPathForKeys { get; set; }

```


# Benifits
Use this library to manage Application Configuration easily without worrying about restarting the application to refresh the configuration.
All the configurations are easily accessed via IConfiguration. It has multiple extension methods to integrate with your current codebase.

# Installing ETCD
```yml
version: '2'

services:
  etcd:
    image: docker.io/bitnami/etcd:3.5
    environment:
      - ALLOW_NONE_AUTHENTICATION=yes
    volumes:
      - etcd_data:/bitnami/etcd
volumes:
  etcd_data:
    driver: local
```
Refer to https://github.com/bitnami/containers/tree/main/bitnami/etcd
# Contributing
This is an open source project, if you want to do any modification to the codebase that is welcomed, Please raise a PR for master branch. Please include your unit test as well. Once your pr is raised, it will scan the code in sonar cloud and bot will comment on your PR, then only it will be merged after a through review.

# Issues
To raise any new issue, please go to below link, https://github.com/arkapravasinha/Etcd.Configuration.Extension/issues/new

# Thanks
https://github.com/shubhamranjan/dotnet-etcd for providing seamless integration with ETCD
