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
    [MinPermissions(PermissionLevel.User)]
    public class FunModule : ModuleBase
    {
        Random _r = new Random();

        [Command("dice"), Summary("Rolls a 6 sided dice.")]
        public async Task Roll6Dice()
        {
            int value = _r.Next(1, 7);
            await ReplyAsync("A 6-sided dice has been rolled, and landed on: " + value);
        }

        [Command("flipcoin"), Summary("Flips a two sided coin.")]
        [Alias("flip", "tosscoin", "coinflip")]
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

        [Command("approve"), Summary("Sends a picture stating \"Harold Likes This\".")]
        public async Task HaroldApproves()
        {
            await ReplyAsync("http://i.imgur.com/AgXT80B.jpg");
        }

        [Command("nice"), Summary("NOICE!")]
        public async Task Noice()
        {
            await ReplyAsync("https://media.giphy.com/media/yJFeycRK2DB4c/giphy.gif");
        }

        [Command("noticeme"), Summary("Will Senpai notice you?")]
        public async Task Senpai()
        {
            if (Configuration.Load().SenpaiEnabled)
            {
                Random _r = new Random();
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
    }
}
