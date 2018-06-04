﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;
using MelissaNet;

namespace DiscordBot.Modules.Mod
{
    [Name("Show Settings Commands")]
    [Group("showconfig")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerMod)]
    public class ShowConfigModule : ModuleBase
    {
        [Command("")]
        public async Task ShowConfig()
        {
            await ReplyAsync("**Syntax:** " +
                             GuildConfiguration.Load(Context.Guild.Id).Prefix + "showconfig [config]\n```" +
                             "Available Commands\n" +
                             "-----------------------------\n" +
                             "-> showconfig bot\n" +
                             "-> showconfig strings\n" +
                             "-> showconfig guild\n" +
                             "```");
        }

        [Command("bot")]
        public async Task ShowBotConfig()
        {
            EmbedBuilder eb = new EmbedBuilder()
                .WithTitle("Bot Configuration")
                .WithFooter("Bot Owner permissions required to change these variables!");

            eb.WithDescription("```INI\n" +
                               "[ 1] Developer [ " + (Configuration.Load().Developer.GetUser().Username ?? "Melissa") + " ]\n" +
                               "[ 2] Developer ID [ " + Configuration.Load().Developer + " ]\n" +
                               "[ 3] Status Text [ " + (Configuration.Load().StatusText ?? "") + " ]\n" +
                               "[ 4] Status Link [ " + (Configuration.Load().StatusLink ?? "") + " ]\n" +
                               "[ 5] Status Activity [ " + Configuration.Load().StatusActivity.ToActivityType() + " ]\n" +
                               "[ 6] Status [ " + (UserStatus) Configuration.Load().Status + " ]\n" +
                               "[ 7] Unknown Command Enabled [ " + Configuration.Load().UnknownCommandEnabled.ToYesNo() + " ]\n" +
                               "[ 8] Leaderboard Amount [ " + Configuration.Load().LeaderboardAmount + " ]\n" +
                               "[ 9] Quote Cost [ " + Configuration.Load().QuoteCost + " coin(s) ]\n" +
                               "[10] Prefix Cost [ " + Configuration.Load().PrefixCost + " coin(s) ]\n" +
                               "[11] Senpai Chance Rate [ " + Configuration.Load().SenpaiChanceRate + "/100 ]\n" +
                               "[12] Global Log Channel [ #" + (Configuration.Load().LogChannelId.GetTextChannel().Name ?? "UNDEFINED") + " ]\n" +
                               "[13] Global Log Channel ID [ " + Configuration.Load().LogChannelId + " ]\n" +
                               "[14] Respects [ " + Configuration.Load().Respects + " ]\n" +
                               "[15] Min Length For Coin(s) [ " + Configuration.Load().MinLengthForCoin + " ]\n" +
                               "[16] Max Rule34 Gamble ID [ " + Configuration.Load().MaxRuleXGamble + " ]\n" +
                               "```");

            await ReplyAsync("", false, eb.Build());

        }

        [Command("strings")]
        public async Task ShowStringConfiguration()
        {
            try
            {
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("String Configuration")
                    .WithFooter("Bot Owner permissions required to change these variables!");

                eb.WithDescription("```INI\n" +
                                   "[ 1] Default Website Name [ " + (StringConfiguration.Load().DefaultWebsiteName ?? "UNDEFINED") + " ]\n" +
                                   "```");

                await ReplyAsync("", false, eb.Build());
            }
            catch (Exception e)
            {
                await ReplyAsync(
                    "It appears that your Guild Configuration has not been set-up completely. Please complete all the steps before using this command.");
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
            }
        }

        [Command("guild")]
        public async Task ShowGuildConfig()
        {
            try
            {
                EmbedBuilder eb = new EmbedBuilder()
                    .WithTitle("Guild Configuration")
                    .WithFooter("Guild Configuration can be edited by Guild Owner/Administrator");

                eb.WithDescription("```INI\n" +
                                   "[ 1] Prefix [ " + (GuildConfiguration.Load(Context.Guild.Id).Prefix ?? "UNDEFINED") + " ]\n" +
                                   "[ 2] Welcome Channel [ #" + (GuildConfiguration.Load(Context.Guild.Id).WelcomeChannelId.GetTextChannel().Name ?? "UNDEFINED") + " ]\n" +
                                   "[ 3] Welcome Channel ID [ " + (GuildConfiguration.Load(Context.Guild.Id).WelcomeChannelId.ToString() ?? "UNDEFINED") + " ]\n" +
                                   "[ 4] Log Channel [ #" + (GuildConfiguration.Load(Context.Guild.Id).LogChannelId.GetTextChannel().Name ?? "UNDEFINED") + " ]\n" +
                                   "[ 5] Log Channel ID [ " + (GuildConfiguration.Load(Context.Guild.Id).LogChannelId.ToString() ?? "UNDEFINED") + " ]\n" +
                                   "[ 6] Bot Channel [ #" + (GuildConfiguration.Load(Context.Guild.Id).BotChannelId.GetTextChannel().Name ?? "UNDEFINED") + " ]\n" +
                                   "[ 7] Bot Channel ID [ " + (GuildConfiguration.Load(Context.Guild.Id).BotChannelId.ToString() ?? "UNDEFINED") + " ]\n" +
                                   "[ 8] Senpai Enabled [ " + GuildConfiguration.Load(Context.Guild.Id).SenpaiEnabled.ToYesNo() + " ]\n" +
                                   "[ 9] Quotes Enabled [ " + GuildConfiguration.Load(Context.Guild.Id).QuotesEnabled.ToYesNo() + " ]\n" +
                                   "[10] NSFW Commands Enabled [ " + GuildConfiguration.Load(Context.Guild.Id).EnableNsfwCommands.ToYesNo() + " ]\n" +
                                   "[11] Rule34 Gamble Channel [ #" + (GuildConfiguration.Load(Context.Guild.Id).RuleGambleChannelId.GetTextChannel().Name ?? "UNDEFINED") + " ]\n" +
                                   "[12] Rule34 Gamble Channel ID [ " + (GuildConfiguration.Load(Context.Guild.Id).RuleGambleChannelId.ToString() ?? "UNDEFINED") + " ]\n" +
                                   "```");

                await ReplyAsync("", false, eb.Build());
            }
            catch (Exception e)
            {
                await ReplyAsync(
                    "It appears that your Guild Configuration has not been set-up completely. Please complete all the steps before using this command.");
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
            }
        }
    }
}
