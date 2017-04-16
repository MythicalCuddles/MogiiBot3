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

namespace DiscordBot.Modules.Owner
{
    [MinPermissions(PermissionLevel.BotOwner)]
    public class OwnerModule : ModuleBase
    {
        [Command("prefix"), Summary("Set the prefix for the bot.")]
        public async Task SetPrefix(string prefix)
        {
            Configuration.UpdateJson("Prefix", prefix);
            await ReplyAsync(Context.User.Mention + " has updated the Prefix to: " + prefix);
        }
    }
}