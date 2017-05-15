using Discord;
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
    public class MogiiBot3
    {
        public static DiscordSocketClient _bot;
        public static CommandService commandService;
        public static DependencyMap dependencyMap;

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
            dependencyMap = new DependencyMap();

            // Create Tasks for Bot Events
            _bot.Log += Log;

            _bot.UserJoined += UserJoined;
            _bot.UserLeft += UserLeft;

            _bot.Ready += Ready;

            _bot.ReactionAdded += ReactionAdded;

            await InstallCommands();
            _bot.MessageDeleted += MessageDeleted;
            _bot.MessageUpdated += MessageUpdated;

            // Connect to Discord with Bot Login Details
            await _bot.LoginAsync(TokenType.Bot, DiscordToken.MogiiBot);
            await _bot.StartAsync();

            // Keep the program running.
            await Task.Delay(-1);
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> cachedMessage, SocketMessage message, ISocketMessageChannel channel)
        {
            var msg = message as SocketUserMessage;
            Logging.MessageLogger.logEditMessage(msg);
        }

        private async Task MessageDeleted(Cacheable<IMessage, ulong> cachedMessage, ISocketMessageChannel channel)
        {
            var message = cachedMessage.Value as SocketUserMessage;
            Logging.MessageLogger.logDeleteMessage(message);
        }

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
                return;

            if (QuoteHandler.quoteMessages.Contains(message.Id))
            {
                // Check to see if the next page or previous page was clicked.
                if (reaction.Emoji.Name == Extensions.Extensions.arrow_left)
                {
                    if (QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] == 1)
                        return;

                    QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)]--;
                }
                else if (reaction.Emoji.Name == Extensions.Extensions.arrow_right)
                {
                    if (QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] == QuoteHandler.getQuotesListLength)
                        return;

                    QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)]++;
                }

                StringBuilder sb = new StringBuilder()
                .Append("**Quote List** : *Page " + QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] + "*\n```");

                List<string> quotes = QuoteHandler.getQuotes(QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)]);

                for (int i = 0; i < quotes.Count; i++)
                {
                    sb.Append((i + ((QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + quotes[i] + "\n");
                }

                sb.Append("```");

                await message.Value.ModifyAsync(msg => msg.Content = sb.ToString());
                await message.Value.RemoveAllReactionsAsync();

                if (QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] == 1)
                {
                    await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
                }
                else if (QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] == QuoteHandler.getQuotesListLength)
                {
                    await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
                }
                else
                {
                    await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
                    await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
                }
            }
        }

        private async Task Ready()
        {
            await _bot.SetGameAsync(Configuration.Load().Playing);
            await _bot.SetStatusAsync(Configuration.Load().Status);

            Modules.Public.InfoModule._dt = DateTime.Now;
        }

        private async Task UserLeft(SocketGuildUser e)
        {
            if (e.Guild.Id == Configuration.Load().ServerID)
            {
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("User Left")
                    .WithDescription("@" + e.Username + "\n" + e.Nickname + "\n" + e.Id)
                    .WithColor(new Color(255, 28, 28))
                    .WithThumbnailUrl(e.GetAvatarUrl())
                    .WithCurrentTimestamp();

                await GetHandler.getTextChannel(Configuration.Load().MCLogChannelID).SendMessageAsync("", false, eb);
            }
            else if (e.Guild.Id == Configuration.Load().NSFWServerID)
            {
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("NSFW Server - User Left")
                    .WithDescription("@" + e.Username + "\n" + e.Nickname + "\n" + e.Id)
                    .WithColor(new Color(255, 28, 28))
                    .WithThumbnailUrl(e.GetAvatarUrl())
                    .WithCurrentTimestamp();

                await GetHandler.getTextChannel(Configuration.Load().MCLogChannelID).SendMessageAsync("", false, eb);
            }
        }

        private async Task UserJoined(SocketGuildUser e)
        {
            if (e.Guild.Id == Configuration.Load().ServerID)
            {
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("User Joined")
                    .WithDescription("@" + e.Username + "\n" + e.Id)
                    .WithColor(new Color(28, 255, 28))
                    .WithThumbnailUrl(e.GetAvatarUrl())
                    .WithCurrentTimestamp();

                await GetHandler.getTextChannel(Configuration.Load().MCLogChannelID).SendMessageAsync("", false, eb);

                string wMsg1 = Configuration.Load().welcomeMessage.Replace("{USERJOINED}", e.Mention).Replace("{GUILDNAME}", e.Guild.Name);
                await GetHandler.getTextChannel(Configuration.Load().MCWelcomeChannelID).SendMessageAsync(wMsg1);
            }
            else if (e.Guild.Id == Configuration.Load().NSFWServerID)
            {
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("NSFW Server - User Joined")
                    .WithDescription("@" + e.Username + "\n" + e.Id)
                    .WithColor(new Color(28, 255, 28))
                    .WithThumbnailUrl(e.GetAvatarUrl())
                    .WithCurrentTimestamp();

                await GetHandler.getTextChannel(Configuration.Load().MCLogChannelID).SendMessageAsync("", false, eb);
            }

            if (User.CreateUserFile(e.Id))
            {
                await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync(e.Username + " was successfully added to the database. [" + e.Id + "]");
            }
        }

        public async Task InstallCommands()
        {
            _bot.MessageReceived += MessageReceived;

            await commandService.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task MessageReceived(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;

            if (message == null) return;
            if (message.Author.IsBot)
            {
                try
                {
                    User.UpdateJson(message.Author.Id, "Coins", (User.Load(message.Author.Id).Coins + 1));
                }
                catch (Exception e) { Console.WriteLine(e); }

                return;
            }

            // Adds the message to the log file
            Logging.MessageLogger.logNewMessage(message);

            // Is the bot told to ignore the user? If so, is the user NOT Melissa? Iff => escape Task
            if (User.Load(message.Author.Id).IsBotIgnoringUser && message.Author.Id != DiscordWorker.getMelissaID) return;
            
            // Only respond on the NSFW Server if the channel id matches the rule34gamble channel id
            if (message.IsMessageOnNSFWChannel() && message.Channel.Id != Configuration.Load().RuleGambleChannelID && message.Author.Id != Configuration.Load().Developer) return;

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

                await GetHandler.getTextChannel(Configuration.Load().MCLogChannelID).SendMessageAsync("", false, eb);
            }

            // If the message is just "F", pay respects.
            if (message.Content.ToUpper() == "F")
            {
                await message.Channel.SendMessageAsync("Respects have been paid.");
                return;
            }

            // If the message does not contain the prefix or mentioning the bot
            int argPos = 0;
            if (!(message.HasStringPrefix(Configuration.Load().Prefix, ref argPos) || message.HasMentionPrefix(_bot.CurrentUser, ref argPos)))
            {
                // Coin System to add a coin for each message the user sends.
                try
                {
                    User.UpdateJson(message.Author.Id, "Coins", (User.Load(message.Author.Id).Coins + 1));
                }
                catch (Exception e) { Console.WriteLine(e); }

                return;
            }

            var context = new CommandContext(_bot, message);
            // Execute Commands in dir \Modules
            var result = await commandService.ExecuteAsync(context, argPos, dependencyMap);

            // If the command modules doesn't contain a task for the message, return an error iff UnknownCommand is enabled
            if (!result.IsSuccess && Configuration.Load().UnknownCommandEnabled)
            {
                await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", " + result.ErrorReason);
            }
        }

        // Log messages to the console in different colors
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
    }
}
