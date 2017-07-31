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

        [Command("setwelcome"), Summary("Set the welcome message to the specified string. (Use `{USERJOINED}` to mention the user and `{GUILDNAME}` to name the guild.")]
        [Alias("setwelcomemessage", "sw")]
        public async Task SetWelcomeMessage([Remainder]string message = null)
        {
            if (message == null)
            {
                StringBuilder sb = new StringBuilder()
                    .Append("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "setwelcome [Welcome Message]\n\n")
                    .Append("```Available Flags\n")
                    .Append("{USERJOINED} - @" + Context.User.Username + "\n")
                    .Append("{GUILDNAME} - " + Context.Guild.Name + "\n")
                    .Append("\nFlags need to be in CAPITAL LETTERS!```");

                await ReplyAsync(sb.ToString());
                return;
            }

            GuildConfiguration.UpdateJson(Context.Guild.Id, "WelcomeMessage", message);
            await ReplyAsync("Welcome message has been changed successfully by " + Context.User.Mention + "\n**SAMPLE WELCOME MESSAGE**\n" + GuildConfiguration.Load(Context.Guild.Id).WelcomeMessage.Replace("{USERJOINED}", Context.User.Mention).Replace("{GUILDNAME}", Context.Guild.Name));
        }
    }
}
