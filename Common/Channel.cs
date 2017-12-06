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

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Other;
using DiscordBot.Logging;

using MelissasCode;

namespace DiscordBot.Common
{
    public class Channel
    {
        [JsonIgnore]
        private static string DirectoryPath { get; } = "channels/";
        [JsonIgnore]
        private static string Extension { get; } = ".json";

        public bool AwardingCoins { get; set; } = true;

        public static void EnsureExists(ulong cId)
        {
            string fileName = DirectoryPath + cId + Extension;
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var channel = new Channel();
                channel.SaveJson(cId);

                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("ok");
                Console.ResetColor();
                Console.WriteLine("]    " + fileName + ": created.");
            }

            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]    " + fileName + ": loaded.");
        }

        private void SaveJson(ulong cId)
        {
            string fileName = DirectoryPath + cId + Extension;
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            File.WriteAllText(file, ToJson());
        }

        public static Channel Load(ulong cId)
        {
            string fileName = DirectoryPath + cId + Extension;
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            return JsonConvert.DeserializeObject<Channel>(File.ReadAllText(file));
        }

        private string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static void SetAwardingCoins(ulong cId, bool value)
        {
            string fileName = DirectoryPath + cId + Extension;
            string json = File.ReadAllText(fileName);

            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj["AwardingCoins"] = value;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fileName, output);
        }
    }
}
