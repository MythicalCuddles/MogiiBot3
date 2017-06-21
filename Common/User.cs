using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MelissasCode;
using Discord;

using DiscordBot.Modules.Public;

namespace DiscordBot.Common
{
    public class User
    {
        [JsonIgnore]
        private static string DirectoryPath { get; set; } = "users/";
        private static string Extension { get; set; } = ".json";

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
        
        public string GitHub { get; set; } = null;
        // Socials
        public string MinecraftUsername { get; set; } = null;
        public string XboxGamertag { get; set; } = null;
        public string PSN { get; set; } = null;
        public string NintendoID { get; set; } = null;
        public string SteamID { get; set; } = null;
        public string Snapchat { get; set; } = null;

        public bool IsBotIgnoringUser { get; set; } = false;
        
        public static bool CreateUserFile(ulong uID)
        {
            string FileName = DirectoryPath + uID + Extension;
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var user = new User();
                user.SaveJson(uID);

                Console.WriteLine(FileName + " Created.");
                return true;
            }
            else
            {
                return false;
            }
        }

        private void SaveJson(ulong uID)
        {
            string FileName = DirectoryPath + uID + Extension;

            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            File.WriteAllText(file, ToJson());
        }

        public static User Load(ulong uID)
        {
            string FileName = DirectoryPath + uID + Extension;

            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            return JsonConvert.DeserializeObject<User>(File.ReadAllText(file));
        }

        private string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static void UpdateJson(ulong uID, string parameterName, string newValue)
        {
            string FileName = DirectoryPath + uID + Extension;

            string json = File.ReadAllText(FileName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(FileName, output);
        }
        public static void UpdateJson(ulong uID, string parameterName, int newValue)
        {
            string FileName = DirectoryPath + uID + Extension;

            string json = File.ReadAllText(FileName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(FileName, output);
        }
        public static void UpdateJson(ulong uID, string parameterName, bool newValue)
        {
            string FileName = DirectoryPath + uID + Extension;

            string json = File.ReadAllText(FileName);
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            jsonObj[parameterName] = newValue;
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(FileName, output);
        }
    }
}
