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

namespace DiscordBot.Modules.SOwner
{
    [MinPermissions(PermissionLevel.ServerOwner)]
    [Name("Server Owner Commands")]
    public class ServerOwnerModule : ModuleBase
    {
        [Command("guildprefix"), Summary("Set the prefix for the bot for the server.")]
        public async Task SetPrefix(string prefix)
        {
            GuildConfiguration.UpdateJson(Context.Guild.Id, "Prefix", prefix);
            await ReplyAsync(Context.User.Mention + " has updated the Prefix to: " + prefix);
        }
    }
}
