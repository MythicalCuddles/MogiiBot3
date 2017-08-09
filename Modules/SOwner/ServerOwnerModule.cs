using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;

using MelissasCode;

namespace DiscordBot.Modules.SOwner
{
    [MinPermissions(PermissionLevel.ServerOwner)]
    [Name("Server Owner Commands")]
    public class ServerOwnerModule : ModuleBase
    {
        //public ulong RuleGambleChannelId { get; set; } = 0;

        [Command("guildprefix"), Summary("Set the prefix for the bot for the server.")]
        public async Task SetPrefix(string prefix)
        {
            GuildConfiguration.UpdateJson(Context.Guild.Id, "Prefix", prefix);
            await ReplyAsync(Context.User.Mention + " has updated the Prefix to: " + prefix);
        }

        [Command("setwelcome"), Summary("Set the welcome message to the specified string. (Use `{USERJOINED}` to mention the user and `{GUILDNAME}` to name the guild.")]
        [Alias("setwelcomemessage", "sw")]
        public async Task SetWelcomeMessage([Remainder]string message = null)
        {
            if (message == null)
            {
                StringBuilder sb = new StringBuilder()
                    .Append("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "setwelcome [Welcome Message]\n\n")
                    .Append("```Available Flags\n")
                    .Append("---------------\n")
                    .Append("{USERMENTION} - @" + Context.User.Username + "\n")
                    .Append("{USERNAME} - " + Context.User.Username + "\n")
                    .Append("{GUILDNAME} - " + Context.Guild.Name + "\n")
                    .Append("{DISCRIMINATOR} - " + Context.User.Discriminator + "\n")
                    .Append("{AVATARURL} - " + Context.User.GetAvatarUrl() + "\n")
                    .Append("{USERID} - " + Context.User.Id + "\n")
                    .Append("{USERCREATEDATE} - " + Context.User.UserCreateDate() + "\n")
                    .Append("{USERJOINDATE} - " + Context.User.GuildJoinDate() + "\n")
                    .Append("```");

                await ReplyAsync(sb.ToString());
                return;
            }

            GuildConfiguration.UpdateJson(Context.Guild.Id, "WelcomeMessage", message);
            await ReplyAsync("Welcome message has been changed successfully by " + Context.User.Mention + "\n**SAMPLE WELCOME MESSAGE**\n" + GuildConfiguration.Load(Context.Guild.Id).WelcomeMessage.FormatWelcomeMessage(Context.User as SocketGuildUser));
        }

        [Command("welcomechannel"), Summary("Set the welcome channel for the server.")]
        public async Task SetWelcomeChannel(SocketTextChannel channel)
        {
            GuildConfiguration.UpdateJson(Context.Guild.Id, "WelcomeChannelId", channel.Id);
            await ReplyAsync(Context.User.Mention + " has updated the Welcome Channel to: " + channel.Mention);
        }

        [Command("logchannel"), Summary("Set the log channel for the server.")]
        public async Task SetLogChannel(SocketTextChannel channel)
        {
            GuildConfiguration.UpdateJson(Context.Guild.Id, "LogChannelId", channel.Id);
            await ReplyAsync(Context.User.Mention + " has updated the Log Channel to: " + channel.Mention);
        }

        [Command("togglesenpai"), Summary("Toggles the senpai command.")]
        public async Task ToggleSenpai()
        {
            GuildConfiguration.UpdateJson(Context.Guild.Id, "SenpaiEnabled", !GuildConfiguration.Load(Context.Guild.Id).SenpaiEnabled);
            await GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("Senpai has been toggled by " + Context.User.Mention + " (enabled: " + GuildConfiguration.Load(Context.Guild.Id).SenpaiEnabled + ")");
        }

        [Command("togglequotes"), Summary("")]
        public async Task ToggleQuotes()
        {
            GuildConfiguration.UpdateJson(Context.Guild.Id, "QuotesEnabled", !GuildConfiguration.Load(Context.Guild.Id).QuotesEnabled);
            await GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("Quotes have been toggled by " + Context.User.Mention + " (enabled: " + GuildConfiguration.Load(Context.Guild.Id).QuotesEnabled + ")");
        }

        [Command("togglensfwstatus"), Summary("")]
        public async Task ToggleNSFWStatus()
        {
            GuildConfiguration.UpdateJson(Context.Guild.Id, "EnableNSFWCommands", !GuildConfiguration.Load(Context.Guild.Id).EnableNSFWCommands);
            await GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("NSFW Server Status have been toggled by " + Context.User.Mention + " (NSFW Server? " + GuildConfiguration.Load(Context.Guild.Id).EnableNSFWCommands + ")");
        }

        [Command("rule34channel"), Summary("Set the Rule34 channel for the server.")]
        public async Task SetNSFWRule34Channel(SocketTextChannel channel)
        {
            GuildConfiguration.UpdateJson(Context.Guild.Id, "RuleGambleChannelId", channel.Id);
            await ReplyAsync(Context.User.Mention + " has updated the Rule34 Gamble Channel to: " + channel.Mention);
        }
    }
}
