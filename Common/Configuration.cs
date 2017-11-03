﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Discord;

using MelissasCode;

namespace DiscordBot.Common
{
    public class Configuration
    {
        [JsonIgnore]
        public static string FileName { get; } = "config/configuration.json";

        public string BotToken { get; set; } = null;
        
        public ulong Developer { get; set; } = 149991092337639424;
        public string Playing { get; set; } = null;
        public string TwitchLink { get; set; } = null;
        public UserStatus Status { get; set; } = UserStatus.Online;
        
        public bool UnknownCommandEnabled { get; set; } = true;
        public int LeaderboardAmount { get; set; } = 5;
        public int QuoteCost { get; set; } = 250;
        public int PrefixCost { get; set; } = 2500;
        public int SenpaiChanceRate { get; set; } = 5;

        public ulong LogChannelId { get; set; } = 313318109500932096;

        /// NSFW Variables
        public int MaxRuleXGamble { get; set; } = 2353312;

        /// Fun Variables
        public int Respects { get; set; } = 0;

        public int MinLengthForCoin { get; set; } = 0; // Magic Number entered via Discord. (Not Shared for Reasons)
        
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
                
                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("ok");
                Console.ResetColor();
                Console.WriteLine("]    " + FileName + ": created.");
            }
            
            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]    " + FileName + ": loaded.");
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
        public static void UpdateJson(string parameterName, ulong newValue)
        {
            string json = File.ReadAllText(FileName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(FileName, output);
        }
    }
}
