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

using MelissasCode;
using Discord.Commands;
using System.Reflection;

namespace DiscordBot
{
    class Program
    {
        public static DiscordSocketClient _bot;
        private CommandService commands;
        private DependencyMap map;

        static void Main(string[] args)
            => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            Common.Configuration.EnsureExists();

            _bot = new DiscordSocketClient(new DiscordSocketConfig
            {
                LogLevel = LogSeverity.Debug,
                WebSocketProvider = WS4NetProvider.Instance,
                UdpSocketProvider = UDPClientProvider.Instance,
            });
            commands = new CommandService();
            map = new DependencyMap();

            _bot.Log += Log;
            
            _bot.UserJoined += UserJoined;
            _bot.UserLeft += UserLeft;

            _bot.Ready += Ready;

            await InstallCommands();

            await _bot.LoginAsync(TokenType.Bot, DiscordToken.MogiiBot);
            await _bot.StartAsync();

            await Task.Delay(-1);

            await _bot.LogoutAsync();
        }

        private async Task Ready()
        {
            await _bot.SetGameAsync(Configuration.Load().Playing);
            await _bot.SetStatusAsync(Configuration.Load().Status);

            Modules.Public.InfoModule._dt = DateTime.Now;
        }

        private async Task UserLeft(SocketGuildUser e)
        {
            if (e.Guild.Id == 221250721046069249)
            {
                await GetHandler.getTextChannel(225721556435730433).SendMessageAsync("Hey " + e.Mention + ", and welcome to the " + e.Guild.Name + " Discord Server! If you are a player on our Minecraft Server, tell us your username and a Staff Member will grant you the MC Players Role \n\nIf you don't mind, could you fill out this form linked below. We are collecting data on how you found out about us, and it'd be great if we had your input. The form can be found here: <https://goo.gl/forms/iA9t5xjoZvnLJ5np1>");
                await GetHandler.getTextChannel(235179833053675522).SendMessageAsync(e.Mention + " has joined the server.");
            }
        }

        private async Task UserJoined(SocketGuildUser e)
        {
            if (e.Guild.Id == 221250721046069249)
            {
                await GetHandler.getTextChannel(235179833053675522).SendMessageAsync(e.Mention + " has left the server.");
            }
        }

        public async Task InstallCommands()
        {
            _bot.MessageReceived += MessageReceived;

            await commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        private async Task MessageReceived(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;

            if (message == null) return;
            if (message.Author.IsBot) return;

            if(message.Content.ToUpper() == "F")
            {
                await message.Channel.SendMessageAsync("Respects have been paid.");
                return;
            }

            int argPos = 0;

            if (!(message.HasStringPrefix(Configuration.Load().Prefix, ref argPos) || message.HasMentionPrefix(_bot.CurrentUser, ref argPos))) return;

            var context = new CommandContext(_bot, message);
            var result = await commands.ExecuteAsync(context, argPos, map);
            if (!result.IsSuccess)
            {
                await context.Channel.SendMessageAsync(messageParam.Author.Mention + ", " + result.ErrorReason);
            }
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
