using System;
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
    [MinPermissions(PermissionLevel.BotOwner)] // CHANGE
    [RequireContext(ContextType.Guild)]
    public class GambleModule : ModuleBase
    {
        [Command("buychips"), Summary("")]
        public async Task BuyChips(int chipAmount)
        {
            int userCoins = User.Load(Context.User.Id).Coins;
            int chipCost = chipAmount * Configuration.Load().CoinToChipRatio;

            if(chipCost > userCoins)
            {
                await ReplyAsync(Context.User.Mention + ", you don't have enough coins to buy " + chipAmount + " chips.");
                return;
            }

            User.UpdateJson(Context.User.Id, "Coins", (userCoins - chipCost));
            User.UpdateJson(Context.User.Id, "Chips", (User.Load(Context.User.Id).Chips + chipAmount));

            await ReplyAsync(Context.User.Mention + ", thank you for purchasing " + chipAmount + " chip(s). You have been charged " + chipCost + " coins.");
        }
    }
}
