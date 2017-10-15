using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.Net.Providers.UDPClient;
using Discord.Net.Providers.WS4Net;
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

        private readonly Random _random = new Random();

        public async Task RunBotAsync()
        {
            Bot = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                MessageCacheSize = 50,
                WebSocketProvider = WS4NetProvider.Instance,
                UdpSocketProvider = UDPClientProvider.Instance
            });
            CommandService = new CommandService();

            // Create Tasks for Bot Events
            Bot.Log += Log;
            Bot.UserJoined += UserHandler.UserJoined;
            Bot.UserLeft += UserHandler.UserLeft;
            Bot.ChannelCreated += ChannelHandler.ChannelCreated;
            Bot.ChannelDestroyed += ChannelHandler.ChannelDestroyed;
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
            DateTime nDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            Console.WriteLine(nDateTime);

            List<Tuple<SocketGuildUser, SocketGuild>> offlineList = new List<Tuple<SocketGuildUser, SocketGuild>>();

		    if (Configuration.Load().TwitchLink == null) { await Bot.SetGameAsync(Configuration.Load().Playing); }
            else { await Bot.SetGameAsync(Configuration.Load().Playing, Configuration.Load().TwitchLink, StreamType.Twitch); }
			await Bot.SetStatusAsync(Configuration.Load().Status);

			ModeratorModule.ActiveForDateTime = DateTime.Now;

			Console.WriteLine("-----------------------------------------------------------------");
			foreach (SocketGuild g in Bot.Guilds)
			{
				Console.Write("status: [");
				Console.ForegroundColor = ConsoleColor.DarkYellow;
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
		}

		private static Task Disconnected(Exception exception)
        {
			Console.WriteLine(exception.ToString());
            Console.WriteLine("\n\n\n\n");

            Task.Delay(5000);

            Process process = new Process
            {
                StartInfo =
                {
                    FileName = Path.Combine(AppContext.BaseDirectory, "DiscordBot.exe"),
                    CreateNoWindow = false
                }
            };
            process.Start();

            Environment.Exit(0);
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
                    .WithFooter("Total Respects: " + respects + " (since V2.6)")
                    .WithColor(User.Load(message.Author.Id).AboutR, User.Load(message.Author.Id).AboutG, User.Load(message.Author.Id).AboutB);

                //await message.Channel.SendMessageAsync("Respects have been paid.");
                await message.Channel.SendMessageAsync("", false, eb.Build());
                return;
            }

            // If the message does not contain the prefix or mentioning the bot
            int argPos = 0;
            if (!(message.HasStringPrefix(GuildConfiguration.Load(message.Channel.GetGuild().Id).Prefix, ref argPos) || message.HasMentionPrefix(Bot.CurrentUser, ref argPos) || message.HasStringPrefix(User.Load(message.Author.Id).CustomPrefix, ref argPos))) // Configuration.Load().Prefix
            {
                AwardCoinsToPlayer(message.Author);
                return;
            }

            var context = new CommandContext(Bot, message);
            var result = await CommandService.ExecuteAsync(context, argPos);
            
            if (!result.IsSuccess && Configuration.Load().UnknownCommandEnabled)
            {
                var errorMessage = await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", " + result.ErrorReason);

                Console.WriteLine("[ERROR] " + messageParam.Author.Mention + " - " + result.ErrorReason);
                
                errorMessage.DeleteAfter(20);
            }
        }

        public static void AwardCoinsToPlayer(IUser user, int coinsToAward = 1)
        {
            try
            {
                User.UpdateJson(user.Id, "Coins", (user.GetCoins() + coinsToAward));
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
    }
}
