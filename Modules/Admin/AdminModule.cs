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
            await GetHandler.getTextChannel(Configuration.Load().MCLogChannelID).SendMessageAsync("Senpai has been toggled by " + Context.User.Mention + " (emabled: " + Configuration.Load().SenpaiEnabled + ")");
        }

        [Command("togglequotes"), Summary("")]
        public async Task ToggleQuotes()
        {
            Configuration.UpdateJson("QuotesEnabled", !Configuration.Load().QuotesEnabled);
            await GetHandler.getTextChannel(Configuration.Load().MCLogChannelID).SendMessageAsync("Quotes have been toggled by " + Context.User.Mention + " (emabled: " + Configuration.Load().QuotesEnabled + ")");
        }

        [Command("toggleunknowncommand"), Summary("Toggles the unknown command message.")]
        public async Task ToggleUC()
        {
            Configuration.UpdateJson("UnknownCommandEnabled", !Configuration.Load().UnknownCommandEnabled);
            await GetHandler.getTextChannel(Configuration.Load().MCLogChannelID).SendMessageAsync("UnknownCommand has been toggled by " + Context.User.Mention + " (enabled: " + Configuration.Load().UnknownCommandEnabled + ")");
        }

        [Command("playing"), Summary("Changes the playing message of the bot.")]
        public async Task PlayingMessage([Remainder] string playingMessage)
        {
            Configuration.UpdateJson("Playing", playingMessage);
            await MogiiBot3._bot.SetGameAsync(playingMessage);
            await Context.Message.DeleteAsync();
        }

        [Command("twitch"), Summary("Sets the twitch streaming link. Type \"none\" to disable.")]
        [Alias("streaming", "twitchstreaming")]
        public async Task SetTwitchStreamingStatus(string linkOrValue, [Remainder]string playing)
        {
            if (linkOrValue.Contains("twitch.tv"))
            {
                await MogiiBot3._bot.SetGameAsync(playing, linkOrValue, StreamType.Twitch);
                await ReplyAsync(Context.User.Mention + ", my status has been updated to streaming with the Twitch.TV link of <" + linkOrValue + ">");
            }
            else
            {
                await MogiiBot3._bot.SetGameAsync(playing, null, StreamType.NotStreaming);
                await ReplyAsync(Context.User.Mention + ", disabling streaming with the bot.");
            }
        }

        [Command("welcome"), Summary("Send the welcome messages to the user specified.")]
        public async Task SendWelcomeMessage(IUser user)
        {
            await GetHandler.getTextChannel(Configuration.Load().MCWelcomeChannelID).SendMessageAsync("Hey " + user.Mention + ", and welcome to the " + Context.Guild.Name + " Discord Server! If you are a player on our Minecraft Server, tell us your username and a Staff Member will grant you the MC Players Role \n\nIf you don't mind, could you fill out this form linked below. We are collecting data on how you found out about us, and it'd be great if we had your input. The form can be found here: <https://goo.gl/forms/iA9t5xjoZvnLJ5np1>");
            await GetHandler.getTextChannel(Configuration.Load().MCLogChannelID).SendMessageAsync(user.Mention + " has joined the server.");
        }
        
        [Command("awardcoins"), Summary("Award the specified user the specified amount of coins.")]
        public async Task AwardCoins(IUser mentionedUser, int awardValue)
        {
            if(awardValue <= 0)
            {
                await ReplyAsync("You can not award that amount of coins!");
                return;
            }

            User.UpdateJson(mentionedUser.Id, "Coins", (User.Load(mentionedUser.Id).Coins + awardValue));
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync(Context.User.Mention + " has awarded " + mentionedUser.Mention + " " + awardValue + " coins!");
            await ReplyAsync(mentionedUser.Mention + " has been awarded " + awardValue + " coins from " + Context.User.Mention);
        }

        [Command("finecoins"), Summary("Fine the specified user the specified amount of coins.")]
        public async Task FineCoins(IUser mentionedUser, int fineValue)
        {
            if (fineValue <= 0)
            {
                await ReplyAsync("You can not fine that amount of coins!");
                return;
            }

            User.UpdateJson(mentionedUser.Id, "Coins", (User.Load(mentionedUser.Id).Coins - fineValue));
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync(Context.User.Mention + " has fined " + mentionedUser.Mention + " " + fineValue + " coins!");
            await ReplyAsync(mentionedUser.Mention + " has been fined " + fineValue + " coins from " + Context.User.Mention);
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
                .Append("**Quote List** : *Page 1*\n```");

            QuoteHandler.SpliceQuotes();
            List<string> quotes = QuoteHandler.getQuotes(1);

            for (int i = 0; i < quotes.Count; i++)
            {
                sb.Append(i + ": " + quotes[i] + "\n");
            }

            sb.Append("```");

            IUserMessage msg = await ReplyAsync(sb.ToString());
            QuoteHandler.quoteMessages.Add(msg.Id);
            QuoteHandler.pageNumber.Add(1);
            await msg.AddReactionAsync(Extensions.Extensions.arrow_right);
        }

        [Command("editquote"), Summary("Edit a quote from the list.")]
        public async Task EditQuote(int quoteID, [Remainder]string quote)
        {
            string oldQuote = QuoteHandler.quoteList[quoteID];
            QuoteHandler.UpdateQuote(quoteID, quote);
            await ReplyAsync(Context.User.Mention + " updated quote id: " + quoteID + "\nOld quote: `" + oldQuote + "`\nUpdated: `" + quote + "`");
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

        [Command("editvotelink"), Summary("Edit a voting link from the list.")]
        public async Task EditVotingLink(int linkID, [Remainder]string link)
        {
            string oldLink = VoteLinkHandler.voteLinkList[linkID];
            VoteLinkHandler.updateLink(linkID, link);
            await ReplyAsync(Context.User.Mention + " updated vote link id: " + linkID + "\nOld link: `" + oldLink + "`\nUpdated: `" + link + "`");
        }

        [Command("deletevotelink"), Summary("Delete a voting link from the list. Make sure to `$listvotelinks` to get the ID for the link being removed!")]
        public async Task RemoveVotingLink(int linkID)
        {
            string link = VoteLinkHandler.voteLinkList[linkID];
            VoteLinkHandler.RemoveAndUpdateLinks(linkID);
            await ReplyAsync("Link " + linkID + " removed successfully, " + Context.User.Mention + "\n**Link:** " + link);

            await ListVotingLinks();
        }

        [Command("addmusic"), Summary("Add a music link to the list.")]
        public async Task AddMusicLink([Remainder]string link)
        {
            MusicHandler.AddAndUpdateLinks(link);
            await ReplyAsync("Link successfully added to the list, " + Context.User.Mention);
        }

        [Command("listmusic"), Summary("Sends a list of all the music links.")]
        public async Task ListMusicLinks()
        {
            StringBuilder sb = new StringBuilder()
                .Append("**Music Link List**\n```");

            for (int i = 0; i < MusicHandler.musicLinkList.Count; i++)
            {
                sb.Append(i + ": " + MusicHandler.musicLinkList[i] + "\n");
            }

            sb.Append("```");

            await ReplyAsync(sb.ToString());
        }

        [Command("editmusic"), Summary("Edit a music link from the list.")]
        public async Task EditMusicLink(int linkID, [Remainder]string link)
        {
            string oldLink = MusicHandler.musicLinkList[linkID];
            MusicHandler.updateLink(linkID, link);
            await ReplyAsync(Context.User.Mention + " updated music link id: " + linkID + "\nOld link: `" + oldLink + "`\nUpdated: `" + link + "`");
        }

        [Command("deletemusic"), Summary("Delete a music link from the list. Make sure to `listmusiclinks` to get the ID for the link being removed!")]
        public async Task RemoveMusicLink(int linkID)
        {
            string link = MusicHandler.musicLinkList[linkID];
            MusicHandler.RemoveAndUpdateLinks(linkID);
            await ReplyAsync("Link " + linkID + " removed successfully, " + Context.User.Mention + "\n**Link:** " + link);

            await ListMusicLinks();
        }

        [Command("mclist"), Summary("")]
        public async Task GetMinecraftUsernames()
        {
            var guild = Context.Guild as SocketGuild;
            StringBuilder sb = new StringBuilder()
                .Append("**Minecraft Username List**\n*Key (Discord Username : Minecraft Username)*\n```");

            foreach(SocketUser u in guild.Users)
            {
                string minecraftUsername = User.Load(u.Id).MinecraftUsername;
                if (minecraftUsername != null)
                {
                    sb.Append("@" + u.Username + " : " + minecraftUsername + "\n");
                }
            }

            sb.Append("```");
            await ReplyAsync(sb.ToString());
        }
    }
}
