using System;
using Newtonsoft.Json;

namespace test.Models
{
    public class UserCommand
    {
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
    }
}
