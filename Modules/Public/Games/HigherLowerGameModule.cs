using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Other;
using DiscordBot.Logging;

using MelissasCode;

namespace DiscordBot.Modules.Public.Games
{
    [Name("Dice Game Commands")]
    [Group("dice")]
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class HigherLowerGameModule : ModuleBase
    {
        private readonly Random _random = new Random();

        [Command("")]
        public async Task Dice()
        {
            //await ReplyAsync("**Syntax:** " +
            //                 GuildConfiguration.Load(Context.Guild.Id).Prefix + "dice [higher/lower] [coins]\n```" +
            //                 "-----------------------------\n" +
            //                 "Looking to roll a dice? " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "rolldice\n" +
            //                 "```");

            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Syntax")
                .WithDescription(GuildConfiguration.Load(Context.Guild.Id).Prefix + "dice [higher/lower] [coins]")
                .WithFooter("Looking to roll some dice? Use \"" + GuildConfiguration.Load(Context.Guild.Id).Prefix + "rolldice\"");
            await ReplyAsync("", false, eb.Build());

        }

        [Command("higher")]
        public async Task DiceHigherBet(int coinsBet)
        {
            int userCoins = User.Load(Context.User.Id).Coins;
            if (coinsBet > userCoins)
            {
                await ReplyAsync("You do not have that many coin(s) to bet with, " + Context.User.Mention);
                return;
            }

            int botOne = _random.RandomNumber(1, 6), botTwo = _random.RandomNumber(1, 6), botTotal = botOne + botTwo;
            int userOne = _random.RandomNumber(1, 6), userTwo = _random.RandomNumber(1, 6), userTotal = userOne + userTwo;

            //StringBuilder sb = new StringBuilder()
            //    .Append(MogiiBot3.Bot.CurrentUser.Mention + " has rolled **" + botOne + "** and **" + botTwo + "** giving a total of **" + botTotal + "**\n")
            //    .Append(Context.User.Mention + " has rolled **" + userOne + "** and **" + userTwo + "** giving a total of **" + userTotal + "**\n");
            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Dice Game")
                .WithDescription(Context.User.Username + " placed a bet that they would roll a higher total than " + MogiiBot3.Bot.CurrentUser.Username)
                .AddField(MogiiBot3.Bot.CurrentUser.Username + "'s Roll", MogiiBot3.Bot.CurrentUser.Mention + " rolled **" + botOne + "** and **" + botTwo + "** giving a total of **" + botTotal + "**", true)
                .AddField(Context.User.Username + "'s Roll", Context.User.Mention + " has rolled **" + userOne + "** and **" + userTwo + "** giving a total of **" + userTotal + "**", true);

            // Maybe?
            eb.WithColor(User.Load(Context.User.Id).AboutR, User.Load(Context.User.Id).AboutG,
                User.Load(Context.User.Id).AboutB);

            User.UpdateJson(Context.User.Id, "Coins", (userCoins - coinsBet));

            if (botTotal > userTotal)
            {
                //sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and lost.");
                eb.AddField(MogiiBot3.Bot.CurrentUser.Username + " Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and lost.");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on higher and lost.");
            }
            else if (botTotal == userTotal)
            {
                //sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and didn't win or lose anything.");
                eb.AddField("No-one Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and drew with " + MogiiBot3.Bot.CurrentUser.Username + ".");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on higher and didn't win nor lose.");
                User.UpdateJson(Context.User.Id, "Coins", (userCoins + coinsBet));
            }
            else
            {
                int coinsWon = (coinsBet * 2);
                //sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and won **" + coinsWon + "** coin(s).");
                eb.AddField(Context.User.Username + " Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and won " + coinsWon + ".");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on higher and won " + coinsWon + " coins.");
                User.UpdateJson(Context.User.Id, "Coins", (userCoins + coinsWon));
            }

            //await ReplyAsync(sb.ToString());
            await ReplyAsync("", false, eb.Build());
        }

        [Command("lower")]
        public async Task DiceLowerBet(int coinsBet)
        {
            int userCoins = User.Load(Context.User.Id).Coins;
            if (coinsBet > userCoins)
            {
                await ReplyAsync("You do not have that many coin(s) to bet with, " + Context.User.Mention);
                return;
            }

            int botOne = _random.RandomNumber(1, 6), botTwo = _random.RandomNumber(1, 6), botTotal = botOne + botTwo;
            int userOne = _random.RandomNumber(1, 6), userTwo = _random.RandomNumber(1, 6), userTotal = userOne + userTwo;

            //StringBuilder sb = new StringBuilder()
            //    .Append(MogiiBot3.Bot.CurrentUser.Mention + " has rolled **" + botOne + "** and **" + botTwo + "** giving a total of **" + botTotal + "**\n")
            //    .Append(Context.User.Mention + " has rolled **" + userOne + "** and **" + userTwo + "** giving a total of **" + userTotal + "**\n");
            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Dice Game")
                .WithDescription(Context.User.Username + " placed a bet that they would roll a lower total than " + MogiiBot3.Bot.CurrentUser.Username)
                .AddField(MogiiBot3.Bot.CurrentUser.Username + "'s Roll", MogiiBot3.Bot.CurrentUser.Mention + " rolled **" + botOne + "** and **" + botTwo + "** giving a total of **" + botTotal + "**")
                .AddField(Context.User.Username + "'s Roll", Context.User.Mention + " has rolled **" + userOne + "** and **" + userTwo + "** giving a total of **" + userTotal + "**");

            User.UpdateJson(Context.User.Id, "Coins", (userCoins - coinsBet));

            if (botTotal < userTotal)
            {
                //sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and lost.");
                eb.AddField(MogiiBot3.Bot.CurrentUser.Username + " Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and lost.");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on lower and lost.");
            }
            else if (botTotal == userTotal)
            {
                //sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and didn't win or lose anything.");
                eb.AddField("No-one Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and drew with " + MogiiBot3.Bot.CurrentUser.Username + ".");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on lower and didn't win nor lose.");
                User.UpdateJson(Context.User.Id, "Coins", (userCoins + coinsBet));
            }
            else
            {
                int coinsWon = (coinsBet * 2);
                //sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and won **" + coinsWon + "** coin(s).");
                eb.AddField(Context.User.Username + " Won!", Context.User.Username + " bet " + coinsBet + " coin(s) and won " + coinsWon + ".");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on lower and won " + coinsWon + " coins.");
                User.UpdateJson(Context.User.Id, "Coins", (userCoins + coinsWon));
            }

            //await ReplyAsync(sb.ToString());
            await ReplyAsync("", false, eb.Build());

        }
    }
}
