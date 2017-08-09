using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Other;
using DiscordBot.Logging;

using MelissasCode;

namespace DiscordBot.Modules.Public
{
    [Name("Fun Commands")]
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class FunModule : ModuleBase
    {
        Random _r = new Random();
        
        [Command("dice"), Summary("Rolls a x sided dice.")]
        public async Task RollDice(int numberOfDice = 1)
        {
            if (numberOfDice > 10)
            {
                await ReplyAsync("You can not roll more than 10 dice at one time, " + Context.User.Mention);
                numberOfDice = 10;
            }
            else if(numberOfDice < 1)
            {
                await ReplyAsync("You can need to roll at least 1 dice, " + Context.User.Mention);
                return;
            }

            EmbedBuilder eb = new EmbedBuilder()
                .WithDescription(Context.User.Mention + " rolled " + numberOfDice + " 6-sided dice.");
            
            int totalOfRoll = 0, roll = 0;
            for(int i = 0; i < numberOfDice; i++)
            {
                roll = _r.RandomNumber(1, 6);
                totalOfRoll += roll;

                eb.AddInlineField("Dice " + (i + 1), roll.ToString());
            }

            eb.AddField("Sum of roll", totalOfRoll);

            await ReplyAsync("", false, eb);
        }

        [Command("dice20"), Summary("Rolls a 20 sided dice.")]
        [Alias("d20")]
        public async Task Roll20Dice()
        {
            int value = _r.RandomNumber(1, 20);
            await ReplyAsync("A 20-sided dice was rolled, and landed on: " + value);
        }

        [Command("flipcoin"), Summary("Flips a two sided coin.")]
        [Alias("flip", "tosscoin", "coinflip")]
        public async Task FlipCoin()
        {
            int value = _r.RandomNumber(1, 2);

            if(value == 1)
            {
                await ReplyAsync("A coin has been flipped, and landed on: **Heads**");
            }
            else if(value == 2)
            {
                await ReplyAsync("A coin has been flipped, and landed on: **Tails**");
            }
            else
            {
                await ReplyAsync("A coin had been flipped, but got lost while landing.");
            }
        }

        [Command("approve"), Summary("Sends a picture stating \"Harold Likes This\".")]
        public async Task HaroldApproves()
        {
            await ReplyAsync("https://i.imgur.com/wHrJIfT.png");
        }

        [Command("nice"), Summary("NOICE!")]
        public async Task Noice()
        {
            await ReplyAsync("https://media.giphy.com/media/yJFeycRK2DB4c/giphy.gif");
        }

        [Command("disagree"), Summary("")]
        public async Task Disagree()
        {
            await ReplyAsync("https://i.imgur.com/3qfnX5M.png");
        }

        [Command("hehe"), Summary("Hehe!")]
        public async Task Hehe()
        {
            await ReplyAsync("https://media.giphy.com/media/9MFsKQ8A6HCN2/giphy.gif");
        }

        [Command("gitgud"), Summary("git gud")]
        public async Task Gitgud()
        {
            await ReplyAsync("https://cdn.discordapp.com/attachments/235124701964271618/310169979083423744/received_10206534636842919.jpeg");
        }

        [Command("why"), Summary("y tho")]
        public async Task WhyTho()
        {
            await ReplyAsync("https://i.imgur.com/W2l8dvp.png");
        }

        [Command("groot"), Summary("Sends a picture of baby groot dancing.")]
        public async Task BabyGroot()
        {
            await ReplyAsync("https://media.giphy.com/media/14b13BDH3V81wc/giphy.gif");
        }

        [Command("answer"), Summary("THE ANSWER TO LIFE, THE UNIVERSE AND EVERYTHING")]
        [Alias("question")]
        public async Task UltimateQuestionOfLifeTheUniverseAndEverything()
        {
            await ReplyAsync("42");
        }

        [Command("noticeme"), Summary("Will Senpai notice you?")]
        public async Task Senpai()
        {
            if (GuildConfiguration.Load(Context.Guild.Id).SenpaiEnabled)
            {
                if (_r.Next(0, 20) == _r.Next(0, 20))
                {
                    await ReplyAsync(Context.User.Mention + ", Senpai has noticed you!");
                }
                else
                {
                    await ReplyAsync(Context.User.Mention + ", Senpai has not noticed you this time around...");
                }
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", Senpai can not notice you if Senpai is in bed sleeping.");
            }
        }

        [Command("wallet"), Summary("")]
        [Alias("purse", "balance", "bal", "coins", "mogiicoins")]
        public async Task LoadUserBalance()
        {
            int userCoins = Context.User.GetUserCoins();
            await ReplyAsync(":moneybag: **" + Context.User.Username + "'s Balance** :moneybag:\n" + 
                "\n" +
                "**" + userCoins + "** coins\n");
        }

        [Command("givecoins"), Summary("Give some of coins to another user.")]
        [Alias("pay")]
        public async Task GiveCoins(IUser user = null, int coins = 0)
        {
            if(user == null || coins == 0)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "pay [@User] [Coins]\n\n" + 
                    "**Disclaimer**\n" +
                    "*There are no refunds for payments that have been made. Payments will only be refunded if a valid reason is presented. The team will discuss your situation in this case and come to a vote on whether or not your refund request will be granted.* " +
                    "*Before making the payment, please ensure that you have mentioned the correct user, and have entered the right amount of coins you wish to send. Refunds will not be made for mistyped payments!*");
                return;
            }

            if(coins <= 0)
            {
                await ReplyAsync("You need to pay a positive amount of coins.");
                return;
            }

            int issuerCoins = User.Load(Context.User.Id).Coins;
            int userCoins = User.Load(user.Id).Coins;

            if(coins > issuerCoins)
            {
                await ReplyAsync(Context.User.Mention + ", you don't have that amount of coins!");
                return;
            }

            User.UpdateJson(Context.User.Id, "Coins", (issuerCoins - coins));
            User.UpdateJson(user.Id, "Coins", userCoins + coins);
            TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") give " + user.Username + " (" + user.Id + ") " + coins + " coins.");
            await ReplyAsync(Context.User.Mention + " has given " + user.Mention + " " + coins + " coin(s)");
        }

        //int lastQuote;
        [Command("quote"), Summary("Get a random quote from the list.")]
        public async Task GenerateQuote()
        {
            if (GuildConfiguration.Load(Context.Guild.Id).QuotesEnabled)
            {
                int generatedNumber = _r.Next(0, QuoteHandler.quoteList.Count());

               // while (generatedNumber == lastQuote)
                    //generatedNumber = _r.Next(0, QuoteHandler.quoteList.Count());

                await ReplyAsync(QuoteHandler.quoteList[generatedNumber]); // Context.User.Mention + ", here's your generated quote: \n" + 

                //lastQuote = generatedNumber;
            }
            else
            {
                await ReplyAsync("Quotes are currently disabled. Try again later.");
            }
        }

        [Command("buyquote"), Summary("Request a quote to be added for a price.")]
        public async Task RequestToAddQuote([Remainder]string quote)
        {
            int userCoins = User.Load(Context.User.Id).Coins;
            int quoteCost = Configuration.Load().QuoteCost;

            if (userCoins < quoteCost)
            {
                await ReplyAsync(Context.User.Mention + ", you don't have enough coins! You need " + quoteCost + " coins to buy a quote request.");
                return;
            }

            QuoteHandler.AddAndUpdateRequestQuotes(quote);
            User.UpdateJson(Context.User.Id, "Coins", (userCoins - quoteCost));
            TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") paid " + quoteCost + " for a custom quote.");
            await ReplyAsync(Context.User.Mention + ", thank you for your quote. This costed you " + quoteCost + " coins. Your quote has been added to a wait list, and should be verified by a staff member shortly.");

            await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync("**New Quote**\nQuote requested by: **" + Context.User.Mention + "**\nQuote: " + quote);
            await GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().SendMessageAsync("**New Quote**\n" + quote + "\n\n*Do " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "listrequestquotes to view the ID and other quotes.*");
        }

        [Command("music"), Summary("Replies posting a music link which has been set by staff.")]
        public async Task FavouriteMusicLink()
        {
            int generatedNumber = _r.Next(0, MusicHandler.musicLinkList.Count());

            await ReplyAsync("Here's the music!\n" + MusicHandler.musicLinkList[generatedNumber]);
        }

        [Command("image"), Summary("Replies posting a image link which has been set by staff.")]
        public async Task PostImageLink()
        {
            int generatedNumber = _r.Next(0, ImageHandler.imageLinkList.Count());

            await ReplyAsync("Here's the image!\n" + ImageHandler.imageLinkList[generatedNumber]);
        }
    }
}
