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
        // private readonly ISession _dbSession;

        private readonly ApiData[] _apiDatas;

        public DbService()
        {
            // _dbSession =
            //     Cluster.Builder()
            //         .WithCloudSecureConnectionBundle(configuration.GetValue<string>("BundlePath"))
            //         .WithCredentials(configuration.GetValue<string>("ClientId"), configuration.GetValue<string>("ClientPassword"))
            //         .Build()
            //         .Connect(configuration.GetValue<string>("Keyspace"));

            // UpdateData();
            _apiDatas = JsonConvert.DeserializeObject<List<ApiData>>(File.ReadAllText("Data/API-list.json")).Distinct()
                .ToArray();
        }

        public List<string> GetCategories()
        {
            return _apiDatas.Select(x => x.Category).Distinct().ToList();
        }

        public List<ApiData> GetCategoriesFromUse(string[] uses, int limit)
        {
            var _scoreByName = new Dictionary<ApiData, int>();
            foreach (var api in _apiDatas)
            {
                _scoreByName.Add(api, 0);
                foreach (var use in uses)
                {
                    if (api.Name.Contains(use, StringComparison.InvariantCultureIgnoreCase))
                    {
                        _scoreByName[api] += 2;
                    }

                    if (api.Category.Contains(use, StringComparison.InvariantCultureIgnoreCase))
                    {
                        _scoreByName[api] += 2;
                    }

                    if (api.Description.Contains(use, StringComparison.InvariantCultureIgnoreCase))
                    {
                        _scoreByName[api] += 1;
                    }
                }
            }

            _scoreByName = _scoreByName.Where(x => x.Value > 1).OrderBy(key => key.Value).Reverse().ToDictionary(x=> x.Key, x=> x.Value);


            if (_scoreByName.Count <= 0)
            {
                return new List<ApiData>();
            }

            var apis = _scoreByName.Select(x => x.Key).Take(limit).ToList();
            return apis;
        }

        public List<ApiData> GetCategoryData(string name)
        {
            return _apiDatas.Where(x => x.Category == x.Name).ToList();
        }

        // private void UpdateData()
        // {
        //     var jsonApis =
        //         (JsonConvert.DeserializeObject<List<ApiData>>(File.ReadAllText("Data/API-list.json")) ??
        //          new List<ApiData>()).Distinct().ToList();
        //
        //     var serverApis = _dbSession.Execute("SELECT * FROM api_data")
        //         .Select(row => row.GetValue<string>("name")).ToList();
        //
        //     var newApis = jsonApis.Where(x => !serverApis.Contains(x.Name, StringComparer.InvariantCultureIgnoreCase)).Distinct().ToArray();
        //     var baseQuery =
        //         _dbSession.Prepare("INSERT INTO api_data (name, description, category, endpoint) VALUES (?, ?, ?, ?)");
        //
        //     var batch = new BatchStatement();
        //     foreach (var api in newApis)
        //     {
        //         batch.Add(baseQuery.Bind(api.Name, api.Description, api.Category, api.Endpoint));
        //     }
        //
        //     _dbSession.Execute(batch);
        // }
    }
}