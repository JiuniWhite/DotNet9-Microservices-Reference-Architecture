using MongoDB.Driver;
using Provider.Core.Domain.Abstractions.Aggregates;
using Provider.Infrastructure.Data;

namespace Provider.Api.Infrastructures.Databases.Context
{
    public class MongoContext : IMongoContext
    {
        private readonly IMongoDatabase _database;

        public MongoContext(IConfiguration configuration)
        {
            // 1. Obtenemos la cadena de conexión
            var connectionString = configuration["CatalogDatabaseSettings:ConnectionString"];

            // 2. Obtenemos el nombre de la base de datos (debe estar en el appsettings)
            var databaseName = configuration["CatalogDatabaseSettings:DatabaseName"];

            if (string.IsNullOrEmpty(connectionString) || string.IsNullOrEmpty(databaseName))
            {
                throw new Exception("Falta la configuración de MongoDB (ConnectionString o DatabaseName) en appsettings.json");
            }

            var mongoClient = new MongoClient(connectionString);

            // 3. Conexión fija a la base de datos
            _database = mongoClient.GetDatabase(databaseName);

            Console.WriteLine($"Conectado a MongoDB: {databaseName}");
        }

        public IMongoCollection<TCollection> Collection<TCollection>()
            where TCollection : class, IAggregateRoot
        {
            // Usamos el nombre de la clase como nombre de la colección por defecto
            return _database.GetCollection<TCollection>(typeof(TCollection).Name);
        }
    }
}