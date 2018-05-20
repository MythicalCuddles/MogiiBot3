﻿using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DiscordBot.Common;
using DiscordBot.Other;
using DiscordBot.Logging;

using MelissaNet;

namespace DiscordBot
{
    public class Program
    {
        private static readonly Version ProgramVersion = Assembly.GetExecutingAssembly().GetName().Version;

        public static void Main(string[] args)
        {
            if (Configuration.Load().SecretKey == null)
            {
                Application.Run(new frmAuth());
            }

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

            Console.Write("Developed by Melissa Brennan (");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("@MythicalCuddles");
            Console.ResetColor();
            Console.WriteLine(")");

            Console.Write("MelissaNet: [");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Version " + VersionInfo.Version);
            Console.ResetColor();
            Console.WriteLine("]    ");

            Console.WriteLine("Web: www.mythicalcuddles.xyz");
            Console.WriteLine("Project: mogiibot.mythicalcuddles.xyz");
            Console.WriteLine("Contact: melissa@mythicalcuddles.xyz" + "\n");
            Console.WriteLine("Copyright 2017 - 2018 Melissa Brennan | Licensed under the MIT License.");
            Console.WriteLine("-----------------------------------------------------------------");

            Configuration.EnsureExists();

            QuoteHandler.EnsureExists();
            VoteLinkHandler.EnsureExists();
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
