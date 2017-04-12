using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;

using MelissasCode;

namespace DiscordBot.Modules.Public
{
    public class FunModule : ModuleBase
    {
        Random _r = new Random();

        [Command("dice"), Summary("Rolls a 6 sided dice.")]
        [MinPermissions(PermissionLevel.User)]
        public async Task Roll6Dice()
        {
            int value = _r.Next(1, 7);
            await ReplyAsync("A 6-sided dice has been rolled, and landed on: " + value);
        }

        [Command("flipcoin"), Summary("Flips a two sided coin.")]
        [Alias("flip", "tosscoin", "coinflip")]
        [MinPermissions(PermissionLevel.User)]
        public async Task FlipCoin()
        {
            int value = _r.Next(1, 3);

            if(value == 1)
            {
                await ReplyAsync("A coin has been flipped, and landed on: Heads");
            }
            else if(value == 2)
            {
                await ReplyAsync("A coin has been flipped, and landed on: Tails");
            }
            else
            {
                await ReplyAsync("A coin had been flipped, but got lost while landing.");
            }
        }
    }
}
