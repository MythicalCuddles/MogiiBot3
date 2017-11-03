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
    public class User
    {
        [JsonIgnore]
        private static string DirectoryPath { get; } = "users/";
        [JsonIgnore]
        private static string Extension { get; } = ".json";

        public int Coins { get; set; } = 0;
        
        public string Name { get; set; } = null;
        public string Gender { get; set; } = null;
        public string Pronouns { get; set; } = null;
        public string About { get; set; } = null;
        public string CustomPrefix { get; set; }

        public byte AboutR { get; set; } = 140;
        public byte AboutG { get; set; } = 90;
        public byte AboutB { get; set; } = 210;
        
        public bool TeamMember { get; set; } = false;
        public string EmbedAuthorBuilderIconUrl { get; set; } = "http://i.imgur.com/Ny5Qcto.png";
        public string EmbedFooterBuilderIconUrl { get; set; } = "http://i.imgur.com/Ny5Qcto.png";
        public string FooterText { get; set; } = null;

        /// Socials
        public string MinecraftUsername { get; set; } = null;
        public string Snapchat { get; set; } = null;

        public bool IsBotIgnoringUser { get; set; } = false;


		public static bool CreateUserFile(ulong uId)
        {
            string fileName = DirectoryPath + uId + Extension;
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var user = new User();
                user.SaveJson(uId);

                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("alert");
                Console.ResetColor();
                Console.WriteLine("]    " + fileName + ": created.");
                return true;
            }
            else
            {
                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("ok");
                Console.ResetColor();
                Console.WriteLine("]    " + fileName + ": already exists.");
                return false;
            }
        }

        private void SaveJson(ulong uId)
        {
            string fileName = DirectoryPath + uId + Extension;

            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            File.WriteAllText(file, ToJson());
        }

        public static User Load(ulong uId)
        {
            string fileName = DirectoryPath + uId + Extension;

            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            return JsonConvert.DeserializeObject<User>(File.ReadAllText(file));
        }

        private string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static void UpdateJson(ulong uId, string parameterName, string newValue)
        {
            string fileName = DirectoryPath + uId + Extension;

            string json = File.ReadAllText(fileName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fileName, output);
        }
        public static void UpdateJson(ulong uId, string parameterName, int newValue)
        {
            string fileName = DirectoryPath + uId + Extension;

            string json = File.ReadAllText(fileName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fileName, output);
        }
        public static void UpdateJson(ulong uId, string parameterName, bool newValue)
        {
            string fileName = DirectoryPath + uId + Extension;

            string json = File.ReadAllText(fileName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fileName, output);
        }

        internal static bool SetCoinsForAll(int newValue = 0)
        {
            string filePath = Path.Combine(AppContext.BaseDirectory, DirectoryPath);
            DirectoryInfo d = new DirectoryInfo(filePath);

            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine("[WARNING] A command was issued resetting the coins for all users.");

            foreach (var file in d.GetFiles("*.json"))
            {
                string[] fileName = file.ToString().Split('.');

                Console.WriteLine("[Info] " + file + " - " + Load(Convert.ToUInt64(fileName[0])).Coins + " coins has been set to " + newValue + "!");
                UpdateJson(Convert.ToUInt64(fileName[0]), "Coins", newValue);
            }

            Console.WriteLine("-----------------------------------------------------------------");
            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]  " + "Coin Reset" + ": reset completed.");
            Console.WriteLine("-----------------------------------------------------------------");

            return true;
        }
    }
}
