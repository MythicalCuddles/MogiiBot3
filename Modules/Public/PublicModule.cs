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
using DiscordBot.Logging;

using MelissasCode;

namespace DiscordBot.Modules.Public
{
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class PublicModule : ModuleBase
    {
        [Command("hug"), Summary("Give your friend a hug!")]
        public async Task HugUser(IUser user)
        {
            await ReplyAsync(Context.User.Mention + " has given a massive hug to " + user.Mention);
        }

        [Command("setabout"), Summary("Set your about message!")]
        public async Task SetUserAbout([Remainder]string aboutMessage)
        {
            User.UpdateJson(Context.User.Id, "About", aboutMessage);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setaboutrgb"), Summary("Custom set the color of the about embed message.")]
        public async Task SetUserAboutRGB(int r = -1, int g = -1, int b = -1)
        {
            if(r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
            {
                await ReplyAsync(Context.User.Mention + ", you have entered an invalid value. You can use this website to help get your RGB values - <http://www.colorhexa.com/>\n\n" +
                    "**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "setaboutrgb [R value] [G value] [B value]\n**Example:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "setaboutrgb 140 90 210");
                return;
            }

            User.UpdateJson(Context.User.Id, "AboutR", r);
            User.UpdateJson(Context.User.Id, "AboutG", g);
            User.UpdateJson(Context.User.Id, "AboutB", b);

            Color aboutColor = new Color(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG, User.Load(Context.User.Id).AboutB);

            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Sample Message")
                .WithDescription("<-- FYI, this is what you updated.")
                .WithColor(aboutColor);

            await ReplyAsync(Context.User.Mention + ", updated successfully.", false, eb);
        }

        [Command("about"), Summary("Returns the about description about the user specified.")]
        public async Task UserAbout(IUser user = null)
        {
            var userSpecified = user ?? Context.User;

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName("About " + userSpecified.Username);
            EmbedFooterBuilder efb = new EmbedFooterBuilder();
            if (User.Load(userSpecified.Id).TeamMember)
            {
                eab.WithIconUrl(User.Load(userSpecified.Id).EmbedAuthorBuilderIconUrl);

                efb.WithIconUrl(User.Load(userSpecified.Id).EmbedFooterBuilderIconUrl);
            }
            
            if (User.Load(userSpecified.Id).FooterText != null)
                efb.WithText(User.Load(userSpecified.Id).FooterText);

            Color aboutColor = new Color(User.Load(userSpecified.Id).AboutR, User.Load(userSpecified.Id).AboutG, User.Load(userSpecified.Id).AboutB);

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithFooter(efb)
                .WithThumbnailUrl(userSpecified.GetAvatarUrl())
                .WithDescription(User.Load(userSpecified.Id).About)
                .WithColor(aboutColor);

            if (User.Load(userSpecified.Id).Name != null)
                eb.AddInlineField("Name", User.Load(userSpecified.Id).Name);

            if (User.Load(userSpecified.Id).Gender != null)
                eb.AddInlineField("Gender", User.Load(userSpecified.Id).Gender);

            if (User.Load(userSpecified.Id).Pronouns != null)
                eb.AddInlineField("Pronouns", User.Load(userSpecified.Id).Pronouns);
            
            eb.AddInlineField("Coin(s)", User.Load(userSpecified.Id).Coins);

            eb.AddInlineField("Account Created", userSpecified.CreatedDate());
            eb.AddInlineField("Joined Guild", userSpecified.GuildJoinDate());

            if (User.Load(userSpecified.Id).MinecraftUsername != null)
            {
                eb.AddInlineField("Minecraft Username", User.Load(userSpecified.Id).MinecraftUsername);
            }

            if (User.Load(userSpecified.Id).SteamID != null)
            {
                string steamId = User.Load(userSpecified.Id).SteamID;
                eb.AddInlineField("Steam", "[" + steamId + "](http://steamcommunity.com/id/" + steamId + "/)");
            }

            if(User.Load(userSpecified.Id).Snapchat != null)
            {
                string snapchat = User.Load(userSpecified.Id).Snapchat;
                eb.AddInlineField("Snapchat", "[" + snapchat + "](https://www.snapchat.com/add/" + snapchat + "/)");
            }

            //if(User.Load(userSpecified.Id).GitHub != null)
            //{
            //    string githubUsername = User.Load(userSpecified.Id).GitHub;
            //    eb.AddInlineField("GitHub", "[" + githubUsername + "](https://github.com/" + githubUsername + "/)");
            //}

            if (User.Load(userSpecified.Id).CustomPrefix != null)
                eb.AddInlineField("Custom Prefix", User.Load(userSpecified.Id).CustomPrefix);

            await ReplyAsync("", false, eb);
        }

        [Command("setprefix"), Summary("Custom set the prefix for the user.")]
        public async Task CustomUserPrefix([Remainder]string prefix = null)
        {
            if (prefix == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "setprefix [prefix]\n\n`This feature will cost you: " + Configuration.Load().PrefixCost + " coins`");
                return;
            }

            if (User.Load(Context.User.Id).Coins >= Configuration.Load().PrefixCost)
            {
                User.UpdateJson(Context.User.Id, "CustomPrefix", prefix);
                User.UpdateJson(Context.User.Id, "Coins", (User.Load(Context.User.Id).Coins - Configuration.Load().PrefixCost));
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") paid " + Configuration.Load().PrefixCost + " for a custom prefix.");
                await ReplyAsync(Context.User.Mention + ", you have set `" + prefix + "` as a custom prefix for yourself. Please do take note that the following prefixes will work for you:\n```KEY: [Prefix][Command]\n" + prefix + " - User Set Prefix\n" + GuildConfiguration.Load(Context.Guild.Id).Prefix + " - Guild Set Prefix\n@" + MogiiBot3._bot.CurrentUser.Username + " - Global Prefix```");
            }
            else
            {
                await ReplyAsync("You do not have enough coins to pay for this feature, " + Context.User.Mention + "! This feature costs " + Configuration.Load().PrefixCost + " coins.");
            }

        }

        [Command("setpronouns"), Summary("Set your pronouns!")]
        public async Task SetUserPronouns([Remainder]string pronouns)
        {
            User.UpdateJson(Context.User.Id, "Pronouns", pronouns);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setmcusername"), Summary("")]
        [Alias("setminecraftusername", "mcreg", "mcregister")]
        public async Task SetMinecraftUsername([Remainder]string username)
        {
            User.UpdateJson(Context.User.Id, "MinecraftUsername", username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }
        
        [Command("setsteam"), Summary("")]
        public async Task SetSteamID([Remainder]string username)
        {
            User.UpdateJson(Context.User.Id, "SteamID", username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setsnapchat"), Summary("")]
        public async Task SetSnapchatUsername([Remainder]string username)
        {
            User.UpdateJson(Context.User.Id, "Snapchat", username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setname"), Summary("")]
        public async Task SetName([Remainder]string name)
        {
            User.UpdateJson(Context.User.Id, "Name", name);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setgender"), Summary("")]
        public async Task SetGender([Remainder]string gender)
        {
            User.UpdateJson(Context.User.Id, "Gender", gender);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("suggest"), Summary("Send your suggestion for the bot!")]
        public async Task SendSuggestion([Remainder]string message)
        {
            await GetHandler.getTextChannel(Configuration.Load().SuggestChannelID).SendMessageAsync("**Suggestion**" + "\n" +
                    Context.User.Mention + "\n" +
                    "*User Suggestion: *" + "\n" +
                    message);
        }

        [Command("support"), Summary("Sends a message out for support.")]
        public async Task SendSupportRequest([Remainder]string message = null)
        {
            if (message == null)
            {
                await GetHandler.getTextChannel(Configuration.Load().SupportChannelID).SendMessageAsync("**Support Needed**" + "\n" +
                    Context.User.Mention + " has issued the support command in <#" + Context.Channel.Id + ">\n" +
                    "*User Added Notes*" + "\n" +
                    "User has not provided any notes.");
            }
            else
            {
                await GetHandler.getTextChannel(Configuration.Load().SupportChannelID).SendMessageAsync("**Support Needed**" + "\n" +
                     Context.User.Mention + " has issued the support command in <#" + Context.Channel.Id + ">\n" +
                     "*User Added Notes*" + "\n" + message);
            }
        }
    }
}
