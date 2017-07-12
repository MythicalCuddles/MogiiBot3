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
using DiscordBot.Logging;

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
            if (linkOrValue.Contains("https://www.twitch.tv/"))
            {
                await MogiiBot3._bot.SetGameAsync(playing, linkOrValue, StreamType.Twitch);
                await ReplyAsync(Context.User.Mention + ", my status has been updated to streaming with the Twitch.TV link of <" + linkOrValue + ">");
            }
            else
            {
                await MogiiBot3._bot.SetGameAsync(playing, null, StreamType.NotStreaming);
            }

            await Context.Message.DeleteAsync();
        }

        [Command("welcome"), Summary("Send the welcome messages to the user specified.")]
        public async Task SendWelcomeMessage(IUser user)
        {
            await GetHandler.getTextChannel(Configuration.Load().MCWelcomeChannelID).SendMessageAsync(GuildConfiguration.Load(Context.Guild.Id).WelcomeMessage.Replace("{USERJOINED}", user.Mention).Replace("{GUILDNAME}", Context.Guild.Name));
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
            TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") awarded " + mentionedUser.Username + "(" + mentionedUser.Id + ") " + awardValue + " coins.");
        }

        [Command("finecoins"), Summary("Fine the specified user the specified amount of coins.")]
        public async Task FineCoins(IUser mentionedUser = null, int fineValue = 0)
        {
            if(mentionedUser == null || fineValue == 0)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "finecoins [@User] [Amount]");
                return;
            }

            if (fineValue <= 0)
            {
                await ReplyAsync("You can not fine that amount of coins!");
                return;
            }

            User.UpdateJson(mentionedUser.Id, "Coins", (User.Load(mentionedUser.Id).Coins - fineValue));
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync(Context.User.Mention + " has fined " + mentionedUser.Mention + " " + fineValue + " coins!");
            await ReplyAsync(mentionedUser.Mention + " has been fined " + fineValue + " coins from " + Context.User.Mention);
            TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") fined " + mentionedUser.Username + "(" + mentionedUser.Id + ") " + fineValue + " coins.");
        }

        [Command("setwelcome"),Summary("Set the welcome message to the specified string. (Use `{USERJOINED}` to mention the user and `{GUILDNAME}` to name the guild.")]
        [Alias("setwelcomemessage", "sw")]
        public async Task SetWelcomeMessage([Remainder]string message = null)
        {
            if(message == null)
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
            await ReplyAsync("Welcome message has been changed successfully by " + Context.User.Mention);
            await ReplyAsync("**SAMPLE WELCOME MESSAGE**\n" + GuildConfiguration.Load(Context.Guild.Id).WelcomeMessage.Replace("{USERJOINED}", Context.User.Mention).Replace("{GUILDNAME}", Context.Guild.Name));
        }
        
        [Command("testwelcome"), Summary("Test the welcome message with mentioning a user.")]
        public async Task TestWelcomeMessage(IUser testWithUser = null)
        {
            var user = testWithUser ?? Context.User;
            await ReplyAsync(GuildConfiguration.Load(Context.Guild.Id).WelcomeMessage.Replace("{USERJOINED}", user.Mention).Replace("{GUILDNAME}", Context.Guild.Name));
        }


        [Command("listtransactions"), Summary("Sends a list of all the transactions.")]
        public async Task ListTransactions()
        {
            StringBuilder sb = new StringBuilder()
                .Append("**Transactions**\n**----------------**\n`Total Transactions: " + TransactionLogger.transactionsList.Count + "`\n```");

            TransactionLogger.SpliceTransactionsIntoList();
            List<string> transactions = TransactionLogger.GetSplicedTransactions(1);

            for (int i = 0; i < transactions.Count; i++)
            {
                sb.Append((i + 1) + ": " + transactions[i] + "\n");
            }

            sb.Append("``` `Page 1`");

            IUserMessage msg = await ReplyAsync(sb.ToString());
            TransactionLogger.transactionMessages.Add(msg.Id);
            TransactionLogger.pageNumber.Add(1);

            if (TransactionLogger.transactionsList.Count() > 10)
                await msg.AddReactionAsync(Extensions.Extensions.arrow_right);
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
                sb.Append((i + 1) + ": " + quotes[i] + "\n");
            }

            sb.Append("```");

            IUserMessage msg = await ReplyAsync(sb.ToString());
            QuoteHandler.quoteMessages.Add(msg.Id);
            QuoteHandler.pageNumber.Add(1);

            if(QuoteHandler.quoteList.Count() > 10)
                await msg.AddReactionAsync(Extensions.Extensions.arrow_right);
        }

        [Command("editquote"), Summary("Edit a quote from the list.")]
        public async Task EditQuote(int quoteID, [Remainder]string quote)
        {
            string oldQuote = QuoteHandler.quoteList[quoteID - 1];
            QuoteHandler.UpdateQuote(quoteID - 1, quote);
            await ReplyAsync(Context.User.Mention + " updated quote id: " + quoteID + "\nOld quote: `" + oldQuote + "`\nUpdated: `" + quote + "`");
        }

        [Command("deletequote"), Summary("Delete a quote from the list. Make sure to `$listquotes` to get the ID for the quote being removed!")]
        public async Task RemoveQuote(int quoteID)
        {
            string quote = QuoteHandler.quoteList[quoteID - 1];
            QuoteHandler.RemoveAndUpdateQuotes(quoteID - 1);
            await ReplyAsync("Quote " + quoteID + " removed successfully, " + Context.User.Mention + "\n**Quote:** " + quote);
            
            await ListQuotes();
        }

        [Command("listrequestquotes"), Summary("Sends a list of all the request quotes.")]
        public async Task ListRequestQuotes()
        {
            if(QuoteHandler.requestQuoteList.Count() > 0)
            {
                StringBuilder sb = new StringBuilder()
                .Append("**Request Quote List** : *Page 1*\nTo accept a quote, type **" + GuildConfiguration.Load(Context.Guild.Id).Prefix + "acceptquote [id]**.\nTo reject a quote, type **" + GuildConfiguration.Load(Context.Guild.Id).Prefix + "denyquote [id]**.\n```");

                QuoteHandler.SpliceRequestQuotes();
                List<string> requestQuotes = QuoteHandler.getRequestQuotes(1);

                for (int i = 0; i < requestQuotes.Count; i++)
                {
                    sb.Append((i + 1) + ": " + requestQuotes[i] + "\n");
                }

                sb.Append("```");

                IUserMessage msg = await ReplyAsync(sb.ToString());
                QuoteHandler.requestQuoteMessages.Add(msg.Id);
                QuoteHandler.requestPageNumber.Add(1);

                if (QuoteHandler.requestQuoteList.Count() > 10)
                    await msg.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else
            {
                await ReplyAsync("There are currently 0 pending request quotes.");
            }
        }

        [Command("acceptquote"), Summary("Add a quote to the list.")]
        public async Task AcceptQuote(int quoteID)
        {
            string quote = QuoteHandler.requestQuoteList[quoteID - 1];
            QuoteHandler.AddAndUpdateQuotes(quote);
            QuoteHandler.RemoveAndUpdateRequestQuotes(quoteID - 1);
            await ReplyAsync(Context.User.Mention + " has accepted Quote " + quoteID + " from the request quote list.\nQuote: " + quote);
        }

        [Command("denyquote"), Summary("")]
        [Alias("rejectquote")]
        public async Task DenyQuote(int quoteID)
        {
            string quote = QuoteHandler.requestQuoteList[quoteID - 1];
            QuoteHandler.RemoveAndUpdateRequestQuotes(quoteID - 1);
            await ReplyAsync(Context.User.Mention + " has denied Quote " + quoteID + " from the request quote list.\nQuote: " + quote);
        }

        [Command("quoteprice"), Summary("")]
        [Alias("changequoteprice", "updatequoteprice")]
        public async Task ChangeQuotePrice(int price)
        {
            int oldPrice = Configuration.Load().QuoteCost;
            Configuration.UpdateJson("QuoteCost", price);
            await ReplyAsync("**" + Context.User.Mention + "** has updated the quote cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
        }

        [Command("prefixprice"), Summary("")]
        [Alias("changeprefixprice", "updateprefixprice")]
        public async Task ChangePrefixPrice(int price)
        {
            int oldPrice = Configuration.Load().PrefixCost;
            Configuration.UpdateJson("PrefixCost", price);
            await ReplyAsync("**" + Context.User.Mention + "** has updated the prefix cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
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
            if (MusicHandler.musicLinkList.Contains(link))
            {
                await ReplyAsync("\"Bitch no, it's already in the list\" - Flamesies.\nBut seriously... that link already is in the list, so you don't need to add it again.");
            }
            else
            {
                MusicHandler.AddAndUpdateLinks(link);
                await ReplyAsync("Link successfully added to the list, " + Context.User.Mention);
            }
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
