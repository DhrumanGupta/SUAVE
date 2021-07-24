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

        /// <summary>
        /// Gets all the categories available
        /// </summary>
        [HttpGet]
        [Route("category")]
        public async Task<IActionResult> GetCategoriesAsync()
        {
            var records = await _dbService.GetCategoriesAsync();

            if (!records.Any())
                return NotFound("The database is under maintenance, please try again later");

            return Ok(records);
        }

        /// <summary>
        /// Gets all the APIs found for the category provided
        /// </summary>
        /// <param name="categoryName">The category to find for</param>
        [HttpGet]
        [Route("category/{categoryName}")]
        public async Task<IActionResult> GetCategoryAsync(string categoryName)
        {
            var records = await _dbService.GetCategoryAsync(categoryName);

            if (!records.Any())
                return NotFound("No APIs found for the category provided");

            return Ok(records);
        }
    }
}