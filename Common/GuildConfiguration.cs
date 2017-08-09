using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using MelissasCode;
using Discord;
using Discord.WebSocket;

namespace DiscordBot.Common
{
    public class GuildConfiguration
    {
        public string Prefix { get; set; } = "$";
        public string WelcomeMessage { get; set; } = null;
        public ulong WelcomeChannelId { get; set; } = 0;
        public ulong LogChannelId { get; set; } = 0;

        public bool SenpaiEnabled { get; set; } = true;
        public bool QuotesEnabled { get; set; } = true;

        public bool EnableNSFWCommands { get; set; } = false;
        public ulong RuleGambleChannelId { get; set; } = 0;

        public static void EnsureExists(ulong GuildID)
        {
            string file = Path.Combine(AppContext.BaseDirectory, GetPath(GuildID));
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var guildConfig = new GuildConfiguration();
                guildConfig.SaveJson(GuildID);

                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("ok");
                Console.ResetColor();
                Console.WriteLine("]    " + GetPath(GuildID) + ": created.");
            }

            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]    " + GetPath(GuildID) + ": loaded.");
        }

        private static string GetPath(ulong GuildID)
        {
            return ("config/guilds/" + GuildID + ".json");
        }

        public void SaveJson(ulong GuildID)
        {
            string file = Path.Combine(AppContext.BaseDirectory, GetPath(GuildID));
            File.WriteAllText(file, ToJson());
        }

        public static GuildConfiguration Load(ulong GuildID)
        {
            string file = Path.Combine(AppContext.BaseDirectory, GetPath(GuildID));
            return JsonConvert.DeserializeObject<GuildConfiguration>(File.ReadAllText(file));
        }

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static void UpdateJson(ulong GuildID, string parameterName, string newValue)
        {
            string json = File.ReadAllText(GetPath(GuildID));
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(GetPath(GuildID), output);
        }
        public static void UpdateJson(ulong GuildID, string parameterName, int newValue)
        {
            string json = File.ReadAllText(GetPath(GuildID));
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(GetPath(GuildID), output);
        }
        public static void UpdateJson(ulong GuildID, string parameterName, bool newValue)
        {
            string json = File.ReadAllText(GetPath(GuildID));
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(GetPath(GuildID), output);
        }
        public static void UpdateJson(ulong GuildID, string parameterName, ulong newValue)
        {
            string json = File.ReadAllText(GetPath(GuildID));
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(GetPath(GuildID), output);
        }
    }
}
