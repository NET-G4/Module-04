using Newtonsoft.Json;

namespace Lesson07.Models
{
    public class University
    {
        [JsonProperty("state-province")]
        public string stateprovince { get; set; }
        public string country { get; set; }
        public string name { get; set; }
    }
}
