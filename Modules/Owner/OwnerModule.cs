using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
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
    [Name("Owner Commands")]
    [MinPermissions(PermissionLevel.BotOwner)]
    public class OwnerModule : ModuleBase
    {
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

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " updated " + user.Mention + "'s footer successfully.")
                .WithColor(Color.DarkGreen);
            var message = await ReplyAsync("", false, eb.Build());

            Context.Message.DeleteAfter(10);
            message.DeleteAfter(10);
        }

        [Command("editiconurl")]
        public async Task EditIconUrl(IUser user = null, string position = null, string url = null)
        {
            if (user == null || position == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "editiconurl [@User] [Author/Footer] [Link to Icon/Image]");
                return;
            }

            var eb = new EmbedBuilder();
            string oldLink, newLink = url;

            if (url.Contains('<') && url.Contains('>'))
            {
                newLink = url.FindAndReplaceFirstInstance("<", "");
                newLink = newLink.FindAndReplaceFirstInstance(">", "");
            }

            switch (position.ToUpper())
            {
                case "AUTHOR":
                    oldLink = User.Load(user.Id).EmbedAuthorBuilderIconUrl;
                    User.UpdateJson(user.Id, "EmbedAuthorBuilderIconUrl", newLink);
                    eb.WithColor(Color.DarkGreen);
                    eb.WithDescription(Context.User.Username + " successfully updated " + user.Mention + "'s Author Icon to: " + url);
                    eb.WithFooter("Old Link: " + oldLink);
                    await ReplyAsync("", false, eb.Build());
                    break;
                case "FOOTER":
                    oldLink = User.Load(user.Id).EmbedAuthorBuilderIconUrl;
                    User.UpdateJson(user.Id, "EmbedFooterBuilderIconUrl", newLink);
                    eb.WithColor(Color.DarkGreen);
                    eb.WithDescription(Context.User.Username + " successfully updated " + user.Mention + "'s Footer Icon to: " + url);
                    eb.WithFooter("Old Link: " + oldLink);
                    await ReplyAsync("", false, eb.Build());
                    break;
                default:
                    eb.WithColor(Color.DarkGreen);
                    eb.WithDescription("");
                    await ReplyAsync("", false, eb.Build());
                    break;
            }
        }

        [Command("botignore"), Summary("Make the bot ignore a user.")]
        public async Task BotIgnore(IUser user)
        {
            User.UpdateJson(user.Id, "IsBotIgnoringUser", !User.Load(user.Id).IsBotIgnoringUser);

            if(User.Load(user.Id).IsBotIgnoringUser)
            {
                await ReplyAsync(Context.User.Mention + ", " + MogiiBot3.Bot.CurrentUser.Username + " will start ignoring " + user.Mention);
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", " + MogiiBot3.Bot.CurrentUser.Username + " will start to listen to " + user.Mention);
            }
        }

        [Command("resetallcoins")]
        public async Task SetCoinsForAll(string confirmation = null)
        {
            Context.Message.DeleteAfter(30);

            if (confirmation == "confirm")
            {
                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has forced all users coins to reset value.");

                User.SetCoinsForAll();

                var eb = new EmbedBuilder()
                    .WithDescription(Context.User.Username + " has successfully reset the coin value for all users.")
                    .WithColor(Color.DarkGreen)
                    .WithCurrentTimestamp()
                    .WithFooter("Info: This command has been logged successfully to " + Configuration.Load().LogChannelId.GetTextChannel().Name);

                await ReplyAsync("", false, eb.Build());
            }
            else
            {
                var message = await ReplyAsync("**Warning**\nIssuing this command will **reset all users coins**. This action is irreversible and any data not backed-up will be lost. Please ensure that you create a backup of the data if you wish to roll-back to the current state. If you wish to issue this command, please type `" + GuildConfiguration.Load(Context.Guild.Id).Prefix + "resetallcoins confirm`");
                message.DeleteAfter(60);
            }
        }
    }
}