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
        public IActionResult GetCategoriesAsync()
        {
            var records = _dbService.GetCategories();

            if (!records.Any())
                return NotFound("The database is under maintenance, please try again later");

            return Ok(records);
        }

        /// <summary>
        /// Gets all the APIs found for the use provided
        /// </summary>
        /// <param name="categoryName">The category to find for</param>
        [HttpGet]
        [Route("category/{categoryName}")]
        public IActionResult GetCategoryAsync(string categoryName)
        {
            var records = _dbService.GetCategoryData(categoryName);

            if (!records.Any())
                return NotFound("No APIs found for the category provided");

            return Ok(records);
        }

        /// <summary>
        /// Gets all the APIs found for the use provided
        /// </summary>
        /// <param name="use">The use of the api</param>
        /// <param name="maxNum">The maximum no. of APIs to show (defaults to 5)</param>
        [HttpGet]
        [Route("{use}")]
        public IActionResult GetFromUse(string use, int maxNum = 5)
        {
            if (string.IsNullOrEmpty(use))
            {
                return BadRequest("No use provided");
            }
            var words = use.Split(' ').Where(word => word.Length > 3).ToArray();

            var records = _dbService.GetCategoriesFromUse(words, maxNum);

            if (!records.Any())
                return NotFound("No APIs found for the use provided");
            
            return Ok(records);
        }
    }
}