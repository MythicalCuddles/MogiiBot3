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
        public async Task DiceHigherBet(int chipsBet)
        {
            int userChips = User.Load(Context.User.Id).Chips;
            if (chipsBet > userChips)
            {
                await ReplyAsync("You do not have that many chips to bet with, " + Context.User.Mention);
                return;
            }

            int botOne = _r.RandomNumber(1, 6), botTwo = _r.RandomNumber(1, 6), botTotal = botOne + botTwo;
            int userOne = _r.RandomNumber(1, 6), userTwo = _r.RandomNumber(1, 6), userTotal = userOne + userTwo;

            StringBuilder sb = new StringBuilder()
                .Append(MogiiBot3._bot.CurrentUser.Mention + " has rolled **" + botOne + "** and **" + botTwo + "** giving a total of **" + botTotal + "**\n")
                .Append(Context.User.Mention + " has rolled **" + userOne + "** and **" + userTwo + "** giving a total of **" + userTotal + "**\n");

            User.UpdateJson(Context.User.Id, "Chips", (userChips - chipsBet));

            if (botTotal > userTotal)
            {
                sb.Append("**" + Context.User.Username + "** bet **" + chipsBet + "** chip(s) and lost.");
            }
            else if (botTotal == userTotal)
            {
                sb.Append("**" + Context.User.Username + "** bet **" + chipsBet + "** chip(s) and didn't win or lose anything.");
                User.UpdateJson(Context.User.Id, "Chips", (userChips + chipsBet));
            }
            else
            {
                int chipsWon = (chipsBet * 2);
                sb.Append("**" + Context.User.Username + "** bet **" + chipsBet + "** chip(s) and won **" + chipsWon + "** chip(s).");
                User.UpdateJson(Context.User.Id, "Chips", (userChips + chipsWon));
            }

            await ReplyAsync(sb.ToString());
        }

        [Command("lower")]
        public async Task DiceLowerBet(int chipsBet)
        {
            int userChips = User.Load(Context.User.Id).Chips;
            if (chipsBet > userChips)
            {
                await ReplyAsync("You do not have that many chips to bet with, " + Context.User.Mention);
                return;
            }

            int botOne = _r.RandomNumber(1, 6), botTwo = _r.RandomNumber(1, 6), botTotal = botOne + botTwo;
            int userOne = _r.RandomNumber(1, 6), userTwo = _r.RandomNumber(1, 6), userTotal = userOne + userTwo;

            StringBuilder sb = new StringBuilder()
                .Append(MogiiBot3._bot.CurrentUser.Mention + " has rolled **" + botOne + "** and **" + botTwo + "** giving a total of **" + botTotal + "**\n")
                .Append(Context.User.Mention + " has rolled **" + userOne + "** and **" + userTwo + "** giving a total of **" + userTotal + "**\n");

            User.UpdateJson(Context.User.Id, "Chips", (userChips - chipsBet));

            if (botTotal < userTotal)
            {
                sb.Append("**" + Context.User.Username + "** bet **" + chipsBet + "** chip(s) and lost.");
            }
            else if (botTotal == userTotal)
            {
                sb.Append("**" + Context.User.Username + "** bet **" + chipsBet + "** chip(s) and didn't win or lose anything.");
                User.UpdateJson(Context.User.Id, "Chips", (userChips + chipsBet));
            }
            else
            {
                int chipsWon = (chipsBet * 2);
                sb.Append("**" + Context.User.Username + "** bet **" + chipsBet + "** chip(s) and won **" + chipsWon + "** chip(s).");
                User.UpdateJson(Context.User.Id, "Chips", (userChips + chipsWon));
            }

            await ReplyAsync(sb.ToString());

        }
    }
}
