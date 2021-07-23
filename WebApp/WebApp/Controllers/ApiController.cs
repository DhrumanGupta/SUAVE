using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cassandra;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using WebApp.Models;
using WebApp.Services;

namespace WebApp.Controllers
{
    [Route("api")]
    public class ApiController : ControllerBase
    {
        private readonly DbService _dbService;

        public ApiController(DbService dbService)
        {
            _dbService = dbService;
        }

        [HttpGet]
        [Route("category/{categoryName}")]
        public async Task<IActionResult> GetCategoryAsync(string categoryName)
        {
            var records = await _dbService.GetFromCategoryAsync(categoryName);

            if (!records.Any())
                return NotFound("No APIs found for the category provided");

            return Ok(records);
        }
    }
}