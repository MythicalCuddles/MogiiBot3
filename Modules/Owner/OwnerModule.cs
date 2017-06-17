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

namespace DiscordBot.Modules.Owner
{
    [MinPermissions(PermissionLevel.BotOwner)]
    public class OwnerModule : ModuleBase
    {
        [Command("setupdatabase"), Summary("Adds all the users to the database.")]
        public async Task SetUpDatabase()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("**Database Report**\n```");

            foreach (SocketGuild g in MogiiBot3._bot.Guilds)
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

        [Command("addteammember"), Summary("Add a member to the team. Gives them a star on $about")]
        public async Task AddTeamMember(IUser user)
        {
            if (!User.Load(user.Id).TeamMember)
            {
                User.UpdateJson(user.Id, "TeamMember", true);
                await ReplyAsync(user.Mention + " has been added to the team by " + Context.User.Mention);
            }
            else
            {
                await ReplyAsync(user.Mention + " is already part of the team, " + Context.User.Mention + "!");
            }
        }

        [Command("removeteammember"), Summary("Add a member to the team. Gives them a star on $about")]
        public async Task RemoveTeamMember(IUser user)
        {
            if (User.Load(user.Id).TeamMember)
            {
                User.UpdateJson(user.Id, "TeamMember", false);
                await ReplyAsync(user.Mention + " has been removed from the team by " + Context.User.Mention);
            }
            else
            {
                await ReplyAsync(user.Mention + " is not part of the team, " + Context.User.Mention + "!");
            }
        }

        [Command("editfooter")]
        public async Task EditFooter(IUser user, [Remainder]string footer)
        {
            if(user == null || footer == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "editfooter [@User] [Footer]");
                return;
            }

            User.UpdateJson(user.Id, "FooterText", footer);
            var message = await ReplyAsync("Updated.");

            Context.Message.DeleteAfter(10);
            message.DeleteAfter(10);
        }

        [Command("botignore"), Summary("Make the bot ignore a user.")]
        public async Task BotIgnore(IUser user)
        {
            User.UpdateJson(user.Id, "IsBotIgnoringUser", !User.Load(user.Id).IsBotIgnoringUser);

            if(User.Load(user.Id).IsBotIgnoringUser)
            {
                await ReplyAsync(Context.User.Mention + ", " + MogiiBot3._bot.CurrentUser.Username + " will start ignoring " + user.Mention);
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", " + MogiiBot3._bot.CurrentUser.Username + " will start to listen to " + user.Mention);
            }
        }
    }
}