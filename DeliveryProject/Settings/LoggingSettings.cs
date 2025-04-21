using Serilog.Events;

namespace DeliveryProject.Settings;

public class LoggingSettings
{
    public LogEventLevel DefaultLogLevel { get; set; }
    public LogEventLevel AspNetCoreLogLevel { get; set; }
    public LogEventLevel ConsoleLogLevel { get; set; }
    public LogEventLevel ElasticSearchLogLevel { get; set; }
    public LogEventLevel FileLogLevel { get; set; }
    
    public string FilePath { get; set; }
}