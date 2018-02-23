﻿using System;
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

using MelissaNet;

namespace DiscordBot.Modules.Admin
{
    [Name("Admin Commands")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class AdminModule : ModuleBase
    {
        [Command("echo"), Summary("Echos a message.")]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
            await Context.Message.DeleteAsync();
        }

        [Command("welcome"), Summary("Send the welcome messages to the user specified.")]
        public async Task SendWelcomeMessage(SocketGuildUser user)
        {
            await GuildConfiguration.Load(Context.Guild.Id).WelcomeChannelId.GetTextChannel().SendMessageAsync(GuildConfiguration.Load(Context.Guild.Id).WelcomeMessage.ModifyStringFlags(user));
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
            
            User.UpdateUser(mentionedUser.Id, coins: (mentionedUser.GetCoins()  + awardValue));
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has awarded " + mentionedUser.Mention + " " + awardValue + " coins!");
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
            
            User.UpdateUser(mentionedUser.Id, coins: (User.Load(mentionedUser.Id).Coins - fineValue));
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has fined " + mentionedUser.Mention + " " + fineValue + " coins!");
            await ReplyAsync(mentionedUser.Mention + " has been fined " + fineValue + " coins from " + Context.User.Mention);
            TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") fined " + mentionedUser.Username + "(" + mentionedUser.Id + ") " + fineValue + " coins.");
        }
                
        [Command("listtransactions"), Summary("Sends a list of all the transactions.")]
        public async Task ListTransactions()
        {
            if(TransactionLogger.TransactionsList.Count > 0)
            {
                StringBuilder sb = new StringBuilder()
                .Append("**Transactions**\n**----------------**\n`Total Transactions: " + TransactionLogger.TransactionsList.Count + "`\n```");

                TransactionLogger.SpliceTransactionsIntoList();
                List<string> transactions = TransactionLogger.GetSplicedTransactions(1);

                for (int i = 0; i < transactions.Count; i++)
                {
                    sb.Append((i + 1) + ": " + transactions[i] + "\n");
                }

                sb.Append("``` `Page 1`");

                IUserMessage msg = await ReplyAsync(sb.ToString());
                TransactionLogger.TransactionMessages.Add(msg.Id);
                TransactionLogger.PageNumber.Add(1);

                if (TransactionLogger.TransactionsList.Count() > 10)
                    await msg.AddReactionAsync(Extensions.Extensions.ArrowRight);
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
            //await ReplyAsync("Quote successfully added to the list, " + Context.User.Mention);
			
			EmbedBuilder eb = new EmbedBuilder()
				.WithDescription(Context.User.Mention + " Quote Added")
				.WithColor(33, 210, 47);

			await ReplyAsync("", false, eb.Build());
        }

        [Command("listquotes"), Summary("Sends a list of all the quotes.")]
        public async Task ListQuotes()
        {
            if (QuoteHandler.QuoteList.Count > 0)
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
                QuoteHandler.QuoteMessages.Add(msg.Id);
                QuoteHandler.PageNumber.Add(1);

                if (QuoteHandler.QuoteList.Count() > 10)
                    await msg.AddReactionAsync(Extensions.Extensions.ArrowRight);
            }
            else
            {
                await ReplyAsync("There are no quotes in the database.");
            }
        }

        [Command("editquote"), Summary("Edit a quote from the list.")]
        public async Task EditQuote(int quoteId, [Remainder]string quote)
        {
            string oldQuote = QuoteHandler.QuoteList[quoteId - 1];
            QuoteHandler.UpdateQuote(quoteId - 1, quote);
            await ReplyAsync(Context.User.Mention + " updated quote id: " + quoteId + "\nOld quote: `" + oldQuote + "`\nUpdated: `" + quote + "`");
        }

        [Command("deletequote"), Summary("Delete a quote from the list. Make sure to `$listquotes` to get the ID for the quote being removed!")]
        public async Task RemoveQuote(int quoteId)
        {
			if(quoteId <= QuoteHandler.QuoteList.Count())
			{
				string quote = QuoteHandler.QuoteList[quoteId - 1];
				QuoteHandler.RemoveAndUpdateQuotes(quoteId - 1);
				//await ReplyAsync("Quote " + quoteID + " removed successfully, " + Context.User.Mention + "\n**Quote:** " + quote);

				EmbedBuilder eb = new EmbedBuilder()
					.WithDescription(Context.User.Mention + " Quote Removed\nQuote: " + quote)
					.WithColor(210, 47, 33);

				await ReplyAsync("", false, eb.Build());
			} 
			else
			{
				EmbedBuilder eb = new EmbedBuilder()
					.WithDescription(Context.User.Mention + " There is no quote with that Id")
					.WithColor(47, 33, 210);

				await ReplyAsync("", false, eb.Build());
			}
        }

        [Command("listrequestquotes"), Summary("Sends a list of all the request quotes.")]
        public async Task ListRequestQuotes()
        {
            if(QuoteHandler.RequestQuoteList.Count() > 0)
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
                QuoteHandler.RequestQuoteMessages.Add(msg.Id);
                QuoteHandler.RequestPageNumber.Add(1);

                if (QuoteHandler.RequestQuoteList.Count() > 10)
                    await msg.AddReactionAsync(Extensions.Extensions.ArrowRight);
            }
            else
            {
                await ReplyAsync("There are currently 0 pending request quotes.");
            }
        }

        [Command("acceptquote"), Summary("Add a quote to the list.")]
        public async Task AcceptQuote(int quoteId)
        {
            string quote = QuoteHandler.RequestQuoteList[quoteId - 1];
            QuoteHandler.AddAndUpdateQuotes(quote);
            QuoteHandler.RemoveAndUpdateRequestQuotes(quoteId - 1);
            await ReplyAsync(Context.User.Mention + " has accepted Quote " + quoteId + " from the request quote list.\nQuote: " + quote);
        }

        [Command("denyquote"), Summary("")]
        [Alias("rejectquote")]
        public async Task DenyQuote(int quoteId)
        {
            string quote = QuoteHandler.RequestQuoteList[quoteId - 1];
            QuoteHandler.RemoveAndUpdateRequestQuotes(quoteId - 1);
            await ReplyAsync(Context.User.Mention + " has denied Quote " + quoteId + " from the request quote list.\nQuote: " + quote);
        }

        [Command("addvotelink"), Summary("Add a voting link to the list.")]
        public async Task AddVoteLink([Remainder]string link)
        {
            VoteLinkHandler.AddAndUpdateLinks(link);

			EmbedBuilder eb = new EmbedBuilder()
				.WithDescription(Context.User.Mention + " Link Added")
				.WithColor(33, 210, 47);

			await ReplyAsync("", false, eb.Build());

			//await ReplyAsync("Link successfully added to the list, " + Context.User.Mention);
        }

        [Command("listvotelinks"), Summary("Sends a list of all the voting links.")]
        public async Task ListVotingLinks()
        {
            StringBuilder sb = new StringBuilder()
                .Append("**Voting Link List**\n```");

            for (int i = 0; i < VoteLinkHandler.VoteLinkList.Count; i++)
            {
                sb.Append(i + ": " + VoteLinkHandler.VoteLinkList[i] + "\n");
            }

            sb.Append("```");

            await ReplyAsync(sb.ToString());
        }

        [Command("editvotelink"), Summary("Edit a voting link from the list.")]
        public async Task EditVotingLink(int linkId, [Remainder]string link)
        {
            string oldLink = VoteLinkHandler.VoteLinkList[linkId];
            VoteLinkHandler.UpdateLink(linkId, link);
            await ReplyAsync(Context.User.Mention + " updated vote link id: " + linkId + "\nOld link: `" + oldLink + "`\nUpdated: `" + link + "`");
        }

        [Command("deletevotelink"), Summary("Delete a voting link from the list. Make sure to `$listvotelinks` to get the ID for the link being removed!")]
        public async Task RemoveVotingLink(int linkId)
        {
            string link = VoteLinkHandler.VoteLinkList[linkId];
            VoteLinkHandler.RemoveAndUpdateLinks(linkId);

			EmbedBuilder eb = new EmbedBuilder()
					.WithDescription(Context.User.Mention + " Link Removed\nLink: " + link)
					.WithColor(210, 47, 33);

			await ReplyAsync("", false, eb.Build());

			//await ReplyAsync("Link " + linkID + " removed successfully, " + Context.User.Mention + "\n**Link:** " + link);

			await ListVotingLinks();
        }
        
        [Command("addimage"), Summary("Add a image link to the list.")]
        public async Task AddImageLink([Remainder]string link)
        {
            if (ImageHandler.ImageLinkList.Contains(link))
            {
                await ReplyAsync("\"Bitch no, it's already in the list\" - Flamesies.\nBut seriously... that link already is in the list, so you don't need to add it again.");
            }
            else
            {
                ImageHandler.AddAndUpdateLinks(link);

				EmbedBuilder eb = new EmbedBuilder()
				.WithDescription(Context.User.Mention + " Link Added")
				.WithColor(33, 210, 47);

				await ReplyAsync("", false, eb.Build());

				//await ReplyAsync("Link successfully added to the list, " + Context.User.Mention);
            }
        }

        [Command("listimages"), Summary("Sends a list of all the image links.")]
        public async Task ListImageLinks()
        {
            if (ImageHandler.ImageLinkList.Count > 0)
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
                ImageHandler.ImageMessages.Add(msg.Id);
                ImageHandler.PageNumber.Add(1);

                if (ImageHandler.ImageLinkList.Count() > 10)
                    await msg.AddReactionAsync(Extensions.Extensions.ArrowRight);
            }
            else
            {
                await ReplyAsync("There are no image links in the database.");
            }
        }

        [Command("editimage"), Summary("Edit a image link from the list.")]
        public async Task EditImageLink(int linkId, [Remainder]string link)
        {
            string oldLink = ImageHandler.ImageLinkList[linkId];
            ImageHandler.UpdateLink(linkId, link);
            await ReplyAsync(Context.User.Mention + " updated image link id: " + linkId + "\nOld link: `" + oldLink + "`\nUpdated: `" + link + "`");
        }

        [Command("deleteimage"), Summary("Delete a image link from the list. Make sure to `listimages` to get the ID for the link being removed!")]
        public async Task RemoveImageLink(int linkId)
        {
            string link = ImageHandler.ImageLinkList[linkId];
            ImageHandler.RemoveAndUpdateLinks(linkId);

			EmbedBuilder eb = new EmbedBuilder()
					.WithDescription(Context.User.Mention + " Link Removed\nLink: " + link)
					.WithColor(210, 47, 33);

			await ReplyAsync("", false, eb.Build());

			//await ReplyAsync("Link " + linkID + " removed successfully, " + Context.User.Mention + "\n**Link:** " + link);

			await ListImageLinks();
        }
    }
}
