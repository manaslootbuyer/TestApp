using System;
using Newtonsoft.Json;

namespace test.Models
{
    public class UserModel
    {
        [JsonProperty(PropertyName = "name")]
        public string PhotoPath { get; set; }
    }
}
