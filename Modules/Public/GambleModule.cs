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

namespace DiscordBot.Modules.Public
{
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class GambleModule : ModuleBase
    {
        Random _r = new Random();

        [Command("buychips"), Summary("")]
        public async Task BuyChips(int chipAmount)
        {
            if(chipAmount < 1)
            {
                await ReplyAsync("You can not buy " + chipAmount + " chips!");
                return;
            }

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

        [Command("sellchips"), Summary("")]
        public async Task SellChips(int chipAmount)
        {
            int userChips = User.Load(Context.User.Id).Chips;
            if (chipAmount < 1)
            {
                await ReplyAsync("You can not sell " + chipAmount + " chip(s)!");
                return;
            }

            if (chipAmount > userChips)
            {
                await ReplyAsync(Context.User.Mention + ", you don't have enough chip(s) to sell " + chipAmount + " chip(s).");
                return;
            }

            int userCoins = User.Load(Context.User.Id).Coins;
            int coinsForChips = chipAmount * Configuration.Load().ChipToCoinRatio;

            User.UpdateJson(Context.User.Id, "Coins", (userCoins + coinsForChips));
            User.UpdateJson(Context.User.Id, "Chips", (User.Load(Context.User.Id).Chips - chipAmount));

            await ReplyAsync(Context.User.Mention + ", thank you for selling " + chipAmount + " chip(s). You have been given " + coinsForChips + " coin(s).");
        }

        [Command("buyslots"), Summary("")]
        public async Task PlaySlots(int inputChips)
        {
            if (inputChips < 1)
            {
                await ReplyAsync("You can not play with " + inputChips + " chips!");
                return;
            }
            else if(inputChips > User.Load(Context.User.Id).Chips)
            {
                await ReplyAsync(Context.User.Mention + ", you don't have enough chips to play with.");
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

            if(one == two || two == three || one == three)
            {
                sb.Append("| : : : : **WIN** : : : : |\n\n");
                int chipsWon = (inputChips * 2) + inputChips;
                sb.Append("**" + Context.User.Username + "** used **" + inputChips + "** chip(s) and won **" + chipsWon + "** chip(s).");

                User.UpdateJson(Context.User.Id, "Chips", (User.Load(Context.User.Id).Chips + chipsWon));
            }
            else
            {
                sb.Append("| : : :  **LOST**  : : : |\n\n");
                sb.Append("**" + Context.User.Username + "** used **" + inputChips + "** chip(s) and lost everything.");
                
                User.UpdateJson(Context.User.Id, "Chips", (User.Load(Context.User.Id).Chips - inputChips));
            }

            await ReplyAsync(sb.ToString());
        }
    }
}
