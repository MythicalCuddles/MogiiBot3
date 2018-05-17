using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Extensions;
using MelissaNet;

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
            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle(e.Guild.Name + " - User Left")
                .WithDescription("")
                .WithCurrentTimestamp()
                .WithThumbnailUrl(e.GetAvatarUrl())
                .WithColor(new Color(255, 28, 28))
                .WithFooter("ID: " + e.Id);

            if (e.GetAbout() != null)
                eb.AddField("About " + e.Username, e.GetAbout());

            eb.AddField("Full Username", "@" + e.Username + "#" + e.DiscriminatorValue);

            if (e.GetName() != null)
                eb.AddField("Name", e.GetName(), true);

            if (e.GetGender() != null)
                eb.AddField("Gender", e.GetGender(), true);

            if (e.GetPronouns() != null)
                eb.AddField("Pronouns", e.GetPronouns(), true);

            eb.AddField("Coins", e.GetCoins(), true);
            eb.AddField("Account Created", e.UserCreateDate(), true);
            eb.AddField("Joined Guild", e.GuildJoinDate(), true);
            eb.AddField("Team Member", e.IsTeamMember().ToYesNo(), true);

            if (e.GetMinecraftUser() != null)
                eb.AddField("Minecraft Username", e.GetMinecraftUser(), true);

            if (e.GetSnapchatUsername() != null)
                eb.AddField("Snapchat", "[" + e.GetSnapchatUsername() + "](https://www.snapchat.com/add/" + e.GetSnapchatUsername() + "/)", true);

            if (e.GetCustomPrefix() != null)
                eb.AddField("Custom Prefix", e.GetCustomPrefix(), true);

            await GuildConfiguration.Load(e.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
        }
    }
}
