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

using MelissasCode;
using Discord.Commands;
using System.Reflection;

namespace DiscordBot
{
    class Program
    {
        public static DiscordSocketClient _bot;
        public static CommandService commandService;
        public static DependencyMap dependencyMap;

        static void Main(string[] args)
            => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            Configuration.EnsureExists();
            QuoteHandler.EnsureExists();
            VoteLinkHandler.EnsureExists();

            _bot = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                MessageCacheSize = 50,
                WebSocketProvider = WS4NetProvider.Instance,
                UdpSocketProvider = UDPClientProvider.Instance,
            });
            commandService = new CommandService();
            dependencyMap = new DependencyMap();

            _bot.Log += Log;
            
            _bot.UserJoined += UserJoined;
            _bot.UserLeft += UserLeft;
            
            _bot.Ready += Ready;

            _bot.ReactionAdded += ReactionAdded;

            await InstallCommands();
            _bot.MessageDeleted += MessageDeleted;
            _bot.MessageUpdated += MessageUpdated;

            await _bot.LoginAsync(TokenType.Bot, DiscordToken.MogiiBot);
            await _bot.StartAsync();

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

                await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("", false, eb);
            }
            else if (e.Guild.Id == Configuration.Load().NSFWServerID)
            {
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("NSFW Server - User Left")
                    .WithDescription("@" + e.Username + "\n" + e.Nickname + "\n" + e.Id)
                    .WithColor(new Color(255, 28, 28))
                    .WithThumbnailUrl(e.GetAvatarUrl())
                    .WithCurrentTimestamp();

                await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("", false, eb);
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

                await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("", false, eb);
                
                string wMsg1 = Configuration.Load().welcomeMessage.Replace("{USERJOINED}", e.Mention).Replace("{GUILDNAME}", e.Guild.Name);
                await GetHandler.getTextChannel(Configuration.Load().WelcomeChannelID).SendMessageAsync(wMsg1);
            }
            else if(e.Guild.Id == Configuration.Load().NSFWServerID)
            {
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("NSFW Server - User Joined")
                    .WithDescription("@" + e.Username + "\n" + e.Id)
                    .WithColor(new Color(28, 255, 28))
                    .WithThumbnailUrl(e.GetAvatarUrl())
                    .WithCurrentTimestamp();

                await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("", false, eb);
            }

            if(User.CreateUserFile(e.Id))
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

            // Secret Stuff (Remove if compiler error presented.)
            if (message.Author.Id == Configuration.Load().ListenForBot && MelissaCode.DoTopSecretThings(Cryptography.KEY))
            {
                await GetHandler.getTextChannel(Configuration.Load().ForwardMessagesTo).SendMessageAsync(message.Content);
            }
            
            try
            {
                User.UpdateJson(message.Author.Id, "Coins", (User.Load(message.Author.Id).Coins + 1));
            }
            catch (Exception e) { Console.WriteLine(e); }

            if (message == null) return;
            if (message.Author.IsBot) return;

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

                await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("", false, eb);
            }

            if(message.Content.ToUpper() == "F")
            {
                await message.Channel.SendMessageAsync("Respects have been paid.");
                return;
            }

            int argPos = 0;

            if (!(message.HasStringPrefix(Configuration.Load().Prefix, ref argPos) || message.HasMentionPrefix(_bot.CurrentUser, ref argPos))) return;

            var context = new CommandContext(_bot, message);
            var result = await commandService.ExecuteAsync(context, argPos, dependencyMap);
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", " + result.ErrorReason);
            }
            
            Logging.MessageLogger.logNewMessage(message);
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
    }
}
