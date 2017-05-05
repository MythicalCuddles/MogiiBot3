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
using DiscordBot.Other;

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
        public async Task PlayingMessage([Remainder] string playingMessage)
        {
            Configuration.UpdateJson("Playing", playingMessage);
            await Program._bot.SetGameAsync(playingMessage);
            await Context.Message.DeleteAsync();
        }

        [Command("twitch"), Summary("Sets the twitch streaming link. Type \"none\" to disable.")]
        [Alias("streaming", "twitchstreaming")]
        public async Task SetTwitchStreamingStatus(string linkOrValue, [Remainder]string playing)
        {
            if (linkOrValue.Contains("twitch.tv"))
            {
                await Program._bot.SetGameAsync(playing, linkOrValue, StreamType.Twitch);
                await ReplyAsync(Context.User.Mention + ", my status has been updated to streaming with the Twitch.TV link of <" + linkOrValue + ">");
            }
            else
            {
                await Program._bot.SetGameAsync(playing, null, StreamType.NotStreaming);
                await ReplyAsync(Context.User.Mention + ", disabling streaming with the bot.");
            }
        }

        [Command("welcome"), Summary("Send the welcome messages to the user specified.")]
        public async Task SendWelcomeMessage(IUser user)
        {
            await GetHandler.getTextChannel(Configuration.Load().WelcomeChannelID).SendMessageAsync("Hey " + user.Mention + ", and welcome to the " + Context.Guild.Name + " Discord Server! If you are a player on our Minecraft Server, tell us your username and a Staff Member will grant you the MC Players Role \n\nIf you don't mind, could you fill out this form linked below. We are collecting data on how you found out about us, and it'd be great if we had your input. The form can be found here: <https://goo.gl/forms/iA9t5xjoZvnLJ5np1>");
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync(user.Mention + " has joined the server.");
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

        [Command("setwelcome"),Summary("Set the welcome message to the specified string. (Use `{USERJOINED}` to mention the user and `{GUILD}` to name the guild.")]
        [Alias("setwelcomemessage", "sw")]
        public async Task SetWelcomeMessage([Remainder]string message)
        {
            Configuration.UpdateJson("welcomeMessage", message);
            await ReplyAsync("Welcome message has been changed successfully by " + Context.User.Mention);
            await ReplyAsync("**SAMPLE WELCOME MESSAGE**\n" + Configuration.Load().welcomeMessage.Replace("{USERJOINED}", Context.User.Mention).Replace("{GUILDNAME}", Context.Guild.Name));
        }

        [Command("testwelcome"), Summary("Test the welcome message with mentioning a user.")]
        public async Task TestWelcomeMessage(IUser testWithUser = null)
        {
            var user = testWithUser ?? Context.User;
            await ReplyAsync(Configuration.Load().welcomeMessage.Replace("{USERJOINED}", user.Mention).Replace("{GUILDNAME}", Context.Guild.Name));
        }

        [Command("addquote"), Summary("Add a quote to the list.")]
        public async Task AddQuote([Remainder]string quote)
        {
            QuoteHandler.AddAndUpdateQuotes(quote);
            await ReplyAsync("Quote successfully added to the list, " + Context.User.Mention);
        }

        [Command("listquotes"), Summary("Sends a list of all the quotes.")]
        public async Task ListQuotes()
        {
            StringBuilder sb = new StringBuilder()
                .Append("**Quote List**\n```");

            for(int i = 0; i < QuoteHandler.quoteList.Count; i++)
            {
                sb.Append(i + ": " + QuoteHandler.quoteList[i] + "\n");
            }

            sb.Append("```");

            await ReplyAsync(sb.ToString());
        }

        [Command("deletequote"), Summary("Delete a quote from the list. Make sure to `$listquotes` to get the ID for the quote being removed!")]
        public async Task RemoveQuote(int quoteID)
        {
            string quote = QuoteHandler.quoteList[quoteID];
            QuoteHandler.RemoveAndUpdateQuotes(quoteID);
            await ReplyAsync("Quote " + quoteID + " removed successfully, " + Context.User.Mention + "\n**Quote:** " + quote);


            await ListQuotes();
        }

        [Command("addvotelink"), Summary("Add a voting link to the list.")]
        public async Task AddVoteLink([Remainder]string link)
        {
            VoteLinkHandler.AddAndUpdateLinks(link);
            await ReplyAsync("Link successfully added to the list, " + Context.User.Mention);
        }

        [Command("listvotelinks"), Summary("Sends a list of all the voting links.")]
        public async Task ListVotingLinks()
        {
            StringBuilder sb = new StringBuilder()
                .Append("**Voting Link List**\n```");

            for (int i = 0; i < VoteLinkHandler.voteLinkList.Count; i++)
            {
                sb.Append(i + ": " + VoteLinkHandler.voteLinkList[i] + "\n");
            }

            sb.Append("```");

            await ReplyAsync(sb.ToString());
        }

        [Command("deletevotelink"), Summary("Delete a voting link from the list. Make sure to `$listvotelinks` to get the ID for the link being removed!")]
        public async Task RemoveVotingLink(int linkID)
        {
            string link = VoteLinkHandler.voteLinkList[linkID];
            VoteLinkHandler.RemoveAndUpdateLinks(linkID);
            await ReplyAsync("Link " + linkID + " removed successfully, " + Context.User.Mention + "\n**Link:** " + link);

            await ListVotingLinks();
        }

        [Command("setmusic"), Summary("Sets the music link for $music.")]
        public async Task SetMusic([Remainder]string message)
        {
            Configuration.UpdateJson("musicLink", message);
            await ReplyAsync("The $music link has been updated to: " + message + " , " + Context.User.Mention);
        }
    }
}
