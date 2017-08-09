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
using DiscordBot.Extensions;

using MelissasCode;

namespace DiscordBot.Modules.Owner
{
    [Name("Configuration Commands")]
    [Group("editconfig")]
    [MinPermissions(PermissionLevel.BotOwner)]
    public class ConfigModule : ModuleBase
    {
        [Command("playing"), Summary("Changes the playing message of the bot.")]
        public async Task PlayingMessage([Remainder] string playingMessage)
        {
            Configuration.UpdateJson("Playing", playingMessage);
            await MogiiBot3._bot.SetGameAsync(playingMessage);
        }

        [Command("twitch"), Summary("Sets the twitch streaming link. Type \"none\" to disable.")]
        [Alias("streaming", "twitchstreaming")]
        public async Task SetTwitchStreamingStatus(string linkOrValue, [Remainder]string playing)
        {
            if (linkOrValue.Contains("https://www.twitch.tv/"))
            {
                await MogiiBot3._bot.SetGameAsync(playing, linkOrValue, StreamType.Twitch);
                await ReplyAsync(Context.User.Mention + ", my status has been updated to streaming with the Twitch.TV link of <" + linkOrValue + ">");
            }
            else
            {
                await MogiiBot3._bot.SetGameAsync(playing, null, StreamType.NotStreaming);
            }
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

        [Command("toggleunknowncommand"), Summary("Toggles the unknown command message.")]
        public async Task ToggleUC()
        {
            Configuration.UpdateJson("UnknownCommandEnabled", !Configuration.Load().UnknownCommandEnabled);
            await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync("UnknownCommand has been toggled by " + Context.User.Mention + " (enabled: " + Configuration.Load().UnknownCommandEnabled + ")");
        }

        [Command("leaderboardamount"), Summary("Set the amount of users who show up in the leaderboards.")]
        public async Task SetLeaderboardAmount(int value)
        {
            int oldValue = Configuration.Load().LeaderboardAmount;
            Configuration.UpdateJson("LeaderboardAmount", value);
            await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated the Leaderboard amount to: " + value + " (was: " + oldValue + ")");
        }

        [Command("quoteprice"), Summary("")]
        [Alias("changequoteprice", "updatequoteprice")]
        public async Task ChangeQuotePrice(int price)
        {
            int oldPrice = Configuration.Load().QuoteCost;
            Configuration.UpdateJson("QuoteCost", price);
            await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the quote cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
        }

        [Command("prefixprice"), Summary("")]
        [Alias("changeprefixprice", "updateprefixprice")]
        public async Task ChangePrefixPrice(int price)
        {
            int oldPrice = Configuration.Load().PrefixCost;
            Configuration.UpdateJson("PrefixCost", price);
            await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the prefix cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
        }

        //CoinsForReactions

        [Command("globallogchannel"), Summary("")]
        public async Task SetGlobalLogChannel(SocketTextChannel channel)
        {
            Configuration.UpdateJson("LogChannelID", channel.Id);
            await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated \"LogChannelID\" to: " + channel.Mention);
        }

        [Command("rule34"), Summary("Set the max random value for the Rule34 Gamble.")]
        public async Task SetRule34Max(int value)
        {
            int oldValue = Configuration.Load().MaxRuleXGamble;
            Configuration.UpdateJson("MaxRuleXGamble", value);
            await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated the Rule34 Max to: " + value + " (was: " + oldValue + ")");
        }
    }
}
