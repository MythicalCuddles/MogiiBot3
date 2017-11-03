using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using Discord.WebSocket;

using DiscordBot;
using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Other;
using DiscordBot.Logging;

using MelissasCode;

namespace DiscordBot.Handlers
{
    public class UserHandler
    {
        public static async Task UserJoined(SocketGuildUser e)
        {
            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle(e.Guild.Name + " - User Joined")
                .WithDescription("@" + e.Username + "\n" + e.Id)
                .WithColor(new Color(28, 255, 28))
                .WithThumbnailUrl(e.GetAvatarUrl())
                .WithCurrentTimestamp();
                
            if(GuildConfiguration.Load(e.Guild.Id).WelcomeMessage != null || GuildConfiguration.Load(e.Guild.Id).WelcomeChannelId != 0)
                await GuildConfiguration.Load(e.Guild.Id).WelcomeChannelId.GetTextChannel().SendMessageAsync(GuildConfiguration.Load(e.Guild.Id).WelcomeMessage.ModifyStringFlags(e));

            await GuildConfiguration.Load(e.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());

            if (User.CreateUserFile(e.Id))
            {
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(e.Username + " was successfully added to the database. [" + e.Id + "]");
            }
        }

        public static async Task UserLeft(SocketGuildUser e)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("**Username:** @" + e.Username + "\n");
            if (e.Nickname != null)
            {
                sb.Append("**Nickname:** " + e.Nickname + "\n");
            }
            sb.Append("**Id: **" + e.Id + "\n");
            sb.Append("**Joined: **" + e.GuildJoinDate() + "\n");
            sb.Append("\n");
            sb.Append("**Coins: **" + User.Load(e.Id).Coins + "\n");

            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle(e.Guild.Name + " - User Left")
                .WithDescription(sb.ToString())
                .WithColor(new Color(255, 28, 28))
                .WithThumbnailUrl(e.GetAvatarUrl())
                .WithCurrentTimestamp();
            
            await GuildConfiguration.Load(e.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
        }
    }
}
