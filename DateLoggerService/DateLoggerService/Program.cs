using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using DateLoggerService;

Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((hostContext, services) =>
    {
        services.Configure<TimeInterval>(
            hostContext.Configuration.GetSection("TimeInterval"));

        services.AddHostedService<Worker>();
    })
    .Build()
    .Run();

//sc create DateLoggerService binPath= "A:\NationalTechnology\Task\DateLoggerService\DateLoggerService\bin\Debug\net6.0\DateLoggerService.exe" start= auto DisplayName= "Date Logger Service"
