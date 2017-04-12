using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;

using MelissasCode;

namespace DiscordBot.Modules.Admin
{
    [Group("set")]
    [RequireContext(ContextType.Guild | ContextType.DM)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class SetModule : ModuleBase<SocketCommandContext>
    {
        [Group("status")]
        public class StatusModule : ModuleBase
        {
            [Command("online"), Summary("Sets the bot's status to online.")]
            [Alias("active", "green")]
            public async Task SetOnline()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.Online);
                await Program._bot.SetStatusAsync(UserStatus.Online);
                await Context.Message.DeleteAsync();
            }

            [Command("donotdisturb"), Summary("Sets the bot's status to do not disturb.")]
            [Alias("dnd", "disturb", "red")]
            public async Task SetBusy()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.DoNotDisturb);
                await Program._bot.SetStatusAsync(UserStatus.DoNotDisturb);
                await Context.Message.DeleteAsync();
            }

            [Command("idle"), Summary("Sets the bot's status to idle.")]
            [Alias("afk", "yellow")]
            public async Task SetIdle()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.AFK);
                await Program._bot.SetStatusAsync(UserStatus.AFK);
                await Context.Message.DeleteAsync();
            }

            [Command("invisible"), Summary("Sets the bot's status to invisible.")]
            [Alias("hidden", "offline", "grey")]
            public async Task SetInvisible()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.Invisible);
                await Program._bot.SetStatusAsync(UserStatus.Invisible);
                await Context.Message.DeleteAsync();
            }
        }
    }
}
