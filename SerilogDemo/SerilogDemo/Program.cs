using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Formatting;
using SerilogDemo.Logger;

namespace SerilogDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", true, true)
               .Build();

                var path = configuration.GetSection("Serilog:path");
                Log.Logger = CommonLogger.GetLogger(path.Value == null ? "" : path.Value);

                CreateWebHostBuilder(args).Build().Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Failed to start application");
            }
            finally
            {
                Log.CloseAndFlush();
            }

        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .UseSerilog()
            .UseStartup<Startup>();
    }

    public class OutputTextFormatter : ITextFormatter
    {
        public void Format(LogEvent logEvent, TextWriter output)
        {
            const string outputTemplate = "{Timestamp:HH:mm} [{Level}] {MachineName} {EnvironmentUserName} ({ThreadId}) ({ProcessId}) {SourceContext}  {Message}{NewLine}{Exception}";

            //output.Write("Timestamp - {0} | Level - {1} | Message {2} {3}", logEvent.Timestamp, logEvent.Level, logEvent.MessageTemplate, output.NewLine);
            output.Write(outputTemplate);
            //if (logEvent.Exception != null)
            //{
            //    output.Write("Exception - {0}", logEvent.Exception);
            //}
        }
    }
}
