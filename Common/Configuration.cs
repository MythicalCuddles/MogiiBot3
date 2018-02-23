﻿using System;
using System.IO;

using Newtonsoft.Json;

using Discord;
using MelissaNet;

namespace DiscordBot.Common
{
    public class Configuration
    {
        [JsonIgnore]
        public static string FileName { get; } = "config/configuration.json";

        public string BotToken { get; set; }
        
        public ulong Developer { get; set; } = 149991092337639424;
        public string StatusText { get; set; } = null;
        public string StatusLink { get; set; } = null;
        public int StatusActivity { get; set; } = -1;
        public UserStatus Status { get; set; } = UserStatus.Online;
        
        public bool UnknownCommandEnabled { get; set; } = true;
        public int LeaderboardAmount { get; set; } = 5;
        public int QuoteCost { get; set; } = 250;
        public int PrefixCost { get; set; } = 2500;
        public int SenpaiChanceRate { get; set; } = 5;

        public ulong LogChannelId { get; set; } = 349976716237602816;

        public int Respects { get; set; } = 0;
        public int MinLengthForCoin { get; set; } = 0;

        /// NSFW Variables
        public int MaxRuleXGamble { get; set; } = 2353312;

        public static void EnsureExists()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                var config = new Configuration();

                //TODO: Clean Up
                Console.WriteLine("No configuration file was found. Lets set one up now!");

                Console.Write("Please enter the Bot Token: ");
                config.BotToken = MelissaNet.Cryptography.EncryptString(Console.ReadLine());
                Console.WriteLine("Token saved to " + FileName + "!");

                Console.WriteLine("Console will now be cleared for security reasons. Press the 'enter' key to continue.");
                Console.ReadLine();

                Console.Clear();

                config.SaveJson();
                
                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("ok");
                Console.ResetColor();
                Console.WriteLine("]    " + FileName + ": created.");
            }

            //TODO: Clean Up
            if (Load().BotToken.IsNullOrEmpty() || Load().BotToken.IsNullOrWhiteSpace())
            {
                var config = new Configuration();

                Console.WriteLine("Warning: The Bot Token was not found.");

                Console.Write("Please enter the Bot Token: ");
                config.BotToken = MelissaNet.Cryptography.EncryptString(Console.ReadLine());
                Console.WriteLine("Token saved to " + FileName + "!");

                Console.WriteLine("Console will now be cleared for security reasons. Press the 'enter' key to continue.");
                Console.ReadLine();

                Console.Clear();

                config.SaveJson();
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

        //public static void UpdateJson(string parameterName, string newValue)
        //{
        //    string json = File.ReadAllText(FileName);
        //    dynamic jsonObj = JsonConvert.DeserializeObject(json);
        //    jsonObj[parameterName] = newValue;
        //    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        //    File.WriteAllText(FileName, output);
        //}
        //public static void UpdateJson(string parameterName, int newValue)
        //{
        //    string json = File.ReadAllText(FileName);
        //    dynamic jsonObj = JsonConvert.DeserializeObject(json);
        //    jsonObj[parameterName] = newValue;
        //    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        //    File.WriteAllText(FileName, output);
        //}
        //public static void UpdateJson(string parameterName, bool newValue)
        //{
        //    string json = File.ReadAllText(FileName);
        //    dynamic jsonObj = JsonConvert.DeserializeObject(json);
        //    jsonObj[parameterName] = newValue;
        //    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        //    File.WriteAllText(FileName, output);
        //}
        //public static void UpdateJson(string parameterName, ulong newValue)
        //{
        //    string json = File.ReadAllText(FileName);
        //    dynamic jsonObj = JsonConvert.DeserializeObject(json);
        //    jsonObj[parameterName] = newValue;
        //    string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        //    File.WriteAllText(FileName, output);
        //}

        public static void UpdateConfiguration(string botToken = null, ulong? developer = null, string statusText = null, string statusLink = null,
            int? statusActivity = null, UserStatus? status = null, bool? unknownCommandEnabled = null,
            int? leaderboardAmount = null, int? quoteCost = null, int? prefixCost = null, int? senpaiChanceRate = null,
            ulong? logChannelId = null, int? respects = null, int? minLengthForCoin = null, int? maxRuleXGamble = null)
        {
            var config = new Configuration()
            {
                BotToken = botToken ?? Load().BotToken,
                Developer = developer ?? Load().Developer,

                StatusText = statusText ?? Load().StatusText,
                StatusLink = statusLink ?? Load().StatusLink,
                StatusActivity = statusActivity ?? Load().StatusActivity,
                Status = status ?? Load().Status,

                UnknownCommandEnabled = unknownCommandEnabled ?? Load().UnknownCommandEnabled,
                LeaderboardAmount = leaderboardAmount ?? Load().LeaderboardAmount,
                QuoteCost = quoteCost ?? Load().QuoteCost,
                PrefixCost = prefixCost ?? Load().PrefixCost,
                SenpaiChanceRate = senpaiChanceRate ?? Load().SenpaiChanceRate,

                LogChannelId = logChannelId ?? Load().LogChannelId,

                Respects = respects ?? Load().Respects,
                MinLengthForCoin = minLengthForCoin ?? Load().MinLengthForCoin,

                MaxRuleXGamble = maxRuleXGamble ?? Load().MaxRuleXGamble
            };
            config.SaveJson();
        }
    }
}
