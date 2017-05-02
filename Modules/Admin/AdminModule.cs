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

namespace DiscordBot.Modules.Admin
{
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class AdminModule : ModuleBase
    {
        [Command("say"), Summary("Echos a message.")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
            await Context.Message.DeleteAsync();
        }

        [Command("togglesenpai"), Summary("Toggles the senpai command.")]
        public async Task ToggleSenpai()
        {
            Configuration.UpdateJson("SenpaiEnabled", !Configuration.Load().SenpaiEnabled);
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("Senpai has been toggled by " + Context.User.Mention + " (" + Configuration.Load().SenpaiEnabled + ")");
        }

        [Command("playing"), Summary("Changes the playing message of the bot.")]
        public async Task PlayingMessage([Remainder, Summary("Playing Message")] string playingMessage)
        {
            Configuration.UpdateJson("Playing", playingMessage);
            await Program._bot.SetGameAsync(playingMessage);
            await Context.Message.DeleteAsync();
        }

        [Command("twitch"), Summary("Sets the twitch streaming link. Type \"none\" to disable.")]
        [Alias("streaming", "twitchstreaming")]
        public async Task SetTwitchStreamingStatus(string linkOrValue)
        {
            if (linkOrValue.Contains("twitch.tv"))
            {
                await Program._bot.SetGameAsync(Configuration.Load().Playing, linkOrValue, StreamType.Twitch);
                await ReplyAsync(Context.User.Mention + ", my status has been updated to streaming with the Twitch.TV link of <" + linkOrValue + ">");
            }
            else
            {
                await Program._bot.SetGameAsync(Configuration.Load().Playing);
                await ReplyAsync(Context.User.Mention + ", disabling streaming with the bot.");
            }
        }

        [Command("welcome"), Summary("Send the welcome messages to the user specified.")]
        public async Task SendWelcomeMessage(IUser user)
        {
            await GetHandler.getTextChannel(Configuration.Load().WelcomeChannelID).SendMessageAsync("Hey " + user.Mention + ", and welcome to the " + Context.Guild.Name + " Discord Server! If you are a player on our Minecraft Server, tell us your username and a Staff Member will grant you the MC Players Role \n\nIf you don't mind, could you fill out this form linked below. We are collecting data on how you found out about us, and it'd be great if we had your input. The form can be found here: <https://goo.gl/forms/iA9t5xjoZvnLJ5np1>");
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync(user.Mention + " has joined the server.");
        }

        [Command("getcoins"), Summary("Get the coins for the specified user.")]
        [Alias("getmogiicoins")]
        public async Task GetCoins(IUser user)
        {
            var getUser = user ?? Context.User;
            await ReplyAsync(getUser.Mention + ", currently has " + User.Load(getUser.Id).Coins + " coins!");
        }

        [Command("forceabout"), Summary("Force set the about message for the specified user.")]
        public async Task ForceSetAbout(IUser user, [Remainder]string about)
        {
            string oldAbout = User.Load(user.Id).About;
            User.UpdateJson(user.Id, "About", about);
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s about message!");
        }

        [Command("forcepronouns"), Summary("Force set the pronouns message for the specified user.")]
        public async Task ForceSetPronouns(IUser user, [Remainder]string pronouns)
        {
            string oldPronouns = User.Load(user.Id).Pronouns;
            User.UpdateJson(user.Id, "Pronouns", pronouns);
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s pronouns message!");
        }

        [Command("forcecoins"), Summary("Force set the coins for the specified user.")]
        public async Task ForceSetCoins(IUser user, int newValue)
        {
            int oldCoins = User.Load(user.Id).Coins;
            User.UpdateJson(user.Id, "Coins", newValue);
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s coins value to " + newValue + " (Was: " + oldCoins + ")");
        }

        [Command("awardcoins"), Summary("Award the specified user the specified amount of coins.")]
        public async Task AwardCoins(IUser mentionedUser, int awardValue)
        {
            User.UpdateJson(mentionedUser.Id, "Coins", (User.Load(mentionedUser.Id).Coins + awardValue));
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync(Context.User.Mention + " has awarded " + mentionedUser.Mention + " " + awardValue + " coins!");
            await ReplyAsync(mentionedUser.Mention + " has been awarded " + awardValue + " coins from " + Context.User.Mention);
        }

        [Command("setwelcomemessage"),Summary("Set the welcome message to the specified string. (Use {USERJOINED} to mention the user and {GUILD} to name the guild.")]
        public async Task SetWelcomeMessage([Remainder]string message)
        {
            Configuration.UpdateJson("welcomeMessage", message);
            await ReplyAsync("Welcome message has been changed successfully by " + Context.User.Mention);
        }
    }
}
