using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Exceptions;
using Serilog.Filters;

namespace SerilogDemo.Logger
{
    public class CommonLogger
    {
        public static ILogger GetLogger(string path)
        {
            Log.Logger = new LoggerConfiguration()
                    //.ReadFrom.Configuration(configuration)
                    .WriteTo.Console()
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(Matching.WithProperty("RequestBody"))
                            .WriteTo.File(path + @"Info\API-.log",
                                            rollingInterval: RollingInterval.Day,
                                            fileSizeLimitBytes: 1024 * 1024 * 10,
                                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{MachineName}] {Message:lj}{NewLine}{Exception}",
                                            rollOnFileSizeLimit: true

                                        ).Enrich.WithEnvironmentUserName().Enrich.WithMachineName()
                                    )
                    .WriteTo.Logger(l => l.Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                            .WriteTo.File(path + @"Info\Info-.log",
                                            rollingInterval: RollingInterval.Day,
                                            fileSizeLimitBytes: 1024 * 1024 * 10,
                                            outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{MachineName}] {Message:lj}{NewLine}{Exception}",
                                            rollOnFileSizeLimit: true

                                        ).Enrich.WithEnvironmentUserName().Enrich.WithMachineName()
                                    )
                    .WriteTo.Logger(l => l.Filter.ByExcluding(e => e.Level == LogEventLevel.Information)
                            .WriteTo.File(path + @"Error\Error-.log",
                                            rollingInterval: RollingInterval.Day,
                                            fileSizeLimitBytes: 1024 * 1024 * 10,
                                            //outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] [{MachineName}] {Message:lj}{NewLine}{Exception}",
                                            rollOnFileSizeLimit: true

                                        ).Enrich.WithEnvironmentUserName().Enrich.WithMachineName().Enrich.WithExceptionDetails()
                                    )
                .CreateLogger();
            return Log.Logger;
        }
    }
}
