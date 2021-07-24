using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using WebApp.Controllers;
using WebApp.Models;

namespace WebApp.Services
{
    public class DbService
    {
        private readonly ISession _dbSession;

        private ApiData[] _apiData;
        private DateTime _nextFetchTime;

        public DbService(IConfiguration configuration)
        {
            _dbSession =
                Cluster.Builder()
                    .WithCloudSecureConnectionBundle(configuration.GetValue<string>("BundlePath"))
                    .WithCredentials(configuration.GetValue<string>("ClientId"),
                        configuration.GetValue<string>("ClientPassword"))
                    .Build()
                    .Connect(configuration.GetValue<string>("Keyspace"));
            
            UpdateData();
        }

        public List<string> GetCategories()
        {
            CheckAndFetch();
            return _apiData.Select(x => x.Category).Distinct().ToList();
        }

        public List<ApiData> GetCategoriesFromUse(string[] uses, int limit)
        {
            CheckAndFetch();
            var scoreByName = new Dictionary<ApiData, int>();
            foreach (var api in _apiData)
            {
                scoreByName.Add(api, 0);
                foreach (var use in uses)
                {
                    if (api.Name.Contains(use, StringComparison.InvariantCultureIgnoreCase))
                        scoreByName[api] += 2;


                    if (api.Category.Contains(use, StringComparison.InvariantCultureIgnoreCase))
                        scoreByName[api] += 2;


                    if (api.Description.Contains(use, StringComparison.InvariantCultureIgnoreCase))
                        scoreByName[api] += 1;
                }
            }

            scoreByName = scoreByName.Where(x => x.Value > 1).OrderBy(key => key.Value).Reverse()
                .ToDictionary(x => x.Key, x => x.Value);

            var apis = scoreByName.Select(x => x.Key).Take(limit).ToList();
            return apis;
        }


        public List<ApiData> GetCategoryData(string name)
        {
            CheckAndFetch();
            return _apiData.Where(x => x.Category.Equals(name, StringComparison.InvariantCultureIgnoreCase)).ToList();
        }

        private void UpdateData()
        {
            var jsonApis = JsonConvert
                .DeserializeObject<List<ApiData>>(File.ReadAllText("Data/API-list.json"))!
                .Distinct()
                .ToList();

            _apiData = _dbSession.Execute("SELECT * FROM api_data").Select(FromRow).ToArray();

            var serverApis = _apiData.Select(x => x.Name);

            var newApis = jsonApis.Where(x => !serverApis.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase))
                .Distinct().ToArray();
            var baseQuery =
                _dbSession.Prepare("INSERT INTO api_data (name, description, category, endpoint) VALUES (?, ?, ?, ?)");

            var batch = new BatchStatement();
            foreach (var api in newApis)
            {
                batch.Add(baseQuery.Bind(api.Name, api.Description, api.Category, api.Endpoint));
            }

            _dbSession.Execute(batch);

            SetNextFetchTime();
        }


        private void CheckAndFetch()
        {
            if (DateTime.Now > _nextFetchTime)
            {
                _apiData = _dbSession.Execute("SELECT * FROM api_data").Select(FromRow).ToArray();
            }

            SetNextFetchTime();
        }

        private void SetNextFetchTime()
        {
            _nextFetchTime = DateTime.Now.AddMinutes(30);
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
    }
}