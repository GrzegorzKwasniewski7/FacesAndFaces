using EmailService;
using GreenPipes;
using MassTransit;
using Messaging.InterfacesConstants.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NotificationService.Consumers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace NotificationService
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            await host.RunAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder(args)
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.SetBasePath(Directory.GetCurrentDirectory());
                    configHost.AddJsonFile($"appsettings.json", optional: false);
                    configHost.AddEnvironmentVariables();
                    configHost.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((hostContext,config) =>
                {
                    config.AddJsonFile($"appsettings.{hostContext.HostingEnvironment.EnvironmentName}.json",
                        optional: false);
                })


               .ConfigureServices((hostContext, services) =>
               {
                   var emailConfig = hostContext.Configuration
                   .GetSection("EmailConfiguration")
                   .Get<EmailConfig>();
                   services.AddSingleton(emailConfig);
                   services.AddScoped<IEmailSender, EmailSender>();
                   services.AddMassTransit(c =>
                   {
                       c.AddConsumer<OrderProcessedEventConsumer>();
                       c.AddBus(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                       {
                           //cfg.UseHealthCheck(provider);
                           cfg.Host(new Uri("rabbitmq://localhost"), h =>
                           {
                               h.Username("guest");
                               h.Password("guest");
                           });
                           cfg.ReceiveEndpoint(RabbitMqMassTransitConstants.NotificationServiceQueue, ep =>
                           {
                               ep.PrefetchCount = 16;
                               ep.UseMessageRetry(r => r.Interval(2, 100));
                               ep.ConfigureConsumer<OrderProcessedEventConsumer>(provider);
                           });                          

                       }));
                   });

                  // services.AddSingleton(provider => Bus.Factory.CreateUsingRabbitMq(cfg =>
                  // {  
                  //   cfg.Host("localhost", "/", h => { });
                  //    cfg.ReceiveEndpoint(RabbitMqMassTransitConstants.NotificationServiceQueue, e =>
                  //    {
                  //        e.PrefetchCount = 16;
                  //        e.UseMessageRetry(x => x.Interval(2, TimeSpan.FromSeconds(10)));
                  //        e.Consumer<OrderProcessedEventConsumer>(provider);
                  //    });
                  //    cfg.ConfigureEndpoints(provider);
                  //}));
                   services.AddSingleton<IHostedService, BusService>();

               });
            return hostBuilder;
        }
    }
}
