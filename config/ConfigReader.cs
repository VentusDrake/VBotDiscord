using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace VBotDiscord.config {
    internal static class ConfigReader {
         public static BotConfig LoadConfig(string path = "config/config.json") {
            var fullPath = Path.Combine(AppContext.BaseDirectory, path);
            var json = File.ReadAllText(fullPath);
            return JsonSerializer.Deserialize<BotConfig>(json);
        }
    }

    internal class BotConfig {
        public string Token { get; set; }
        public string Prefix { get; set; }
    }
}
