using System;
using Newtonsoft.Json;

namespace WebApp.Models
{
    public class ApiData
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("link")]
        public string Endpoint { get; set; }
        
        [JsonProperty("category")]
        public string Category { get; set; }
    }
}