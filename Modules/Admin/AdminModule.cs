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
using DiscordBot.Other;
using DiscordBot.Logging;

using MelissasCode;

namespace DiscordBot.Modules.Admin
{
    [Name("Admin Commands")]
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

        [Command("welcome"), Summary("Send the welcome messages to the user specified.")]
        public async Task SendWelcomeMessage(SocketGuildUser user)
        {
            await GuildConfiguration.Load(Context.Guild.Id).WelcomeChannelId.GetTextChannel().SendMessageAsync(GuildConfiguration.Load(Context.Guild.Id).WelcomeMessage.FormatWelcomeMessage(user));
            await GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("A welcome message for " + user.Mention + " has been posted. (Forced by: " + Context.User.Mention + ")");
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
            await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync(Context.User.Mention + " has awarded " + mentionedUser.Mention + " " + awardValue + " coins!");
            //await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync(Context.User.Mention + " has awarded " + mentionedUser.Mention + " " + awardValue + " coins!");
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
            await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync(Context.User.Mention + " has fined " + mentionedUser.Mention + " " + fineValue + " coins!");
            //await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync(Context.User.Mention + " has fined " + mentionedUser.Mention + " " + fineValue + " coins!");
            await ReplyAsync(mentionedUser.Mention + " has been fined " + fineValue + " coins from " + Context.User.Mention);
            TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") fined " + mentionedUser.Username + "(" + mentionedUser.Id + ") " + fineValue + " coins.");
        }
                
        [Command("listtransactions"), Summary("Sends a list of all the transactions.")]
        public async Task ListTransactions()
        {
            if(TransactionLogger.transactionsList.Count > 0)
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
            else
            {
                await ReplyAsync("No transactions were found in the database.");
            }
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
            if(QuoteHandler.quoteList.Count > 0)
            {
                StringBuilder sb = new StringBuilder()
                .Append("**Quote List** : *Page 1*\n```");

                QuoteHandler.SpliceQuotes();
                List<string> quotes = QuoteHandler.GetQuotes(1);

                for (int i = 0; i < quotes.Count; i++)
                {
                    sb.Append((i + 1) + ": " + quotes[i] + "\n");
                }

                sb.Append("```");

                IUserMessage msg = await ReplyAsync(sb.ToString());
                QuoteHandler.quoteMessages.Add(msg.Id);
                QuoteHandler.pageNumber.Add(1);

                if (QuoteHandler.quoteList.Count() > 10)
                    await msg.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else
            {
                await ReplyAsync("There are no quotes in the database.");
            }
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
                List<string> requestQuotes = QuoteHandler.GetRequestQuotes(1);

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
            VoteLinkHandler.UpdateLink(linkID, link);
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
            if(MusicHandler.musicLinkList.Count > 0)
            {
                StringBuilder sb = new StringBuilder()
                .Append("**Music Links**\n```");

                MusicHandler.SpliceMusicIntoList();
                List<string> music = MusicHandler.GetSplicedMusic(1);

                for (int i = 0; i < music.Count; i++)
                {
                    sb.Append((i + 1) + ": " + music[i] + "\n");
                }

                sb.Append("``` `Page 1`");

                IUserMessage msg = await ReplyAsync(sb.ToString());
                MusicHandler.musicMessages.Add(msg.Id);
                MusicHandler.pageNumber.Add(1);

                if (MusicHandler.musicLinkList.Count() > 10)
                    await msg.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else
            {
                await ReplyAsync("There are no music links in the database.");
            }
        }
        
        [Command("editmusic"), Summary("Edit a music link from the list.")]
        public async Task EditMusicLink(int linkID, [Remainder]string link)
        {
            string oldLink = MusicHandler.musicLinkList[linkID];
            MusicHandler.UpdateLink(linkID, link);
            await ReplyAsync(Context.User.Mention + " updated music link id: " + linkID + "\nOld link: `" + oldLink + "`\nUpdated: `" + link + "`");
        }

        [Command("deletemusic"), Summary("Delete a music link from the list. Make sure to `listmusic` to get the ID for the link being removed!")]
        public async Task RemoveMusicLink(int linkID)
        {
            string link = MusicHandler.musicLinkList[linkID];
            MusicHandler.RemoveAndUpdateLinks(linkID);
            await ReplyAsync("Link " + linkID + " removed successfully, " + Context.User.Mention + "\n**Link:** " + link);

            await ListMusicLinks();
        }
        
        [Command("addimage"), Summary("Add a image link to the list.")]
        public async Task AddImageLink([Remainder]string link)
        {
            if (ImageHandler.imageLinkList.Contains(link))
            {
                await ReplyAsync("\"Bitch no, it's already in the list\" - Flamesies.\nBut seriously... that link already is in the list, so you don't need to add it again.");
            }
            else
            {
                ImageHandler.AddAndUpdateLinks(link);
                await ReplyAsync("Link successfully added to the list, " + Context.User.Mention);
            }
        }

        [Command("listimages"), Summary("Sends a list of all the image links.")]
        public async Task ListImageLinks()
        {
            if (ImageHandler.imageLinkList.Count > 0)
            {
                StringBuilder sb = new StringBuilder()
                .Append("**Image Links**\n```");

                ImageHandler.SpliceIntoList();
                List<string> images = ImageHandler.GetSplicedList(1);

                for (int i = 0; i < images.Count; i++)
                {
                    sb.Append((i + 1) + ": " + images[i] + "\n");
                }

                sb.Append("``` `Page 1`");

                IUserMessage msg = await ReplyAsync(sb.ToString());
                ImageHandler.imageMessages.Add(msg.Id);
                ImageHandler.pageNumber.Add(1);

                if (ImageHandler.imageLinkList.Count() > 10)
                    await msg.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else
            {
                await ReplyAsync("There are no music links in the database.");
            }
        }

        [Command("editimage"), Summary("Edit a image link from the list.")]
        public async Task EditImageLink(int linkID, [Remainder]string link)
        {
            string oldLink = ImageHandler.imageLinkList[linkID];
            ImageHandler.UpdateLink(linkID, link);
            await ReplyAsync(Context.User.Mention + " updated image link id: " + linkID + "\nOld link: `" + oldLink + "`\nUpdated: `" + link + "`");
        }

        [Command("deleteimage"), Summary("Delete a image link from the list. Make sure to `listimages` to get the ID for the link being removed!")]
        public async Task RemoveImageLink(int linkID)
        {
            string link = ImageHandler.imageLinkList[linkID];
            ImageHandler.RemoveAndUpdateLinks(linkID);
            await ReplyAsync("Link " + linkID + " removed successfully, " + Context.User.Mention + "\n**Link:** " + link);

            await ListImageLinks();
        }
    }
}
