﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Other;

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

        [Command("hehe"), Summary("Hehe!")]
        public async Task Hehe()
        {
            await ReplyAsync("https://media.giphy.com/media/9MFsKQ8A6HCN2/giphy.gif");
        }

        [Command("why"), Summary("y tho")]
        public async Task WhyTho()
        {
            await ReplyAsync("http://i.imgur.com/yNlQWRM.jpg");
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
            if (Configuration.Load().SenpaiEnabled)
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

        [Command("coins"), Summary("Returns the amount of coins you have earned")]
        [Alias("mogiicoins")]
        public async Task Coins(IUser user = null)
        {
            var mentionedUser = user ?? Context.User;

            if(mentionedUser == Context.User)
            {
                await ReplyAsync(mentionedUser.Mention + ", you have " + User.Load(mentionedUser.Id).Coins + " coins!");
            }
            else
            {
                await ReplyAsync(mentionedUser.Mention + ", currently has " + User.Load(mentionedUser.Id).Coins + " coins!");
            }
        }

        [Command("quote"), Summary("Get a random quote from the list.")]
        public async Task GenerateQuote()
        {
            int generatedNumber = _r.Next(0, QuoteHandler.quoteList.Count());

            await ReplyAsync(Context.User.Mention + ", here's your generated quote: \n" + QuoteHandler.quoteList[generatedNumber]);
        }
    }
}
