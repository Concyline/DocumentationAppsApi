using DocumentationAppsApi.Src.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace DocumentationAppsApi.Src.MongoConfig
{
    public static class MongoDbServiceExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
        {
            var mongoConfig = new MongoDbConfig();
            configuration.GetSection("MongoDb").Bind(mongoConfig);

            services.AddSingleton<IMongoClient>(sp => new MongoClient(mongoConfig.ConnectionString));

            services.AddSingleton(sp =>
            {
                var client = sp.GetRequiredService<IMongoClient>();
                return client.GetDatabase(mongoConfig.DatabaseName);
            });

            // REGISTRO DA COLLECTION DE USUÁRIO
            services.AddSingleton(sp =>
            {
                var db = sp.GetRequiredService<IMongoDatabase>();
                return db.GetCollection<User>("Users");
            });

            return services;
        }
    }
}

