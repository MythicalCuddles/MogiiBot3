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
        public bool UnknownCommandEnabled { get; set; } = true;
        public bool QuotesEnabled { get; set; } = true;
        public int LeaderboardAmount { get; set; } = 5;
        public int CoinToChipRatio { get; set; } = 10; // Exchange 10 coins for 1 chip.
        public int ChipToCoinRatio { get; set; } = 8; // Exchange 1 chip for 8 coins.

        /// NSFW Server Variables
        // Rule 34 Gamble Game Variables
        public int MaxRuleXGamble { get; set; } = 2353312;
        public ulong NSFWServerID { get; set; } = 225737373307109377;
        public ulong RuleGambleChannelID { get; set; } = 268911937977450498;

        /// MogiiCraft Variables
        // Server Variables
        public ulong ServerID { get; set; } = 221250721046069249;
        // Channel Variables
        public ulong MCWelcomeChannelID { get; set; } = 225721556435730433;
        public ulong MCLogChannelID { get; set; } = 235179833053675522;
        public ulong MCMinecraftChannelID { get; set; } = 221292894047174676;
        // Other Variables
        public string welcomeMessage { get; set; } = "Hey {USERJOINED}, and welcome to the {GUILDNAME} Discord Server! If you are a player on our Minecraft Server, tell us your username and a Staff Member will grant you the MC Players Role \n\nIf you don't mind, could you fill out this form linked below. We are collecting data on how you found out about us, and it'd be great if we had your input. The form can be found here: <https://goo.gl/forms/iA9t5xjoZvnLJ5np1>";
        public ulong SupportChannelID { get; set; } = 309630743825612800;
        public ulong SuggestChannelID { get; set; } = 310102597014913024;
        public ulong LogChannelID { get; set; } = 313318109500932096;

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
