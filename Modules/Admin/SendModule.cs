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
using DiscordBot.Extensions;

using MelissasCode;

namespace DiscordBot.Modules.Admin
{
    [Name("Send Commands")]
    [Group("send")]
    [RequireContext(ContextType.Guild | ContextType.DM)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class SendModule : ModuleBase
    {
        [Command("")]
        public async Task Send()
        {
            await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "send channelmessage [cmessage/channelmsg] / privatemessage [pm/dm/directmessage]");
        }

        [Command("channelmessage"), Summary("Sends a message to the channel specified.")]
        [Alias("cmessage", "channelmsg")]
        public async Task SendChannelMessage([Summary("The channel id to send the message to.")] ulong channel = 0, [Remainder]string message = null)
        {
            if(channel == 0 || message == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "send channelmessage [Channel ID] [Message]");
                return;
            }

            await channel.GetTextChannel().SendMessageAsync(message);
            //await GetHandler.getTextChannel(channel).SendMessageAsync(message);

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName("author: @" + Context.User.Username);
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("Message Sent to #" + channel.GetTextChannel().Name + " | Sent by @" + Context.User.Username);
                //.WithText("Message Sent to #" + GetHandler.getTextChannel(channel).Name + " | Sent by @" + Context.User.Username);

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
        public async Task SendPrivateMessage([Summary("The user to send the message to.")] IUser user = null, [Remainder]string message = null)
        {
            if (user == null || message == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "send privatemessage [@User] [Message]");
                return;
            }

            await user.GetOrCreateDMChannelAsync().Result.SendMessageAsync(message);

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
