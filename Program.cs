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
using DiscordBot.Logging;

using MelissasCode;
using Discord.Commands;
using System.Reflection;

namespace DiscordBot
{
    class Program
    {
        private static Version _v = Assembly.GetExecutingAssembly().GetName().Version;

        static void Main(string[] args)
        {
            Console.Write("MogiiBot 3: [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Version " + _v.Major + "." + _v.Minor + "." + _v.Build + "." + _v.Revision);
            Console.ResetColor();
            Console.WriteLine("]    ");

            Console.WriteLine("A Discord.Net Bot");
            Console.WriteLine("-----------------------------------------------------------------");
            
            Console.Write("Developed by Melissa B. (");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("@MythicalCuddles");
            Console.ResetColor();
            Console.WriteLine(")");

            Console.Write("MelissasCode: [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Version " + MelissaCode.Version);
            Console.ResetColor();
            Console.WriteLine("]    ");

            Console.WriteLine("Web: www.mythicalcuddles.xyz");
            Console.WriteLine("Contact: melissa@mythicalcuddles.xyz" + "\n");
            Console.WriteLine("Copyright 2017 Melissa B. | Licensed under the MIT License.");
            Console.WriteLine("-----------------------------------------------------------------");

            License.PrintMIT();
            Console.WriteLine("-----------------------------------------------------------------");

            Configuration.EnsureExists();

            QuoteHandler.EnsureExists();
            VoteLinkHandler.EnsureExists();
            MusicHandler.EnsureExists();
            ImageHandler.EnsureExists();
            TransactionLogger.EnsureExists();
            Console.WriteLine("-----------------------------------------------------------------");

            new MogiiBot3().RunBotAsync().GetAwaiter().GetResult();
        }
    }
}
