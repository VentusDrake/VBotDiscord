using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace VBotDiscord.other {
    public class AnimeQuotes {
        [JsonPropertyName("character")]
        public string Character { get; set; }
        [JsonPropertyName("quote")]
        public string Quote { get; set; }
        [JsonPropertyName("show")]
        public string Show { get; set; }
    }
}
