using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;

using MelissasCode;

namespace DiscordBot.Modules.Admin
{
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class AdminModule : ModuleBase
    {
        [Command("say"), Summary("Echos a message.")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
            await Context.Message.DeleteAsync();
        }

        [Command("togglesenpai"), Summary("Toggles the senpai command.")]
        public async Task ToggleSenpai()
        {
            Configuration.UpdateJson("SenpaiEnabled", !Configuration.Load().SenpaiEnabled);
            await GetHandler.getTextChannel(235179833053675522).SendMessageAsync("Senpai has been toggled by " + Context.User.Mention + " (" + Configuration.Load().SenpaiEnabled + ")");
        }

        [Command("playing"), Summary("Changes the playing message of the bot.")]
        public async Task PlayingMessage([Remainder, Summary("Playing Message")] string playingMessage)
        {
            Configuration.UpdateJson("Playing", playingMessage);
            await Program._bot.SetGameAsync(playingMessage);
            await Context.Message.DeleteAsync();
        }

        [Command("twitch"), Summary("Sets the twitch streaming link. Type \"none\" to disable.")]
        [Alias("streaming", "twitchstreaming")]
        public async Task SetTwitchStreamingBot(string linkOrValue)
        {
            if (linkOrValue.Contains("twitch.tv"))
            {
                await Program._bot.SetGameAsync(Configuration.Load().Playing, linkOrValue, StreamType.Twitch);
                await ReplyAsync(Context.User.Mention + ", my status has been updated to streaming with the Twitch.TV link of <" + linkOrValue + ">");
            }
            else
            {
                await Program._bot.SetGameAsync(Configuration.Load().Playing);
                await ReplyAsync(Context.User.Mention + ", disabling streaming with the bot.");
            }
        }

        [Command("welcome"), Summary("Send the welcome messages to the user specified.")]
        public async Task SendWelcomeMessage(IUser user)
        {
            await GetHandler.getTextChannel(225721556435730433).SendMessageAsync("Hey " + user.Mention + ", and welcome to the " + Context.Guild.Name + " Discord Server! If you are a player on our Minecraft Server, tell us your username and a Staff Member will grant you the MC Players Role \n\nIf you don't mind, could you fill out this form linked below. We are collecting data on how you found out about us, and it'd be great if we had your input. The form can be found here: <https://goo.gl/forms/iA9t5xjoZvnLJ5np1>");
            await GetHandler.getTextChannel(235179833053675522).SendMessageAsync(user.Mention + " has joined the server.");
        }
    }
}
