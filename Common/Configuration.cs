using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelissasCode;
using Discord;

namespace DiscordBot.Common
{
    public class Configuration
    {
        [JsonIgnore]
        public static string FileName { get; private set; } = "config/configuration.json";

        public ulong Developer { get; set; } = DiscordWorker.getMelissaID;
        public string Prefix { get; set; } = "$";
        public string Playing { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Online;
        public bool SenpaiEnabled { get; set; } = true;
        /// MogiiCraft Variables
        // Server Variables
        public ulong ServerID { get; set; } = 221250721046069249;
        // Channel Variables
        public ulong WelcomeChannelID { get; set; } = 225721556435730433;
        public ulong LogChannelID { get; set; } = 235179833053675522;

        public static void EnsureExists()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var config = new Configuration();
                config.SaveJson();

                Console.WriteLine("Configuration Created.");
            }
            Console.WriteLine("Configuration Loaded");
        }
        
        public void SaveJson()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            File.WriteAllText(file, ToJson());
        }
        
        public static Configuration Load()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            return JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(file));
        }
        
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static void UpdateJson(string parameterName, string newValue)
        {
            string json = File.ReadAllText(FileName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(FileName, output);
        }
        public static void UpdateJson(string parameterName, int newValue)
        {
            string json = File.ReadAllText(FileName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(FileName, output);
        }
        public static void UpdateJson(string parameterName, bool newValue)
        {
            string json = File.ReadAllText(FileName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(FileName, output);
        }
    }
}
