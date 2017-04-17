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
        public async Task SendChannelMessage([Summary("The channel id to send the message to.")] ulong channel, [Remainder]string message)
        {
            await GetHandler.getTextChannel(channel).SendMessageAsync(message);

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName("author: @" + Context.User.Username);
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("Message Sent to #" + GetHandler.getTextChannel(channel).Name + " | Sent by @" + Context.User.Username);

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithColor(new Color(181, 81, 215))
                .WithDescription(message)
                .WithFooter(efb)
                .WithCurrentTimestamp();

            await ReplyAsync("", false, eb);
        }

        [Command("privatemessage"), Summary("Sends a private message to the user specified.")]
        [Alias("pm", "dm", "directmessage")]
        public async Task SendPrivateMessage([Summary("The user to send the message to.")] IUser user, [Remainder]string message)
        {
            await user.CreateDMChannelAsync().Result.SendMessageAsync(message);

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName("to: @" + user.Username);
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("Message Sent to @" + user.Username + " | Sent by @" + Context.User.Username);

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithTitle("from: @" + Context.User.Username)
                .WithColor(new Color(181, 81, 215))
                .WithDescription(message)
                .WithFooter(efb)
                .WithCurrentTimestamp();

            await ReplyAsync("", false, eb);
        }
    }
}
