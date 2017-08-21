using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

using System.IO;
using System.Diagnostics;

using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Net.Providers.WS4Net;
using Discord.Net.Providers.UDPClient;

using DiscordBot.Common;
using DiscordBot.Handlers;
using DiscordBot.Other;
using DiscordBot.Extensions;
using DiscordBot.Logging;

using MelissasCode;
//
namespace DiscordBot
{
    public class MogiiBot3
    {
        private static string BotToken = DiscordToken.MogiiBot;

        public static DiscordSocketClient _bot;
        public static CommandService commandService;

        Random _r = new Random();

        public async Task RunBotAsync()
        {
            _bot = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                MessageCacheSize = 50,
                WebSocketProvider = WS4NetProvider.Instance,
                UdpSocketProvider = UDPClientProvider.Instance,
            });
            commandService = new CommandService();

            // Create Tasks for Bot Events
            _bot.Log += Log;
            
            _bot.UserJoined += UserHandler.UserJoined;
            _bot.UserLeft += UserHandler.UserLeft;

            _bot.ChannelCreated += ChannelHandler.ChannelCreated;
            _bot.ChannelDestroyed += ChannelHandler.ChannelDestroyed;

            _bot.Ready += Ready;
			_bot.Disconnected += Disconnected;

			_bot.ReactionAdded += ReactionHandler.ReactionAdded;

            //await InstallCommands();
			_bot.MessageReceived += MessageReceived;
			_bot.MessageDeleted += MessageHandler.MessageDeleted;
            _bot.MessageUpdated += MessageHandler.MessageUpdated;
			
			await commandService.AddModulesAsync(Assembly.GetEntryAssembly());
			
			// Add token from config
			await _bot.LoginAsync(TokenType.Bot, BotToken); // Connect to Discord with Bot Login Details
			await _bot.StartAsync();

            // Keep the program running.
            await Task.Delay(-1);
		}

		private Task Log(LogMessage logMessage)
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
			}
			Console.WriteLine($"{DateTime.Now,-19} [{logMessage.Severity,8}] {logMessage.Source}: {logMessage.ToString()}");
			Console.ForegroundColor = cc;
			return Task.CompletedTask;
		}
		
		private async Task Ready()
		{
			await _bot.SetGameAsync(Configuration.Load().Playing);
			await _bot.SetStatusAsync(Configuration.Load().Status);

			Modules.Mod.ModeratorModule._dt = DateTime.Now;

			Console.WriteLine("-----------------------------------------------------------------");
			foreach (SocketGuild g in _bot.Guilds)
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
						Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync("[ALERT] " + u.Mention + " (" + u.Id + ") was added to the database while the bot was offline...");
					}
				}
				Console.WriteLine("-----------------------------------------------------------------");
			}

			Console.WriteLine(_bot.CurrentUser.Id + " | " + _bot.CurrentUser.Username);
		}

		private async Task Disconnected(Exception exception)
        {
			Console.WriteLine(exception.ToString());
            Console.WriteLine("\n\n\n\n");

            Task.Delay(5000);

            Process process = new Process();
            process.StartInfo.FileName = Path.Combine(AppContext.BaseDirectory, "DiscordBot.exe");
            process.StartInfo.CreateNoWindow = false;
            process.Start();

            Environment.Exit(0);
        }
        
        //private async Task InstallCommands()
        //{
        //    _bot.MessageReceived += MessageReceived;

        //    await commandService.AddModulesAsync(Assembly.GetEntryAssembly());
        //}
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

                await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync("", false, eb);
            }

            // If the message is just "F", pay respects.
            if (message.Content.ToUpper() == "F")
            {
                await message.Channel.SendMessageAsync("Respects have been paid.");
                return;
            }

            // If the message does not contain the prefix or mentioning the bot
            int argPos = 0;
            if (!(message.HasStringPrefix(GuildConfiguration.Load(message.Channel.GetGuild().Id).Prefix, ref argPos) || message.HasMentionPrefix(_bot.CurrentUser, ref argPos) || message.HasStringPrefix(User.Load(message.Author.Id).CustomPrefix, ref argPos))) // Configuration.Load().Prefix
            {
                await AwardCoinsToPlayer(message.Author);
                return;
            }

            var context = new CommandContext(_bot, message);
            var result = await commandService.ExecuteAsync(context, argPos);
            
            if (!result.IsSuccess && Configuration.Load().UnknownCommandEnabled)
            {
                IUserMessage errorMessage;
                if (result.ErrorReason.ToUpper().Contains(MelissaCode.GetOldFullNameUpper))
                {
                    errorMessage = await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", an error containing classified information has occured. Please contact Melissa.\n`Error Code/Log File: #Ex00f" + _r.RandomNumber(0, 1000000) + "`");
                }
                else if(result.ErrorReason.ToUpper().Contains("END ON AN INCOMPLETE ESCAPE") && context.Message.Content.ToUpper().Contains("$SETPREFIX"))
                {
                    errorMessage = await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", you can not use that prefix as it contains an escape character.");
                }
                else
                {
                    errorMessage = await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", " + result.ErrorReason);
                }

                Console.WriteLine(messageParam.Author.Mention + ", " + result.ErrorReason);
                
                errorMessage.DeleteAfter(20);

                return;
            }
        }

        public static Task AwardCoinsToPlayer(IUser Account, int CoinsToAward = 1)
        {
            try
            {
                User.UpdateJson(Account.Id, "Coins", (Account.GetCoins() + CoinsToAward));
            }
            catch (Exception e) { Console.WriteLine(e); }

            return Task.CompletedTask;
        }
    }
}
