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

        /// NSFW Server Variables
        // Rule 34 Gamble Game Variables
        public int MaxRuleXGamble { get; set; } = 2353312;
        public ulong NSFWServerID { get; set; } = 225737373307109377;
        public ulong RuleGambleChannelID { get; set; } = 268911937977450498;

        /// MogiiCraft Variables
        // Server Variables
        public ulong ServerID { get; set; } = 221250721046069249;
        // Channel Variables
        public ulong WelcomeChannelID { get; set; } = 225721556435730433;
        public ulong LogChannelID { get; set; } = 235179833053675522;
        public ulong MinecraftChannelID { get; set; } = 221292894047174676;
        // Other Variables
        public string welcomeMessage { get; set; } = "Hey {USERJOINED}, and welcome to the {GUILDNAME} Discord Server! If you are a player on our Minecraft Server, tell us your username and a Staff Member will grant you the MC Players Role \n\nIf you don't mind, could you fill out this form linked below. We are collecting data on how you found out about us, and it'd be great if we had your input. The form can be found here: <https://goo.gl/forms/iA9t5xjoZvnLJ5np1>";
        public ulong SupportChannelID { get; set; } = 309630743825612800;
        public ulong SuggestChannelID { get; set; } = 310102597014913024;
        public string musicLink { get; set; } = "";

        // Secret Stuff
        public ulong ListenForBot { get; set; } = 307953847207460865;
        public ulong ForwardMessagesTo { get; set; } = 267832190652514305;
                
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

                Console.WriteLine(FileName + " Created.");
            }
            Console.WriteLine(FileName + " Loaded");
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
