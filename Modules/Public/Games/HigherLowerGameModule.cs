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
    [Group("dice")]
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class HigherLowerGameModule : ModuleBase
    {
        Random _r = new Random();

        [Command("higher")]
        public async Task DiceHigherBet(int coinsBet)
        {
            int userCoins = User.Load(Context.User.Id).Coins;
            if (coinsBet > userCoins)
            {
                await ReplyAsync("You do not have that many coin(s) to bet with, " + Context.User.Mention);
                return;
            }

            int botOne = _r.RandomNumber(1, 6), botTwo = _r.RandomNumber(1, 6), botTotal = botOne + botTwo;
            int userOne = _r.RandomNumber(1, 6), userTwo = _r.RandomNumber(1, 6), userTotal = userOne + userTwo;

            StringBuilder sb = new StringBuilder()
                .Append(MogiiBot3._bot.CurrentUser.Mention + " has rolled **" + botOne + "** and **" + botTwo + "** giving a total of **" + botTotal + "**\n")
                .Append(Context.User.Mention + " has rolled **" + userOne + "** and **" + userTwo + "** giving a total of **" + userTotal + "**\n");

            User.UpdateJson(Context.User.Id, "Coins", (userCoins - coinsBet));

            if (botTotal > userTotal)
            {
                sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and lost.");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on higher and lost.");
            }
            else if (botTotal == userTotal)
            {
                sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and didn't win or lose anything.");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on higher and didn't win nor lose.");
                User.UpdateJson(Context.User.Id, "Coins", (userCoins + coinsBet));
            }
            else
            {
                int coinsWon = (coinsBet * 2);
                sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and won **" + coinsWon + "** coin(s).");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on higher and won " + coinsWon + " coins.");
                User.UpdateJson(Context.User.Id, "Coins", (userCoins + coinsWon));
            }

            await ReplyAsync(sb.ToString());
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

            int botOne = _r.RandomNumber(1, 6), botTwo = _r.RandomNumber(1, 6), botTotal = botOne + botTwo;
            int userOne = _r.RandomNumber(1, 6), userTwo = _r.RandomNumber(1, 6), userTotal = userOne + userTwo;

            StringBuilder sb = new StringBuilder()
                .Append(MogiiBot3._bot.CurrentUser.Mention + " has rolled **" + botOne + "** and **" + botTwo + "** giving a total of **" + botTotal + "**\n")
                .Append(Context.User.Mention + " has rolled **" + userOne + "** and **" + userTwo + "** giving a total of **" + userTotal + "**\n");

            User.UpdateJson(Context.User.Id, "Coins", (userCoins - coinsBet));

            if (botTotal < userTotal)
            {
                sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and lost.");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on lower and lost.");
            }
            else if (botTotal == userTotal)
            {
                sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and didn't win or lose anything.");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on lower and didn't win nor lose.");
                User.UpdateJson(Context.User.Id, "Coins", (userCoins + coinsBet));
            }
            else
            {
                int coinsWon = (coinsBet * 2);
                sb.Append("**" + Context.User.Username + "** bet **" + coinsBet + "** coin(s) and won **" + coinsWon + "** coin(s).");
                TransactionLogger.AddTransaction(Context.User.Username + " (" + Context.User.Id + ") bet " + coinsBet + " on lower and won " + coinsWon + " coins.");
                User.UpdateJson(Context.User.Id, "Coins", (userCoins + coinsWon));
            }

            await ReplyAsync(sb.ToString());

        }
    }
}
