using DateLoggerService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly int _intervalMinutes;
    private readonly string _filePath = @"A:\NationalTechnology\Task\log.txt";

    public Worker(ILogger<Worker> logger, IOptions<TimeInterval> config)
    {
        _logger = logger;
        _intervalMinutes = config.Value.IntervalMinutes;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_filePath));

            while (!stoppingToken.IsCancellationRequested)
            {
                string logMessage = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Task executed.";
                await File.AppendAllTextAsync(_filePath, logMessage + Environment.NewLine);
                _logger.LogInformation(logMessage);

                await Task.Delay(TimeSpan.FromMinutes(_intervalMinutes), stoppingToken);
            }
        }
        catch (Exception ex)
        {
            File.AppendAllText(@"C:\Temp\service-error.log", ex.ToString());
        }
    }
}