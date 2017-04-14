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
    [Group("send")]
    [RequireContext(ContextType.Guild | ContextType.DM)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class SendModule : ModuleBase
    {
        [Command("channelmessage"), Summary("Sends a message to the channel specified.")]
        [Alias("cmessage", "channelmsg")]
        public async Task SendChannelMessage([Summary("The channel id to send the message to.")] SocketTextChannel channel)
        {
            await ReplyAsync(channel.Name);
        }

        [Command("privatemessage"), Summary("Sends a private message to the user specified.")]
        [Alias("pm", "dm", "directmessage")]
        public async Task SendPrivateMessage([Summary("The user to send the message to.")] IUser user, [Remainder]string message)
        {
            await user.CreateDMChannelAsync().Result.SendMessageAsync(message);
        }
    }
}
