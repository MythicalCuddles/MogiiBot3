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
    }
}
