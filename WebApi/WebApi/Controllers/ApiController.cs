using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        private readonly ISession _dbSession;

        public ApiController(IConfiguration configuration)
        {
            var section = configuration.GetSection("DbData");
            _dbSession =
                Cluster.Builder()
                    .WithCloudSecureConnectionBundle(section.GetValue<string>("BundlePath"))
                    .WithCredentials(section.GetValue<string>("ClientId"), section.GetValue<string>("ClientPassword"))
                    .Build()
                    .Connect(section.GetValue<string>("Keyspace"));
        }

        [HttpGet]
        [Route("category/{name}")]
        public async Task<IActionResult> GetCategoryAsync(string name)
        {
            var rows = await _dbSession.ExecuteAsync(new SimpleStatement($"SELECT * FROM api_data WHERE category='{name}'"));

            if (!rows.Any())
            {
                NotFound("No APIs found for the category provided");
            }
            
            return Ok(rows.Select(FromRow));
        }

        private ApiData FromRow(Row row)
        {
            return new()
            {
                Id = Guid.Parse(row.GetValue<string>("id")),
                Category = row.GetValue<string>("category"),
                Description = row.GetValue<string>("description"),
                Endpoint = row.GetValue<string>("endpoint"),
                Name = row.GetValue<string>("name")
            };
        }
    }
}