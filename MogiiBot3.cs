using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
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
    public class MogiiBot3
    {
        private static string BotToken = DiscordToken.MogiiBot;

        public static DiscordSocketClient _bot;
        public static CommandService commandService;
        public static DependencyMap dependencyMap;

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
            dependencyMap = new DependencyMap();

            // Create Tasks for Bot Events
            _bot.Log += Log;
            
            _bot.UserJoined += UserJoined;
            _bot.UserLeft += UserLeft;

            _bot.ChannelCreated += ChannelCreated;
            _bot.ChannelDestroyed += ChannelDestroyed;
            //_bot.ChannelUpdated += ChannelUpdated;
            
            _bot.Ready += Ready;

            _bot.ReactionAdded += ReactionAdded;

            await InstallCommands();
            _bot.MessageDeleted += MessageDeleted;
            _bot.MessageUpdated += MessageUpdated;

            // Connect to Discord with Bot Login Details
            await _bot.LoginAsync(TokenType.Bot, BotToken);
            await _bot.StartAsync();

            // Keep the program running.
            await Task.Delay(-1);
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
        
        private async Task Ready()
        {
            await _bot.SetGameAsync(Configuration.Load().Playing);
            await _bot.SetStatusAsync(Configuration.Load().Status);

            Modules.Public.InfoModule._dt = DateTime.Now;

            Console.WriteLine("-----------------------------------------------------------------");
            foreach (SocketGuild g in _bot.Guilds)
            {
                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("find");
                Console.ResetColor();
                Console.WriteLine("]  " + g.Name + ": attempting to load.");

                GuildConfiguration.EnsureExists(g.Id);
            }
            Console.WriteLine("-----------------------------------------------------------------");
        }

        private async Task ChannelCreated(SocketChannel channel)
        {
            if (channel is ITextChannel)
            {
                var channelParam = channel as ITextChannel;
                await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**New Text Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
                    + "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
                    + "\nTopic: " + channelParam.Topic + "```");
            }
            else if (channel is IVoiceChannel)
            {
                var channelParam = channel as IVoiceChannel;
                await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**New Voice Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
                    + "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
                    + "\nUser Limit: " + channelParam.UserLimit + "```");
            }
            else
            {
                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("error");
                Console.ResetColor();
                Console.WriteLine("]    " + ": " + channel.Id + " type is unknown.");
            }
        }
        //private async Task ChannelUpdated(SocketChannel cachedChannel, SocketChannel channel)
        //{
        //    if(channel is ITextChannel && cachedChannel is ITextChannel)
        //    {
        //        var cachedChannelParam = cachedChannel as ITextChannel;
        //        var channelParam = channel as ITextChannel;
        //        await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**Text Channel Updated**\n```ID: " + cachedChannelParam.Id + " / " + channelParam.Id 
        //            + "\nOld Name: " + cachedChannelParam.Name
        //            + "\nNew Name: " + channelParam.Name
        //            + "\nGuild ID: " + cachedChannelParam.GuildId + " / " + channelParam.GuildId
        //            + "\nGuild: " + cachedChannelParam.Guild.Name + " / " + channelParam.Guild.Name
        //            + "\nOld Topic: " + cachedChannelParam.Topic
        //            + "\nNew Topic: " + channelParam.Topic + "```");
        //    }
        //    else if(channel is IVoiceChannel && cachedChannel is IVoiceChannel)
        //    {
        //        var cachedChannelParam = cachedChannel as IVoiceChannel;
        //        var channelParam = channel as IVoiceChannel;
        //        await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**Text Channel Updated**\n```ID: " + cachedChannelParam.Id + " / " + channelParam.Id
        //            + "\nOld Name: " + cachedChannelParam.Name
        //            + "\nNew Name: " + channelParam.Name
        //            + "\nGuild ID: " + cachedChannelParam.GuildId + " / " + channelParam.GuildId
        //            + "\nGuild: " + cachedChannelParam.Guild.Name + " / " + channelParam.Guild.Name
        //            + "\nOld User Limit: " + cachedChannelParam.UserLimit
        //            + "\nNew User Limit: " + channelParam.UserLimit + "```");
        //    }
        //    else
        //    {
        //        Console.Write("status: [");
        //        Console.ForegroundColor = ConsoleColor.DarkRed;
        //        Console.Write("error");
        //        Console.ResetColor();
        //        Console.WriteLine("]    " + ": " + cachedChannel.Id + " does not match types with " + channel.Id);
        //    }
        //}
        private async Task ChannelDestroyed(SocketChannel channel)
        {
            if (channel is ITextChannel)
            {
                var channelParam = channel as ITextChannel;
                await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**Removed Text Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
                    + "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
                    + "\nTopic: " + channelParam.Topic + "```");
            }
            else if (channel is IVoiceChannel)
            {
                var channelParam = channel as IVoiceChannel;
                await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**Removed Voice Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
                    + "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
                    + "\nUser Limit: " + channelParam.UserLimit + "```");
            }
            else
            {
                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.Write("error");
                Console.ResetColor();
                Console.WriteLine("]    " + ": " + channel.Id + " type is unknown.");
            }
        }

        private async Task MessageUpdated(Cacheable<IMessage, ulong> cachedMessage, SocketMessage message, ISocketMessageChannel channel)
        {
            var msg = message as SocketUserMessage;
            MessageLogger.logEditMessage(msg);
        }
        private async Task MessageDeleted(Cacheable<IMessage, ulong> cachedMessage, ISocketMessageChannel channel)
        {
            var message = cachedMessage.Value as SocketUserMessage;
            MessageLogger.logDeleteMessage(message);
        }

        private async Task HandleQuoteReactions(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
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
                sb.Append(((i + 1) + ((QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + quotes[i] + "\n");
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
        private async Task HandleRequestQuoteReactions(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            // Check to see if the next page or previous page was clicked.
            if (reaction.Emoji.Name == Extensions.Extensions.arrow_left)
            {
                if (QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] == 1)
                    return;

                QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)]--;
            }
            else if (reaction.Emoji.Name == Extensions.Extensions.arrow_right)
            {
                if (QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] == QuoteHandler.getRequestQuotesListLength)
                    return;

                QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)]++;
            }

            StringBuilder sb = new StringBuilder()
            .Append("**Request Quote List** : *Page " + QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] + "*\nTo accept a quote, type **" + GuildConfiguration.Load(channel.getGuild().Id).Prefix + "acceptquote[id]**.\nTo reject a quote, type **" + GuildConfiguration.Load(channel.getGuild().Id).Prefix + "denyquote[id]**.\n```");

            List<string> requestQuotes = QuoteHandler.getRequestQuotes(QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)]);

            for (int i = 0; i < requestQuotes.Count; i++)
            {
                sb.Append(((i + 1) + ((QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + requestQuotes[i] + "\n");
            }

            sb.Append("```");

            await message.Value.ModifyAsync(msg => msg.Content = sb.ToString());
            await message.Value.RemoveAllReactionsAsync();

            if (QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] == 1)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else if (QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] == QuoteHandler.getRequestQuotesListLength)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
            }
            else
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
        }
        private async Task HandleTransactionReactions(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            // Check to see if the next page or previous page was clicked.
            if (reaction.Emoji.Name == Extensions.Extensions.arrow_left)
            {
                if (TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] == 1)
                    return;

                TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)]--;
            }
            else if (reaction.Emoji.Name == Extensions.Extensions.arrow_right)
            {
                if (TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] == QuoteHandler.getRequestQuotesListLength)
                    return;

                TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)]++;
            }

            StringBuilder sb = new StringBuilder()
                .Append("**Transactions**\n**----------------**\n`Total Transactions: " + TransactionLogger.transactionsList.Count + "`\n```");

            List<string> transactions = TransactionLogger.GetSplicedTransactions(TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)]);

            for (int i = 0; i < transactions.Count; i++)
            {
                sb.Append(((i + 1) + ((TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + transactions[i] + "\n");
            }
            
            sb.Append("``` `Page " + TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] + "`");

            await message.Value.ModifyAsync(msg => msg.Content = sb.ToString());
            await message.Value.RemoveAllReactionsAsync();

            if (TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] == 1)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else if (TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] == QuoteHandler.getRequestQuotesListLength)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
            }
            else
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
        }

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
                return;
            
            if (QuoteHandler.quoteMessages.Contains(message.Id))
            {
                await HandleQuoteReactions(message, channel, reaction);
                return;
            }

            if (QuoteHandler.requestQuoteMessages.Contains(message.Id))
            {
                await HandleRequestQuoteReactions(message, channel, reaction);
                return;
            }

            if(TransactionLogger.transactionMessages.Contains(message.Id))
            {
                await HandleTransactionReactions(message, channel, reaction);
                return;
            }

            // Coin System to add a coin for each reaction that is added to a message.
            if (Configuration.Load().CoinsForReactions)
            {
                AwardCoinsToPlayer(reaction.UserId);
            }
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

                string wMsg1 = GuildConfiguration.Load(e.Guild.Id).WelcomeMessage.Replace("{USERJOINED}", e.Mention).Replace("{GUILDNAME}", e.Guild.Name);
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
            // Adds the message to the log file
            MessageLogger.logNewMessage(message);

            if (message == null) return;
            if (message.Author.IsBot) return;

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
            if (!(message.HasStringPrefix(GuildConfiguration.Load(message.Channel.getGuild().Id).Prefix, ref argPos) || message.HasMentionPrefix(_bot.CurrentUser, ref argPos) || message.HasStringPrefix(User.Load(message.Author.Id).CustomPrefix, ref argPos))) // Configuration.Load().Prefix
            {
                // Coin System to add a coin for each message the user sends.
                AwardCoinsToPlayer(message.Author.Id);

                return;
            }

            var context = new CommandContext(_bot, message);
            // Execute Commands in dir \Modules
            var result = await commandService.ExecuteAsync(context, argPos, dependencyMap);

            // If the command modules doesn't contain a task for the message, return an error iff UnknownCommand is enabled
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

                message.DeleteAfter(20);
                errorMessage.DeleteAfter(20);

                return;
            }
        }

        private static void AwardCoinsToPlayer(ulong UserId, int CoinsToAward = 1)
        {
            try
            {
                User.UpdateJson(UserId, "Coins", (User.Load(UserId).Coins + CoinsToAward));
            }
            catch (Exception e) { Console.WriteLine(e); }

            return;
        }
    }
}
