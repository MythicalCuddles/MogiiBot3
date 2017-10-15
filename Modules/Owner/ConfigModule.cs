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
        [Command("")]
        public async Task Send()
        {
            await ReplyAsync("**Syntax:** " +
                             GuildConfiguration.Load(Context.Guild.Id).Prefix + "editconfig [variable] [command syntax]\n```" +
                             "Available Commands\n" +
                             "-----------------------------\n" +
                             "Channel Message\n" +
                             "-> editconfig gameactivity [activity message] (twitch link)" +
                             //"-> editconfig playing [message]\n" +
                             //"-> editconfig twitch [twitch link] [message]\n" +
                             "-> editconfig status [status]\n" +
                             "• statuses: online, donotdisturb, idle, invisible\n" +
                             "-> editconfig toggleunknowncommand\n" +
                             "-> editconfig leaderboardamount [number of users to display]\n" +
                             "-> editconfig quoteprice [price to add quote]\n" +
                             "-> editconfig prefixprice [price to change custom prefix]\n" +
                             "-> editconfig globallogchannel [channel mention / channel id]\n" +
                             "-> editconfig rule34 [max number for random to use]\n" +
                             "-> editconfig senpaichance [number 1-100]\n" +
                             "```");
        }

        [Command("gameactivity"), Summary("Changes the playing message of the bot, and changes it to streaming mode if twitch link is inserted.")]
        public async Task SetGameActivity(string activityMessage = null, string twitchLink = null)
        {
            if (activityMessage == null)
            {
                await ReplyAsync("**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "gameactivity [\"activity message\"] (twitch link)\n`Note: Activity message needs to contain the speech marks to register properly.`");
                return;
            }

            Configuration.UpdateJson("Playing", activityMessage);
            Configuration.UpdateJson("TwitchLink", twitchLink);

            if (twitchLink == null)
            {
                await MogiiBot3.Bot.SetGameAsync(activityMessage);
            }
            else
            {
                await MogiiBot3.Bot.SetGameAsync(activityMessage, twitchLink, StreamType.Twitch);
            }

            var eb = new EmbedBuilder()
                .WithDescription(Context.User.Username + " updated " + MogiiBot3.Bot.CurrentUser.Mention + "'s game activity message.")
                .WithColor(Color.DarkGreen);

            await ReplyAsync("", false, eb.Build());
        }

        //[Command("playing"), Summary("Changes the playing message of the bot.")]
        //public async Task PlayingMessage([Remainder] string playingMessage)
        //{
        //    Configuration.UpdateJson("Playing", playingMessage);
        //    await MogiiBot3.Bot.SetGameAsync(playingMessage);
        //}

        //[Command("twitch"), Summary("Sets the twitch streaming link. Type \"none\" to disable.")]
        //[Alias("streaming", "twitchstreaming")]
        //public async Task SetTwitchStreamingStatus(string linkOrValue, [Remainder]string playing)
        //{
        //    if (linkOrValue.Contains("https://www.twitch.tv/"))
        //    {
        //        await MogiiBot3.Bot.SetGameAsync(playing, linkOrValue, StreamType.Twitch);
        //        await ReplyAsync(Context.User.Mention + ", my status has been updated to streaming with the Twitch.TV link of <" + linkOrValue + ">");
        //    }
        //    else
        //    {
        //        await MogiiBot3.Bot.SetGameAsync(playing);
        //    }
        //}

        [Group("status")]
        public class StatusModule : ModuleBase
        {
            [Command("online"), Summary("Sets the bot's status to online.")]
            [Alias("active", "green")]
            public async Task SetOnline()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.Online);
                await MogiiBot3.Bot.SetStatusAsync(UserStatus.Online);
                await ReplyAsync("Status updated to Online, " + Context.User.Mention);
            }

            [Command("donotdisturb"), Summary("Sets the bot's status to do not disturb.")]
            [Alias("dnd", "disturb", "red")]
            public async Task SetBusy()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.DoNotDisturb);
                await MogiiBot3.Bot.SetStatusAsync(UserStatus.DoNotDisturb);
                await ReplyAsync("Status updated to Do Not Disturb, " + Context.User.Mention);
            }

            [Command("idle"), Summary("Sets the bot's status to idle.")]
            [Alias("afk", "yellow")]
            public async Task SetIdle()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.AFK);
                await MogiiBot3.Bot.SetStatusAsync(UserStatus.AFK);
                await ReplyAsync("Status updated to Idle, " + Context.User.Mention);
            }

            [Command("invisible"), Summary("Sets the bot's status to invisible.")]
            [Alias("hidden", "offline", "grey")]
            public async Task SetInvisible()
            {
                Configuration.UpdateJson("Status", (int)UserStatus.Invisible);
                await MogiiBot3.Bot.SetStatusAsync(UserStatus.Invisible);
                await ReplyAsync("Status updated to Invisible, " + Context.User.Mention);
            }
        }

        [Command("toggleunknowncommand"), Summary("Toggles the unknown command message.")]
        public async Task ToggleUc()
        {
            Configuration.UpdateJson("UnknownCommandEnabled", !Configuration.Load().UnknownCommandEnabled);
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("UnknownCommand has been toggled by " + Context.User.Mention + " (enabled: " + Configuration.Load().UnknownCommandEnabled + ")");
        }

        [Command("leaderboardamount"), Summary("Set the amount of users who show up in the leaderboards.")]
        public async Task SetLeaderboardAmount(int value)
        {
            int oldValue = Configuration.Load().LeaderboardAmount;
            Configuration.UpdateJson("LeaderboardAmount", value);
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated the Leaderboard amount to: " + value + " (was: " + oldValue + ")");
        }

        [Command("quoteprice"), Summary("")]
        [Alias("changequoteprice", "updatequoteprice")]
        public async Task ChangeQuotePrice(int price)
        {
            int oldPrice = Configuration.Load().QuoteCost;
            Configuration.UpdateJson("QuoteCost", price);
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the quote cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
        }
        
        [Command("prefixprice"), Summary("")]
        [Alias("changeprefixprice", "updateprefixprice")]
        public async Task ChangePrefixPrice(int price)
        {
            int oldPrice = Configuration.Load().PrefixCost;
            Configuration.UpdateJson("PrefixCost", price);
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the prefix cost to **" + price + "** coins. (Was: **" + oldPrice + "** coins)");
        }

        [Command("senpaichance"), Summary("")]
        public async Task ChangeSenpaiChance(int chanceValue)
        {
            int oldChance = Configuration.Load().SenpaiChanceRate;
            Configuration.UpdateJson("SenpaiChanceRate", chanceValue);
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**" + Context.User.Mention + "** has updated the senpai chance to **" + chanceValue + "%**. (Was: **" + oldChance + "%**)");
        }

        [Command("globallogchannel"), Summary("")]
        public async Task SetGlobalLogChannel(SocketTextChannel channel)
        {
            Configuration.UpdateJson("LogChannelID", channel.Id);
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated \"LogChannelID\" to: " + channel.Mention);
        }

        [Command("rule34"), Summary("Set the max random value for the Rule34 Gamble.")]
        public async Task SetRule34Max(int value)
        {
            int oldValue = Configuration.Load().MaxRuleXGamble;
            Configuration.UpdateJson("MaxRuleXGamble", value);
            await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync(Context.User.Mention + " has updated the Rule34 Max to: " + value + " (was: " + oldValue + ")");
        }
    }
}
