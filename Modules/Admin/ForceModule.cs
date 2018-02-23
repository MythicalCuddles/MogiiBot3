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

using MelissaNet;

namespace DiscordBot.Modules.Admin
{
    [Name("Force Commands")]
    [Group("force")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class ForceModule : ModuleBase
    {
        [Command(""), Summary("Get the available options for the force command.")]
        public async Task Force()
        {
            await ReplyAsync("**Syntax:** " +
                GuildConfiguration.Load(Context.Guild.Id).Prefix + "force [variable] [user mention / id] [value]\n```" +
                "Available Commands\n" +
                "------------------\n" +
                "-> force about [mention/id] [value]\n" +
                "-> force name [mention/id] [value]\n" +
                "-> force gender [mention/id] [value]\n" +
                "-> force pronouns [mention/id] [value]\n" +
                "-> force coins [mention/id] [value]\n" +
                "-> force minecraftusername [mention/id] [value]\n" +
                "-> force snapchat [mention/id] [value]\n" +
                "-> force prefix [mention/id] [value]\n" +
                "```");
        }

        [Command("about"), Summary("Force set the about message for the specified user.")]
        public async Task ForceAbout(IUser user, [Remainder]string about)
        {
            //User.UpdateJson(user.Id, "About", about);
            User.UpdateUser(user.Id, about:about);

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " changed " + user.Mention + "'s about text successfully.")
                .WithColor(Color.DarkGreen);

            await ReplyAsync("", false, eb.Build());
        }

        [Command("name"), Summary("Force set the name message for the specified user.")]
        public async Task ForceName(IUser user, [Remainder]string name)
        {
            //string oldName = User.Load(user.Id).Name;
            //User.UpdateJson(user.Id, "Name", name);
            User.UpdateUser(user.Id, name:name);
            //await ReplyAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s name message!");

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " changed " + user.Mention + "'s name text successfully.")
                .WithColor(Color.DarkGreen);

            await ReplyAsync("", false, eb.Build());
        }

        [Command("gender"), Summary("Force set the gender message for the specified user.")]
        public async Task ForceGender(IUser user, [Remainder]string gender)
        {
            //string oldGender = User.Load(user.Id).Gender;
            //User.UpdateJson(user.Id, "Gender", gender);
            User.UpdateUser(user.Id, gender:gender);
            //await ReplyAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s gender message!");

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " changed " + user.Mention + "'s gender text successfully.")
                .WithColor(Color.DarkGreen);

            await ReplyAsync("", false, eb.Build());
        }

        [Command("pronouns"), Summary("Force set the pronouns message for the specified user.")]
        public async Task ForcePronouns(IUser user, [Remainder]string pronouns)
        {
            //string oldPronouns = User.Load(user.Id).Pronouns;
            User.UpdateUser(user.Id, pronouns:pronouns);
            //await ReplyAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s pronouns message!");

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " changed " + user.Mention + "'s pronoun text successfully.")
                .WithColor(Color.DarkGreen);

            await ReplyAsync("", false, eb.Build());
        }

        [Command("coins"), Summary("Force set the coins for the specified user.")]
        public async Task ForceCoins(IUser user, int newValue)
        {
            int oldCoins = User.Load(user.Id).Coins;
            //User.UpdateJson(user.Id, "Coins", newValue);
            User.UpdateUser(user.Id, coins: newValue);
            //await ReplyAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s coins value to " + newValue + " (Was: " + oldCoins + ")");

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " changed " + user.Mention + "'s coins successfully.")
                .WithFooter("Was: " + oldCoins + " | Note: Your action has been logged!")
                .WithColor(Color.DarkGreen);

            await ReplyAsync("", false, eb.Build());
        }

        [Command("minecraftusername"), Summary("Force set the minecraft username for the specified user.")]
        public async Task ForceMinecraftUsername(IUser user, [Remainder]string username)
        {
            //string oldName = User.Load(user.Id).MinecraftUsername;
            User.UpdateUser(user.Id, minecraftUsername:username);
            //await ReplyAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s minecraft username to " + username + " (Was: " + oldName + ")");

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " changed " + user.Mention + "'s Minecraft username text successfully.")
                .WithColor(Color.DarkGreen);

            await ReplyAsync("", false, eb.Build());
        }

        [Command("snapchat"), Summary("")]
        public async Task ForceSnapchatUsername(IUser user, [Remainder]string username)
        {
            //string oldName = User.Load(user.Id).Snapchat;
            User.UpdateUser(user.Id, snapchat:username);
            //await ReplyAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s Snapchat Username to " + username + " (Was: " + oldName + ")");

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " changed " + user.Mention + "'s Snapchat username text successfully.")
                .WithColor(Color.DarkGreen);

            await ReplyAsync("", false, eb.Build());
        }

        [Command("prefix"), Summary("")]
        public async Task ForcePrefix(IUser user, [Remainder]string prefix)
        {
            //string oldPrefix = User.Load(user.Id).CustomPrefix;
            User.UpdateUser(user.Id, customPrefix:prefix);
            //await ReplyAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s Custom Prefix to " + prefix + " (Was: " + oldPrefix + ")");

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " changed " + user.Mention + "'s custom prefix successfully.")
                .WithColor(Color.DarkGreen);

            await ReplyAsync("", false, eb.Build());
        }
    }
}
