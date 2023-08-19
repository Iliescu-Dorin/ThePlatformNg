using Core.SharedKernel.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog.Parsing;
using System.Text;

namespace Core.Services.Setup.ServiceExtensions;

/// <summary>
/// Project Startup Configuration
/// </summary>
public static class AppConfigSetup
{
    public static void AddAppConfigSetup(this IServiceCollection services, IHostEnvironment env)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        if (AppSettings.app(new string[] { "Startup", "AppConfigAlert", "Enabled" }).ObjToBool())
        {
            if (env.IsDevelopment())
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Console.OutputEncoding = Encoding.GetEncoding("GB2312");
            }

            Console.WriteLine("************ Blog.Core Configuration Set *****************");

            ConsoleHelper.WriteSuccessLine("Current environment: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

            // Authorization scheme
            if (Permissions.IsUseIds4)
            {
                ConsoleHelper.WriteSuccessLine($"Current authorization scheme: " + (Permissions.IsUseIds4 ? "Ids4" : "JWT"));
            }
            else
            {
                Console.WriteLine($"Current authorization scheme: " + (Permissions.IsUseIds4 ? "Ids4" : "JWT"));
            }

            // Caching AOP
            if (!AppSettings.app(new string[] { "AppSettings", "CachingAOP", "Enabled" }).ObjToBool())
            {
                Console.WriteLine($"Caching AOP: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"Caching AOP: True");
            }

            // Service Log AOP
            if (!AppSettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
            {
                Console.WriteLine($"Service Log AOP: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"Service Log AOP: True");
            }

            // Enabled middleware logs
            var requestResponseLogOpen = AppSettings.app(new string[] { "Middleware", "RequestResponseLog", "Enabled" }).ObjToBool();
            var ipLogOpen = AppSettings.app(new string[] { "Middleware", "IPLog", "Enabled" }).ObjToBool();
            var recordAccessLogsOpen = AppSettings.app(new string[] { "Middleware", "RecordAccessLogs", "Enabled" }).ObjToBool();
            ConsoleHelper.WriteSuccessLine($"Log Enabled: " +
                                           (requestResponseLogOpen ? "RequestResponseLog √," : "") +
                                           (ipLogOpen ? "IPLog √," : "") +
                                           (recordAccessLogsOpen ? "RecordAccessLogs √," : "")
            );

            // Transaction AOP
            if (!AppSettings.app(new string[] { "AppSettings", "TranAOP", "Enabled" }).ObjToBool())
            {
                Console.WriteLine($"Transaction AOP: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"Transaction AOP: True");
            }

            // Database SQL execution AOP
            if (!AppSettings.app(new string[] { "AppSettings", "SqlAOP", "OutToLogFile", "Enabled" }).ObjToBool())
            {
                Console.WriteLine($"DB Sql AOP To LogFile: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"DB Sql AOP To LogFile: True");
            }

            // Output SQL execution logs to console
            if (!AppSettings.app(new string[] { "AppSettings", "SqlAOP", "OutToConsole", "Enabled" }).ObjToBool())
            {
                Console.WriteLine($"DB Sql AOP To Console: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"DB Sql AOP To Console: True");
            }

            // SignalR data sending
            if (!AppSettings.app(new string[] { "Middleware", "SignalR", "Enabled" }).ObjToBool())
            {
                Console.WriteLine($"SignalR send data: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"SignalR send data: True");
            }

            // IP rate limiting
            if (!AppSettings.app("Middleware", "IpRateLimit", "Enabled").ObjToBool())
            {
                Console.WriteLine($"IpRateLimiting: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"IpRateLimiting: True");
            }

            // Performance profiling
            if (!AppSettings.app("Startup", "MiniProfiler", "Enabled").ObjToBool())
            {
                Console.WriteLine($"MiniProfiler: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"MiniProfiler: True");
            }

            // CORS Cross-Origin Resource Sharing
            if (!AppSettings.app("Startup", "Cors", "EnableAllIPs").ObjToBool())
            {
                Console.WriteLine($"EnableAllIPs For CORS: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"EnableAllIPs For CORS: True");
            }

            // Redis message queue
            if (!AppSettings.app("Startup", "RedisMq", "Enabled").ObjToBool())
            {
                Console.WriteLine($"Redis MQ: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"Redis MQ: True");
            }

            // RabbitMQ message queue
            if (!AppSettings.app("RabbitMQ", "Enabled").ObjToBool())
            {
                Console.WriteLine($"RabbitMQ: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"RabbitMQ: True");
            }

            // Consul service registration
            if (!AppSettings.app("Middleware", "Consul", "Enabled").ObjToBool())
            {
                Console.WriteLine($"Consul service: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"Consul service: True");
            }

            // EventBus event bus
            if (!AppSettings.app("EventBus", "Enabled").ObjToBool())
            {
                Console.WriteLine($"EventBus: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"EventBus: True");
            }

            // Multi-database
            if (!AppSettings.app(new string[] { "MutiDBEnabled" }).ObjToBool())
            {
                Console.WriteLine($"Is multi-DataBase: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"Is multi-DataBase: True");
            }

            // Read-write separation (CQRS)
            if (!AppSettings.app(new string[] { "CQRSEnabled" }).ObjToBool())
            {
                Console.WriteLine($"Is CQRS: False");
            }
            else
            {
                ConsoleHelper.WriteSuccessLine($"Is CQRS: True");
            }

            Console.WriteLine();
        }
    }

    public static void AddAppTableConfigSetup(this IServiceCollection services, IHostEnvironment env)
    {
        if (services == null) throw new ArgumentNullException(nameof(services));

        if (AppSettings.app(new string[] { "Startup", "AppConfigAlert", "Enabled" }).ObjToBool())
        {
            if (env.IsDevelopment())
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Console.OutputEncoding = Encoding.GetEncoding("GB2312");
            }

            #region Program Configuration

            List<string[]> configInfos = new()
                {
                    new string[] { "Current environment", Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") },
                    new string[] { "Current authorization scheme", Permissions.IsUseIds4 ? "Ids4" : "JWT" },
                    new string[] { "CORS Cross-Origin Resource Sharing", AppSettings.app("Startup", "Cors", "EnableAllIPs") },
                    new string[] { "RabbitMQ message queue", AppSettings.app("RabbitMQ", "Enabled") },
                    new string[] { "Event bus (requires message queue)", AppSettings.app("EventBus", "Enabled") },
                    new string[] { "Redis message queue", AppSettings.app("Startup", "RedisMq", "Enabled") },
                    new string[] { "Multi-database", AppSettings.app("MutiDBEnabled") },
                    new string[] { "Read-write separation (CQRS)", AppSettings.app("CQRSEnabled") },
                };

            new ConsoleTable()
            {
                TitleString = "Blog.Core Configuration Set",
                Columns = new string[] { "Configuration Name", "Configuration Info/Enabled" },
                Rows = configInfos,
                EnableCount = false,
                Alignment = Alignment.Left,
                ColumnBlankNum = 4,
                TableStyle = TableStyle.Alternative
            }.Writer(ConsoleColor.Blue);
            Console.WriteLine();

            #endregion Program Configuration

            #region AOP

            List<string[]> aopInfos = new()
                {
                    new string[] { "Caching AOP", AppSettings.app("AppSettings", "CachingAOP", "Enabled") },
                    new string[] { "Service Log AOP", AppSettings.app("AppSettings", "LogAOP", "Enabled") },
                    new string[] { "Transaction AOP", AppSettings.app("AppSettings", "TranAOP", "Enabled") },
                    new string[] { "SQL execution AOP", AppSettings.app("AppSettings", "SqlAOP", "Enabled") },
                    new string[] { "SQL execution AOP console output", AppSettings.app("AppSettings", "SqlAOP", "LogToConsole", "Enabled") },
                };

            new ConsoleTable
            {
                TitleString = "AOP",
                Columns = new string[] { "Configuration Name", "Configuration Info/Enabled" },
                Rows = aopInfos,
                EnableCount = false,
                Alignment = Alignment.Left,
                ColumnBlankNum = 7,
                TableStyle = TableStyle.Alternative
            }.Writer(ConsoleColor.Blue);
            Console.WriteLine();

            #endregion AOP

            #region Middleware

            List<string[]> MiddlewareInfos = new()
                {
                    new string[] { "Request recording middleware", AppSettings.app("Middleware", "RecordAccessLogs", "Enabled") },
                    new string[] { "IP logging middleware", AppSettings.app("Middleware", "IPLog", "Enabled") },
                    new string[] { "Request-response log middleware", AppSettings.app("Middleware", "RequestResponseLog", "Enabled") },
                    new string[] { "SingnalR real-time data sending middleware", AppSettings.app("Middleware", "SignalR", "Enabled") },
                    new string[] { "IP rate limiting middleware", AppSettings.app("Middleware", "IpRateLimit", "Enabled") },
                    new string[] { "Performance profiling middleware", AppSettings.app("Startup", "MiniProfiler", "Enabled") },
                    new string[] { "Consul service registration", AppSettings.app("Middleware", "Consul", "Enabled") },
                };

            new ConsoleTable
            {
                TitleString = "Middleware",
                Columns = new string[] { "Configuration Name", "Configuration Info/Enabled" },
                Rows = MiddlewareInfos,
                EnableCount = false,
                Alignment = Alignment.Left,
                ColumnBlankNum = 3,
                TableStyle = TableStyle.Alternative
            }.Writer(ConsoleColor.Blue);
            Console.WriteLine();

            #endregion Middleware
        }
    }
}
