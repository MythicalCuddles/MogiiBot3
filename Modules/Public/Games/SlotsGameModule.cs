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
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class SlotsGameModule : ModuleBase
    {
        Random _r = new Random();

        [Command("buyslots"), Summary("")]
        [Alias("slots")]
        public async Task PlaySlots(int inputChips)
        {
            if (inputChips < 1)
            {
                await ReplyAsync("You can not play with " + inputChips + " chips!");
                return;
            }
            else if (inputChips > User.Load(Context.User.Id).Chips)
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

            User.UpdateJson(Context.User.Id, "Chips", (User.Load(Context.User.Id).Chips - inputChips));

            if (one == two || two == three || one == three)
            {
                sb.Append("| : : : : **WIN** : : : : |\n\n");
                int chipsWon = (inputChips * 2) + inputChips;
                sb.Append("**" + Context.User.Username + "** bet **" + inputChips + "** chip(s) and won **" + chipsWon + "** chip(s).");

                User.UpdateJson(Context.User.Id, "Chips", (User.Load(Context.User.Id).Chips + chipsWon));
            }
            else
            {
                sb.Append("| : : :  **LOST**  : : : |\n\n");
                sb.Append("**" + Context.User.Username + "** bet **" + inputChips + "** chip(s) and lost.");
            }

            await ReplyAsync(sb.ToString());
        }
    }
}
