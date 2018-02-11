﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.Net.Providers.UDPClient;
using Discord.Net.Providers.WS4Net;
using Discord.Rest;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Handlers;
using DiscordBot.Logging;
using DiscordBot.Modules.Mod;

using MelissaNet;
using MelissasCode;

namespace DiscordBot
{
	public class MogiiBot3
    {
        protected static readonly string BotToken = Configuration.Load().BotToken ?? DiscordToken.MogiiBot;

        public static DiscordSocketClient Bot;
        public static CommandService CommandService;

        //private readonly Random _random = new Random();

        public async Task RunBotAsync()
        {
            Bot = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                MessageCacheSize = 50,
                WebSocketProvider = WS4NetProvider.Instance,
                UdpSocketProvider = UDPClientProvider.Instance,
                DefaultRetryMode = RetryMode.AlwaysRetry,
                AlwaysDownloadUsers = true,
                ConnectionTimeout = int.MaxValue,

            });
            CommandService = new CommandService();

            // Create Tasks for Bot Events
            Bot.Log += Log;
            Bot.UserJoined += UserHandler.UserJoined;
            Bot.UserLeft += UserHandler.UserLeft;
            Bot.ChannelCreated += ChannelHandler.ChannelCreated;
            Bot.ChannelDestroyed += ChannelHandler.ChannelDestroyed;
            Bot.JoinedGuild += BotOnJoinedGuild;
			Bot.ReactionAdded += ReactionHandler.ReactionAdded;
			Bot.MessageReceived += MessageReceived;
			Bot.MessageDeleted += MessageHandler.MessageDeleted;
            Bot.MessageUpdated += MessageHandler.MessageUpdated;
            Bot.Ready += Ready;
            Bot.Disconnected += Disconnected;

            await CommandService.AddModulesAsync(Assembly.GetEntryAssembly());
			
			// Add token from config
			await Bot.LoginAsync(TokenType.Bot, BotToken); // Connect to Discord with Bot Login Details
			await Bot.StartAsync();

            // Keep the program running.
            await Task.Delay(-1);
		}

        private static Task Log(LogMessage logMessage)
		{
			var cc = Console.ForegroundColor;
			switch (logMessage.Severity)
			{
				case LogSeverity.Critical:
				case LogSeverity.Error:
					Console.ForegroundColor = ConsoleColor.Red;
					break;

                case LogSeverity.Warning:
					Console.ForegroundColor = ConsoleColor.Yellow;
					break;

                case LogSeverity.Info:
					Console.ForegroundColor = ConsoleColor.White;
					break;

                case LogSeverity.Verbose:
				case LogSeverity.Debug:
					Console.ForegroundColor = ConsoleColor.DarkGray;
					break;

                default:
			        Console.ForegroundColor = ConsoleColor.Blue;
			        break;
			}
			Console.WriteLine($"{DateTime.Now,-19} [{logMessage.Severity,8}] {logMessage.Source}: {logMessage.ToString()}");
			Console.ForegroundColor = cc;
			return Task.CompletedTask;
		}
		
		private static async Task Ready()
        {
            List<Tuple<SocketGuildUser, SocketGuild>> offlineList = new List<Tuple<SocketGuildUser, SocketGuild>>();

		    if (Configuration.Load().TwitchLink == null) { await Bot.SetGameAsync(Configuration.Load().Playing); }
            else { await Bot.SetGameAsync(Configuration.Load().Playing, Configuration.Load().TwitchLink, ActivityType.Streaming); }
			await Bot.SetStatusAsync(Configuration.Load().Status);

			ModeratorModule.ActiveForDateTime = DateTime.Now;

			Console.WriteLine("-----------------------------------------------------------------");
			foreach (SocketGuild g in Bot.Guilds)
			{
			    Console.ResetColor();
				Console.Write("status: [");
				Console.ForegroundColor = ConsoleColor.Yellow;
				Console.Write("find");
				Console.ResetColor();
				Console.WriteLine("]  " + g.Name + ": attempting to load.");

				GuildConfiguration.EnsureExists(g.Id);

				Console.WriteLine("-----------------------------------------------------------------");

				foreach (SocketGuildUser u in g.Users)
				{
					if (User.CreateUserFile(u.Id))
					{
                        offlineList.Add(new Tuple<SocketGuildUser, SocketGuild>(u, g));
					}
				}

			    Console.WriteLine("-----------------------------------------------------------------");

			    foreach (SocketChannel c in g.Channels)
			    {
			        Channel.EnsureExists(c.Id);
			    }

			    Console.WriteLine("-----------------------------------------------------------------");
            }

            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("info");
            Console.ResetColor();
            Console.Write("]  " + Bot.CurrentUser.Username + " : ");
            if (offlineList.Any())
                Console.WriteLine(offlineList.Count + " new users added.");
            else
                Console.WriteLine("no new users added.");

            Console.Write("status: [");
		    Console.ForegroundColor = ConsoleColor.DarkGreen;
		    Console.Write("ok");
		    Console.ResetColor();
		    Console.WriteLine("]    " + Bot.CurrentUser.Id + ": " + Bot.CurrentUser.Username + " loaded.");

			// Send message to log channel to announce bot is up and running.
			Version v = Assembly.GetExecutingAssembly().GetName().Version;
			EmbedBuilder eb = new EmbedBuilder()
					.WithTitle("Startup Log")
					.WithColor(59, 212, 50)
					.WithThumbnailUrl(Bot.CurrentUser.GetAvatarUrl())
					.WithDescription("**" + Bot.CurrentUser.Username + "** : status [ok] : If there was any new users, they will be shown below.")
                    .AddField("Version", v.Major + "." + v.Minor + "." + v.Build + "." + v.Revision, true)
                    .AddField("Latest Version", MythicalCuddlesXYZ.CheckForNewVersion("MogiiBot3").Item1, true)
					.AddField("MelissasCode", MelissaCode.Version, true)
                    .AddField("MelissaNet", VersionInfo.Version, true)
					.AddField("Database Status", MelissaCode.GetDatabaseStatus(), true)
					.AddField("Latency", Bot.Latency + "ms", true)
                    .WithCurrentTimestamp();
				await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
            
            foreach (Tuple<SocketGuildUser, SocketGuild> tupleList in offlineList)
		    {
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("[ALERT] While " + Bot.CurrentUser.Username + " was offline, " + tupleList.Item1.Mention + " (" + tupleList.Item1.Id + ") joined " + tupleList.Item2.Name + ". They have been added to the database.");
            }

            //await GetPlayerCount();
        }

        //TODO: Finsih this.
        //private static string onlineCountUrl = "https://minecraft-api.com/api/ping/playeronline.php?ip=mogiicraft.ddns.net&port=25635";
        //private static string onlineListUrl = "https://minecraft-api.com/api/query/playerlist.php?ip=mogiicraft.ddns.net&port=28069";
        //private static WebClient client = new WebClient();

        //private static async Task GetPlayerCount()
        //{
        //    while (true)
        //    {
        //        if (Configuration.Load().PlayerCountPlayingMessageEnabled)
        //        {
        //            using (var stream = client.OpenRead(onlineCountUrl))
        //            {
        //                using (var reader = new StreamReader(stream))
        //                {
        //                    string line;
        //                    string message = Configuration.Load().PlayerCountPlayingMessage;
        //                    while ((line = reader.ReadLine()) != null)
        //                    {
        //                        message = Regex.Replace(message, "{SERVER.PLAYERCOUNT}", line, RegexOptions.IgnoreCase);
        //                        Bot.SetGameAsync(message);
        //                    }
        //                }
        //            }
        //        }

        //        using (var stream = client.OpenRead(onlineListUrl))
        //        {
        //            using (var reader = new StreamReader(stream))
        //            {
        //                string line;
        //                while ((line = reader.ReadLine()) != null)
        //                {
        //                    Console.WriteLine(line);
        //                }
        //            }
        //        }

        //        Console.WriteLine("Cooldown started.");
        //        Thread.Sleep(300000);
        //    }
        //}

        private async Task BotOnJoinedGuild(SocketGuild socketGuild)
        {
            foreach (SocketChannel c in socketGuild.Channels)
                Channel.EnsureExists(c.Id);

            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(socketGuild.Name + " has been added to MogiiBot's guild list. \n" + socketGuild.Owner.Username + " is the owner (" + socketGuild.Owner.Id + ")");
        }

        private static Task Disconnected(Exception exception)
        {
			Console.WriteLine(exception + "\n");

            // REMOVED AS DefaultRetryMode ADDED TO DISCORDSOCKETCONFIG
            //Console.WriteLine("\n\n\n\n");

            //Task.Delay(5000);

            //Process process = new Process
            //{
            //    StartInfo =
            //    {
            //        FileName = Path.Combine(AppContext.BaseDirectory, "DiscordBot.exe"),
            //        CreateNoWindow = false
            //    }
            //};
            //process.Start();

            //Environment.Exit(0);
            return Task.CompletedTask;
        }
	    
        private async Task MessageReceived(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            // Adds the message to the log file
            MessageLogger.LogNewMessage(message);

            if (message == null) return;
            if (message.Author.IsBot) return;

            // Is the bot told to ignore the user? If so, is the user NOT Melissa? Iff => escape Task
            if (User.Load(message.Author.Id).IsBotIgnoringUser && message.Author.Id != DiscordWorker.getMelissaID) return;
            
            // If the message came from somewhere that is not a text channel -> Private Message
            if (!(messageParam.Channel is ITextChannel))
            {
                Color color = new Color(255, 116, 140);
                EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                    .WithName("Private Message");
                EmbedFooterBuilder efb = new EmbedFooterBuilder()
                    .WithText("PRIVATE MESSAGE | UID: " + message.Author.Id + " | MID: " + message.Id + " | STATUS: [ok]");
                EmbedBuilder eb = new EmbedBuilder()
                    .WithAuthor(eab)
                    .WithTitle("from: @" + message.Author.Username)
                    .WithDescription(message.Content + "\n\n---------------------------------------------")
                    .WithColor(color)
                    .WithFooter(efb)
                    .WithCurrentTimestamp();

                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
            }

            // If the message is just "F", pay respects.
            if (message.Content.ToUpper() == "F")
            {
                var respects = Configuration.Load().Respects + 1;
                Configuration.UpdateJson("Respects", respects);

                var eb = new EmbedBuilder()
                    .WithDescription("**" + message.Author.Username + "** has paid their respects.")
                    .WithFooter("Total Respects: " + respects)
                    .WithColor(User.Load(message.Author.Id).AboutR, User.Load(message.Author.Id).AboutG, User.Load(message.Author.Id).AboutB);

                //await message.Channel.SendMessageAsync("Respects have been paid.");
                await message.Channel.SendMessageAsync("", false, eb.Build());
                return;
            }
            
            int argPos = 0;
            if (message.HasStringPrefix(GuildConfiguration.Load(message.Channel.GetGuild().Id).Prefix, ref argPos) ||
                message.HasMentionPrefix(Bot.CurrentUser, ref argPos) ||
                message.HasStringPrefix(User.Load(message.Author.Id).CustomPrefix, ref argPos))
            {
                var context = new CommandContext(Bot, message);
                var result = await CommandService.ExecuteAsync(context, argPos);

                if (!result.IsSuccess && Configuration.Load().UnknownCommandEnabled)
                {
                    var errorMessage = await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", " + result.ErrorReason);

                    Console.Write("status: [");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("warning");
                    Console.ResetColor();
                    Console.WriteLine("]  " + message.Author.Username + " : " + result.ErrorReason);

                    errorMessage.DeleteAfter(20);
                }
            }
            else
            {
                if (message.Content.Length >= Configuration.Load().MinLengthForCoin)
                {
                    if (Channel.Load(message.Channel.Id).AwardingCoins)
                    {
                        AwardCoinsToPlayer(message.Author);

                        Console.Write("status: [");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("info");
                        Console.ResetColor();
                        Console.WriteLine("]  " + message.Author.Username + " : sent a message and was awarded 1 coin(s).");
                    }
                    else
                    {
                        Console.Write("status: [");
                        Console.ForegroundColor = ConsoleColor.DarkCyan;
                        Console.Write("info");
                        Console.ResetColor();
                        Console.WriteLine("]  " + message.Author.Username + " : sent a message and was awarded 0 coin(s) due to the channel being set not to give coins.");
                    }
                }
                else
                {
                    Console.Write("status: [");
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.Write("info");
                    Console.ResetColor();
                    Console.WriteLine("]  " + message.Author.Username + " : sent a message and was awarded 0 coin(s) due to length (" + message.Content.Length + ").");
                }
            }
        }

        public static void AwardCoinsToPlayer(IUser user, int coinsToAward = 1)
        {
            try
            {
                //User.UpdateUser(user.Id, (user.GetCoins() + coinsToAward));
                User.SetCoins(user.Id, (user.GetCoins() + coinsToAward));

                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkCyan;
                Console.Write("info");
                Console.ResetColor();
                Console.WriteLine("]  " + user.Username + " : coin method called.");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
