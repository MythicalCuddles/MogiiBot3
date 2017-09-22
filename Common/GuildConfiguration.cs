using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Handlers;
using DiscordBot.Logging;
using DiscordBot.Other;

using MelissasCode;

namespace DiscordBot.Common
{
    public class GuildConfiguration
    {
        public string Prefix { get; set; } = "$";
        public string WelcomeMessage { get; set; } = "";
        public ulong WelcomeChannelId { get; set; } = 0;
        public ulong LogChannelId { get; set; } = 0;
		public ulong BotChannelId { get; set; } = 0;

        public bool SenpaiEnabled { get; set; } = true;
        public bool QuotesEnabled { get; set; } = true;

        public bool EnableNsfwCommands { get; set; } = false;
        public ulong RuleGambleChannelId { get; set; } = 0;

        public static void EnsureExists(ulong guildId)
        {
            string file = Path.Combine(AppContext.BaseDirectory, GetPath(guildId));
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var guildConfig = new GuildConfiguration();
                guildConfig.SaveJson(guildId);

                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("ok");
                Console.ResetColor();
                Console.WriteLine("]    " + GetPath(guildId) + ": created.");
            }

            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]    " + GetPath(guildId) + ": loaded.");
        }

        private static string GetPath(ulong guildId)
        {
            return ("config/guilds/" + guildId + ".json");
        }

        public void SaveJson(ulong guildId)
        {
            string file = Path.Combine(AppContext.BaseDirectory, GetPath(guildId));
            File.WriteAllText(file, ToJson());
        }

        public static GuildConfiguration Load(ulong guildId)
        {
            string file = Path.Combine(AppContext.BaseDirectory, GetPath(guildId));
            return JsonConvert.DeserializeObject<GuildConfiguration>(File.ReadAllText(file));
        }

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static void UpdateJson(ulong guildId, string parameterName, string newValue)
        {
            string json = File.ReadAllText(GetPath(guildId));
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(GetPath(guildId), output);
        }
        public static void UpdateJson(ulong guildId, string parameterName, int newValue)
        {
            string json = File.ReadAllText(GetPath(guildId));
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(GetPath(guildId), output);
        }
        public static void UpdateJson(ulong guildId, string parameterName, bool newValue)
        {
            string json = File.ReadAllText(GetPath(guildId));
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(GetPath(guildId), output);
        }
        public static void UpdateJson(ulong guildId, string parameterName, ulong newValue)
        {
            string json = File.ReadAllText(GetPath(guildId));
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(GetPath(guildId), output);
        }
    }
}
