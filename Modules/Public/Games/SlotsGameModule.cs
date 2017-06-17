﻿using System;
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
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class SlotsGameModule : ModuleBase
    {
        Random _r = new Random();

        [Command("buyslots"), Summary("")]
        [Alias("slots")]
        public async Task PlaySlots(int inputCoins)
        {
            if (inputCoins < 1)
            {
                await ReplyAsync("You can not play with " + inputCoins + " coin(s)!");
                return;
            }
            else if (inputCoins > User.Load(Context.User.Id).Coins)
            {
                await ReplyAsync(Context.User.Mention + ", you don't have enough coin(s) to play with.");
                return;
            }

            int slotEmotesCount = Extensions.Extensions.SlotEmotes.Count();
            int one = _r.Next(0, slotEmotesCount), two = _r.Next(0, slotEmotesCount), three = _r.Next(0, slotEmotesCount);

            StringBuilder sb = new StringBuilder()
                .Append("**[  :slot_machine: l SLOTS ]**\n")
                .Append("------------------\n")
                .Append(Extensions.Extensions.SlotEmotes[_r.Next(0, slotEmotesCount)] + " : " + Extensions.Extensions.SlotEmotes[_r.Next(0, slotEmotesCount)] + " : " + Extensions.Extensions.SlotEmotes[_r.Next(0, slotEmotesCount)] + "\n")
                .Append(Extensions.Extensions.SlotEmotes[one] + " : " + Extensions.Extensions.SlotEmotes[two] + " : " + Extensions.Extensions.SlotEmotes[three] + " < \n")
                .Append(Extensions.Extensions.SlotEmotes[_r.Next(0, slotEmotesCount)] + " : " + Extensions.Extensions.SlotEmotes[_r.Next(0, slotEmotesCount)] + " : " + Extensions.Extensions.SlotEmotes[_r.Next(0, slotEmotesCount)] + "\n")
                .Append("------------------\n");

            User.UpdateJson(Context.User.Id, "Coins", (User.Load(Context.User.Id).Coins - inputCoins));

            if (one == two || two == three || one == three)
            {
                sb.Append("| : : : : **WIN** : : : : |\n\n");
                int coinsWon = (inputCoins * 2) + inputCoins;
                sb.Append("**" + Context.User.Username + "** bet **" + inputCoins + "** coin(s) and won **" + coinsWon + "** coin(s).");

                User.UpdateJson(Context.User.Id, "Coins", (User.Load(Context.User.Id).Coins + coinsWon));
            }
            else
            {
                sb.Append("| : : :  **LOST**  : : : |\n\n");
                sb.Append("**" + Context.User.Username + "** bet **" + inputCoins + "** coin(s) and lost.");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}