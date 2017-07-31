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
    [Name("Set Commands")]
    [Group("set")]
    [RequireContext(ContextType.Guild | ContextType.DM)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class SetModule : ModuleBase
    {
        [Command(""), Summary("Get the available options for the set command.")]
        public async Task Set()
        {
            await ReplyAsync("**Syntax:** " + 
                GuildConfiguration.Load(Context.Guild.Id).Prefix + "set [variable] [value]\n```" +
                "Available Commands\n" +
                "------------------\n" +
                "-> set status online\n" +
                "-> set status donotdisturb\n" +
                "-> set status idle\n" +
                "-> set status invisible\n" +
                "-> set rule34 [Number]\n" +
                "-> set leaderboardamount [Number]\n" +
                "```");
        }

        [Group("status")]
        public class StatusModule : ModuleBase
        {
            [Command("online"), Summary("Sets the bot's status to online.")]
            [Alias("active", "green")]
            public async Task SetOnline()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.Online);
                await MogiiBot3._bot.SetStatusAsync(UserStatus.Online);
                await ReplyAsync("Status updated to Online, " + Context.User.Mention);
            }

            [Command("donotdisturb"), Summary("Sets the bot's status to do not disturb.")]
            [Alias("dnd", "disturb", "red")]
            public async Task SetBusy()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.DoNotDisturb);
                await MogiiBot3._bot.SetStatusAsync(UserStatus.DoNotDisturb);
                await ReplyAsync("Status updated to Do Not Disturb, " + Context.User.Mention);
            }

            [Command("idle"), Summary("Sets the bot's status to idle.")]
            [Alias("afk", "yellow")]
            public async Task SetIdle()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.AFK);
                await MogiiBot3._bot.SetStatusAsync(UserStatus.AFK);
                await ReplyAsync("Status updated to Idle, " + Context.User.Mention);
            }

            [Command("invisible"), Summary("Sets the bot's status to invisible.")]
            [Alias("hidden", "offline", "grey")]
            public async Task SetInvisible()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.Invisible);
                await MogiiBot3._bot.SetStatusAsync(UserStatus.Invisible);
                await ReplyAsync("Status updated to Invisible, " + Context.User.Mention);
            }
        }

        [Command("rule34"), Summary("Set the max random value for the Rule34 Gamble.")]
        public async Task SetRule34Max(int value)
        {
            int oldValue = Configuration.Load().MaxRuleXGamble;
            Configuration.UpdateJson("MaxRuleXGamble", value);
            await ReplyAsync(Context.User.Mention + " has updated the Rule34 Max to: " + value + " (was: " + oldValue + ")");
        }

        [Command("leaderboardamount"), Summary("Set the amount of users who show up in the leaderboards.")]
        public async Task SetLeaderboardAmount(int value)
        {
            int oldValue = Configuration.Load().LeaderboardAmount;
            Configuration.UpdateJson("LeaderboardAmount", value);
            await ReplyAsync(Context.User.Mention + " has updated the Leaderboard amount to: " + value + " (was: " + oldValue + ")");
        }
    }
}
