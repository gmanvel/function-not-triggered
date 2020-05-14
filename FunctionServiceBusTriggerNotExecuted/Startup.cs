using System;
using FunctionServiceBusTriggerNotExecuted;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

[assembly: FunctionsStartup(typeof(Startup))]

namespace FunctionServiceBusTriggerNotExecuted
{
    public class Startup: FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var serviceProvider = builder.Services.BuildServiceProvider();

            var executionContextOptions = serviceProvider.GetService<IOptions<ExecutionContextOptions>>().Value;

            var currentDirectory = executionContextOptions.AppDirectory;

            var existingConfiguration = serviceProvider.GetService<IConfiguration>();

            var appSettingsConfig = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ENVIRONMENT")}.json", optional: true)
                .AddConfiguration(existingConfiguration)
                .AddEnvironmentVariables()
                .Build();

            var services = builder.Services;

            services.Replace(ServiceDescriptor.Singleton(typeof(IConfiguration), appSettingsConfig));

            services.AddOptions<ServiceBusSettings>()
                .Configure<IConfiguration>((settings, configuration) => { appSettingsConfig.Bind(settings); });

            services.Configure<ServiceBusSettings>(appSettingsConfig.GetSection("ServiceBus"));
            services.Configure<QueueNamesConfiguration>(appSettingsConfig.GetSection("QueueNamesConfiguration"));

            services.AddSingleton<INameResolver>(new CustomNameResolver(appSettingsConfig));
        }
    }
}