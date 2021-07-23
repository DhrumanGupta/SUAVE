using System;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
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
                    .Connect("suave");
        }

        [HttpGet]
        public async Task<ApiData> GetAsync()
        {
            var rows = _dbSession.Execute("select * from suave.data");
            foreach (var x in rows)
            {
                Console.WriteLine(x.GetValue<string>("key"));
            }

            return new ApiData();
        }
    }
}