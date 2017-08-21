using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using Discord.WebSocket;

using DiscordBot;
using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Other;
using DiscordBot.Logging;

using MelissasCode;

namespace DiscordBot.Handlers
{
    public class ReactionHandler
    {
        public static async Task ReactionAdded(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.User.Value.IsBot)
                return;

            if (QuoteHandler.quoteMessages.Contains(message.Id))
            {
                await HandleQuoteReactions(message, channel, reaction);
                return;
            }

            if (QuoteHandler.requestQuoteMessages.Contains(message.Id))
            {
                await HandleRequestQuoteReactions(message, channel, reaction);
                return;
            }

            if (TransactionLogger.transactionMessages.Contains(message.Id))
            {
                await HandleTransactionReactions(message, channel, reaction);
                return;
            }

            if (MusicHandler.musicMessages.Contains(message.Id))
            {
                await HandleMusicReactions(message, channel, reaction);
                return;
            }

            if (ImageHandler.imageMessages.Contains(message.Id))
            {
                await HandleImageReactions(message, channel, reaction);
                return;
            }

            // Coin System to add a coin for each reaction that is added to a message.
            if (Configuration.Load().CoinsForReactions)
            {
                await MogiiBot3.AwardCoinsToPlayer(reaction.User.Value);
            }
        }

        private static async Task HandleQuoteReactions(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            // Check to see if the next page or previous page was clicked.
            if (reaction.Emote.Name == Extensions.Extensions.arrow_left.Name)
            {
                if (QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] == 1)
                    return;

                QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)]--;
            }
            else if (reaction.Emote.Name == Extensions.Extensions.arrow_right.Name)
            {
                if (QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] == QuoteHandler.GetQuotesListLength)
                    return;

                QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)]++;
            }

            StringBuilder sb = new StringBuilder()
            .Append("**Quote List** : *Page " + QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] + "*\n```");

            List<string> quotes = QuoteHandler.GetQuotes(QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)]);

            for (int i = 0; i < quotes.Count; i++)
            {
                sb.Append(((i + 1) + ((QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + quotes[i] + "\n");
            }

            sb.Append("```");

            await message.Value.ModifyAsync(msg => msg.Content = sb.ToString());
            await message.Value.RemoveAllReactionsAsync();

            if (QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] == 1)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else if (QuoteHandler.pageNumber[QuoteHandler.quoteMessages.IndexOf(message.Id)] == QuoteHandler.GetQuotesListLength)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
            }
            else
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
        }
        private static async Task HandleRequestQuoteReactions(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            // Check to see if the next page or previous page was clicked.
            if (reaction.Emote.Name == Extensions.Extensions.arrow_left.Name)
            {
                if (QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] == 1)
                    return;

                QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)]--;
            }
            else if (reaction.Emote.Name == Extensions.Extensions.arrow_right.Name)
            {
                if (QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] == QuoteHandler.GetRequestQuotesListLength)
                    return;

                QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)]++;
            }

            StringBuilder sb = new StringBuilder()
            .Append("**Request Quote List** : *Page " + QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] + "*\nTo accept a quote, type **" + GuildConfiguration.Load(channel.GetGuild().Id).Prefix + "acceptquote[id]**.\nTo reject a quote, type **" + GuildConfiguration.Load(channel.GetGuild().Id).Prefix + "denyquote[id]**.\n```");

            List<string> requestQuotes = QuoteHandler.GetRequestQuotes(QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)]);

            for (int i = 0; i < requestQuotes.Count; i++)
            {
                sb.Append(((i + 1) + ((QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + requestQuotes[i] + "\n");
            }

            sb.Append("```");

            await message.Value.ModifyAsync(msg => msg.Content = sb.ToString());
            await message.Value.RemoveAllReactionsAsync();

            if (QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] == 1)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else if (QuoteHandler.requestPageNumber[QuoteHandler.requestQuoteMessages.IndexOf(message.Id)] == QuoteHandler.GetRequestQuotesListLength)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
            }
            else
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
        }
        private static async Task HandleTransactionReactions(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            // Check to see if the next page or previous page was clicked.
            if (reaction.Emote.Name == Extensions.Extensions.arrow_left.Name)
            {
                if (TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] == 1)
                    return;

                TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)]--;
            }
            else if (reaction.Emote.Name == Extensions.Extensions.arrow_right.Name)
            {
                if (TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] == TransactionLogger.GetSplicedTransactonListCount)
                    return;

                TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)]++;
            }

            StringBuilder sb = new StringBuilder()
                .Append("**Transactions**\n**----------------**\n`Total Transactions: " + TransactionLogger.transactionsList.Count + "`\n```");

            List<string> transactions = TransactionLogger.GetSplicedTransactions(TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)]);

            for (int i = 0; i < transactions.Count; i++)
            {
                sb.Append(((i + 1) + ((TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + transactions[i] + "\n");
            }

            sb.Append("``` `Page " + TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] + "`");

            await message.Value.ModifyAsync(msg => msg.Content = sb.ToString());
            await message.Value.RemoveAllReactionsAsync();

            if (TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] == 1)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else if (TransactionLogger.pageNumber[TransactionLogger.transactionMessages.IndexOf(message.Id)] == TransactionLogger.GetSplicedTransactonListCount)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
            }
            else
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
        }
        private static async Task HandleMusicReactions(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            // Check to see if the next page or previous page was clicked.
            if (reaction.Emote.Name == Extensions.Extensions.arrow_left.Name)
            {
                if (MusicHandler.pageNumber[MusicHandler.musicMessages.IndexOf(message.Id)] == 1)
                    return;

                MusicHandler.pageNumber[MusicHandler.musicMessages.IndexOf(message.Id)]--;
            }
            else if (reaction.Emote.Name == Extensions.Extensions.arrow_right.Name)
            {
                if (MusicHandler.pageNumber[MusicHandler.musicMessages.IndexOf(message.Id)] == MusicHandler.GetSplicedMusicListCount)
                    return;

                MusicHandler.pageNumber[MusicHandler.musicMessages.IndexOf(message.Id)]++;
            }

            StringBuilder sb = new StringBuilder()
                .Append("**Music Links**\n```");

            List<string> musicLinks = MusicHandler.GetSplicedMusic(MusicHandler.pageNumber[MusicHandler.musicMessages.IndexOf(message.Id)]);

            for (int i = 0; i < musicLinks.Count; i++)
            {
                sb.Append(((i + 1) + ((MusicHandler.pageNumber[MusicHandler.musicMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + musicLinks[i] + "\n");
            }

            sb.Append("``` `Page " + MusicHandler.pageNumber[MusicHandler.musicMessages.IndexOf(message.Id)] + "`");

            await message.Value.ModifyAsync(msg => msg.Content = sb.ToString());
            await message.Value.RemoveAllReactionsAsync();

            if (MusicHandler.pageNumber[MusicHandler.musicMessages.IndexOf(message.Id)] == 1)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else if (MusicHandler.pageNumber[MusicHandler.musicMessages.IndexOf(message.Id)] == MusicHandler.GetSplicedMusicListCount)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
            }
            else
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
        }
        private static async Task HandleImageReactions(Cacheable<IUserMessage, ulong> message, ISocketMessageChannel channel, SocketReaction reaction)
        {
            // Check to see if the next page or previous page was clicked.
            if (reaction.Emote.Name == Extensions.Extensions.arrow_left.Name)
            {
                if (ImageHandler.pageNumber[ImageHandler.imageMessages.IndexOf(message.Id)] == 1)
                    return;

                ImageHandler.pageNumber[ImageHandler.imageMessages.IndexOf(message.Id)]--;
            }
            else if (reaction.Emote.Name == Extensions.Extensions.arrow_right.Name)
            {
                if (ImageHandler.pageNumber[ImageHandler.imageMessages.IndexOf(message.Id)] == ImageHandler.GetSplicedListCount)
                    return;

                ImageHandler.pageNumber[ImageHandler.imageMessages.IndexOf(message.Id)]++;
            }

            StringBuilder sb = new StringBuilder()
                .Append("**Image Links**\n```");

            List<string> imageLinks = ImageHandler.GetSplicedList(ImageHandler.pageNumber[ImageHandler.imageMessages.IndexOf(message.Id)]);

            for (int i = 0; i < imageLinks.Count; i++)
            {
                sb.Append(((i + 1) + ((ImageHandler.pageNumber[ImageHandler.imageMessages.IndexOf(message.Id)] - 1) * 10)) + ": " + imageLinks[i] + "\n");
            }

            sb.Append("``` `Page " + ImageHandler.pageNumber[ImageHandler.imageMessages.IndexOf(message.Id)] + "`");

            await message.Value.ModifyAsync(msg => msg.Content = sb.ToString());
            await message.Value.RemoveAllReactionsAsync();

            if (ImageHandler.pageNumber[ImageHandler.imageMessages.IndexOf(message.Id)] == 1)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
            else if (ImageHandler.pageNumber[ImageHandler.imageMessages.IndexOf(message.Id)] == ImageHandler.GetSplicedListCount)
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
            }
            else
            {
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_left);
                await message.Value.AddReactionAsync(Extensions.Extensions.arrow_right);
            }
        }
    }
}
