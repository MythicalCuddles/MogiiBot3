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

        [Command("setupdatabase"), Summary("Adds all the users to the database.")]
        public async Task SetUpDatabase()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("**Database Report**\n```");

            foreach (SocketGuild g in Program._bot.Guilds)
            {
                if (g.Id == Configuration.Load().ServerID)
                {
                    foreach (SocketGuildUser u in g.Users)
                    {
                        if(User.CreateUserFile(u.Id))
                        {
                            sb.Append(u.Username + " - Added. [" + u.Id + "]\n");
                        }
                        else
                        {
                            sb.Append(u.Username + " - Already Added. [" + u.Id + "]\n");
                        }
                    }
                }
            }

            sb.Append("```");

            await ReplyAsync(sb.ToString());
            sb.Clear();
        }

        [Command("testwelcome"), Summary("")]
        public async Task TestWelcomeMessage(IUser testWithUser)
        {
            await ReplyAsync(Configuration.Load().welcomeMessage.Replace("{USERJOINED}", testWithUser.Mention).Replace("{GUILDNAME}", Context.Guild.Name));
        }
    }
}