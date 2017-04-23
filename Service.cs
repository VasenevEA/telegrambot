using System;
using System.IO;
using telegrambot.Models;
using Newtonsoft.Json;

namespace telegrambot
{
    public static class Service
    {
        private static string configName = "config.txt";

        public static Config readConfig()
        {         
            var text = File.ReadAllText(configName);
            Config config = JsonConvert.DeserializeObject<Config>(text);

            return config;
        }
    }
}
