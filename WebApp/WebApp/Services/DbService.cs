using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApp.Controllers;
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
            
            UpdateData();
        }

        public async Task<List<string>> GetCategoriesAsync()
        {
            var rows = await _dbSession.ExecuteAsync(
                new SimpleStatement($"SELECT category FROM api_data"));
            var records = rows.Select(x => x.GetValue<string>("category")).Distinct().ToList();
            return records;
        }

        public async Task<List<ApiData>> GetCategoryAsync(string name)
        {
            var rows = await _dbSession.ExecuteAsync(
                new SimpleStatement($"SELECT * FROM api_data WHERE category='{name}' ALLOW FILTERING"));
            var records = rows.Select(FromRow).ToList();
            return records;
        }

        private static ApiData FromRow(Row row)
        {
            return new()
            {
                Name = row.GetValue<string>("name"),
                Description = row.GetValue<string>("description"),
                Category = row.GetValue<string>("category"),
                Endpoint = row.GetValue<string>("endpoint"),
            };
        }

        private void UpdateData()
        {
            var jsonApis =
                (JsonConvert.DeserializeObject<List<ApiData>>(File.ReadAllText("Data/API-list.json")) ??
                 new List<ApiData>()).Distinct().ToList();

            var serverApis = _dbSession.Execute("SELECT * FROM api_data")
                .Select(row => row.GetValue<string>("name")).ToList();

            var newApis = jsonApis.Where(x => !serverApis.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase)).Distinct().ToArray();
            var baseQuery =
                _dbSession.Prepare("INSERT INTO api_data (name, description, category, endpoint) VALUES (?, ?, ?, ?)");

            var batch = new BatchStatement();
            foreach (var api in newApis)
            {
                batch.Add(baseQuery.Bind(api.Name, api.Description, api.Category, api.Endpoint));
            }

            _dbSession.Execute(batch);
        }
    }
}