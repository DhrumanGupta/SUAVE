using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.Extensions.Configuration;
using WebApp.Models;

namespace WebApp.Services
{
    public class DbService
    {
        private readonly ISession _dbSession;

        public DbService(IConfiguration configuration)
        {
            var section = configuration.GetSection("DbData");
            _dbSession =
                Cluster.Builder()
                    .WithCloudSecureConnectionBundle(section.GetValue<string>("BundlePath"))
                    .WithCredentials(section.GetValue<string>("ClientId"), section.GetValue<string>("ClientPassword"))
                    .Build()
                    .Connect(section.GetValue<string>("Keyspace"));
        }

        public async Task<List<ApiData>> GetFromCategoryAsync(string name)
        {
            var rows = await _dbSession.ExecuteAsync(
                new SimpleStatement($"SELECT * FROM api_data WHERE category='{name}' ALLOW FILTERING;"));
            var records = rows.Select(FromRow).ToList();
            return records;
        }

        private static ApiData FromRow(Row row)
        {
            return new()
            {
                Category = row.GetValue<string>("category"),
                Description = row.GetValue<string>("description"),
                Endpoint = row.GetValue<string>("endpoint"),
                Name = row.GetValue<string>("name")
            };
        }
    }
}