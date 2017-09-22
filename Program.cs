using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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

namespace DiscordBot
{
    public class Program
    {
        private static readonly Version ProgramVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static void Main(string[] args)
        {
            StartBot();
        }

        public static void StartBot()
        {
            Console.Write("MogiiBot 3: [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Version " + ProgramVersion.Major + "." + ProgramVersion.Minor + "." + ProgramVersion.Build + "." + ProgramVersion.Revision);
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

            try
            {
                new MogiiBot3().RunBotAsync().GetAwaiter().GetResult();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception.ToString());
                Console.WriteLine("\n\n\n\n");

                Task.Delay(1000);

                StartBot();
            }

        }
    }
}
