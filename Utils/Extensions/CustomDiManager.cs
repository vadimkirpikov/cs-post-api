

using Confluent.Kafka;
using CsPostApi.Repositories.Implementations;
using CsPostApi.Repositories.Interfaces;
using CsPostApi.Services.Implementations;
using CsPostApi.Services.Interfaces;

namespace CsPostApi.Utils.Extensions;

public static class CustomDiManager
{
    public static WebApplicationBuilder InjectDependencies(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddScoped<IProducer<Null, string>>(sp =>
            {
                var config = new ProducerConfig()
                {
                    BootstrapServers = builder.Configuration["Kafka:BootstrapServers"],
                };
                var client = new ProducerBuilder<Null, string>(config).Build();
                return client;
            })
            .AddScoped<IPostRepository, PostRepository>()
            .AddScoped<IPostService, PostService>()
            .AddScoped<INotifier, Notifier>();
        return builder;
    }
}