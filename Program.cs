using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.WebSocket;
using Discord.Net.Providers.WS4Net;
using Discord.Net.Providers.UDPClient;

using DiscordBot.Common;
using DiscordBot.Other;
using DiscordBot.Extensions;

using MelissasCode;
using Discord.Commands;
using System.Reflection;

namespace DiscordBot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("MogiiBot 3");
            Console.WriteLine("A Discord.Net Bot");
            Console.WriteLine("------------------------------------------");

            Console.WriteLine("Developed by Melissa B. (@MythicalCuddles)");
            Console.WriteLine("www.mythicalcuddles.xyz");
            Console.WriteLine("Copyright 2017 Melissa B.");
            Console.WriteLine("------------------------------------------");

            License.PrintMIT();
            Console.WriteLine("------------------------------------------");

            Configuration.EnsureExists();
            QuoteHandler.EnsureExists();
            VoteLinkHandler.EnsureExists();
            MusicHandler.EnsureExists();
            Console.WriteLine("------------------------------------------");

            new MogiiBot3().RunBotAsync().GetAwaiter().GetResult();
        }
    }
}
