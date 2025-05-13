using DateLoggerService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text.Json;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly int _intervalMinutes;
    private readonly string _logPath = @"A:\NationalTechnology\Task\log.txt";
    private readonly string _apiUrl = "https://localhost:7064/api/MathOperations/AddNumbers"; // the url for add number

    public Worker(ILogger<Worker> logger, IOptions<TimeInterval> config, IHttpClientFactory httpClientFactory)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _intervalMinutes = config.Value.IntervalMinutes;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_logPath)); // to ensure the directory for the log file exists

        while (!stoppingToken.IsCancellationRequested) // start the while loop when service is running
        {
            try
            {
                var numberData = new //generat random numbers
                {
                    a = new Random().Next(1, 100),
                    b = new Random().Next(1, 100)
                };

                var httpClient = _httpClientFactory.CreateClient();
                var content = new StringContent(JsonSerializer.Serialize(numberData), Encoding.UTF8, "application/json"); //serialization : to convert the number into json format

                var response = await httpClient.PostAsync(_apiUrl, content, stoppingToken); // send the numbers to the endpoint
                string result = await response.Content.ReadAsStringAsync(stoppingToken);

                string log = $"[{DateTime.Now}] The Two numbers: {numberData.a}, {numberData.b}. Response = {result}";
                await File.AppendAllTextAsync(_logPath, log + Environment.NewLine, stoppingToken);
                _logger.LogInformation(log); // print the responce into the txt file
            }
            catch (Exception ex)
            {
                File.AppendAllText(@"C:\Temp\service-error.log", ex.ToString() + Environment.NewLine); // to print the exeption in file
            }

            await Task.Delay(TimeSpan.FromMinutes(_intervalMinutes), stoppingToken); // repeat every 5 min or as what in the interval
        }
    }
}