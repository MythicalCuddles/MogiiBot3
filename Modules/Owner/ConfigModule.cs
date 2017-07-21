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
    [Group("setconfig")]
    [MinPermissions(PermissionLevel.BotOwner)]
    public class ConfigModule : ModuleBase
    {
        // SAMPLE
        //[Command(""), Summary("")]
        //public async Task abcdefgh()
        //{
        //    Configuration.UpdateJson("abcdefgh", value);
        //    await ReplyAsync(Context.User.Mention + " has updated \"abcdefgh\" to: " + value);
        //}

        #region NSFW Server ID's
        [Command("nsfwserverid"), Summary("")]
        public async Task SetNSFWServerID(ulong serverID)
        {
            Configuration.UpdateJson("NSFWServerID", serverID);
            await ReplyAsync(Context.User.Mention + " has updated \"NSFWServerID\" to: " + serverID);
        }

        [Command("rule34gamblechannelid"), Summary("")]
        public async Task SetRuleGambleChannelID(ulong channelID)
        {
            Configuration.UpdateJson("RuleGambleChannelID", channelID);
            await ReplyAsync(Context.User.Mention + " has updated \"RuleGambleChannelID\" to: " + channelID);
        }
        #endregion

        #region MogiiCraft ID's
        [Command("mogiicraftserverid"), Summary("")]
        public async Task SetMogiiCraftServerID(ulong serverID)
        {
            Configuration.UpdateJson("ServerID", serverID);
            await ReplyAsync(Context.User.Mention + " has updated \"ServerID\" to: " + serverID);
        }

        [Command("welcomechannelid"), Summary("")]
        public async Task SetWelcomeChannelID(ulong channelID)
        {
            Configuration.UpdateJson("MCWelcomeChannelID", channelID);
            await ReplyAsync(Context.User.Mention + " has updated \"MCWelcomeChannelID\" to: " + channelID);
        }

        [Command("logchannelid"), Summary("")]
        public async Task SetLogChannelID(ulong channelID)
        {
            Configuration.UpdateJson("MCLogChannelID", channelID);
            await ReplyAsync(Context.User.Mention + " has updated \"MCLogChannelID\" to: " + channelID);
        }

        [Command("minecraftchannelid"), Summary("")]
        public async Task SetMinecraftChannelID(ulong channelID)
        {
            Configuration.UpdateJson("MCMinecraftChannelID", channelID);
            await ReplyAsync(Context.User.Mention + " has updated \"MCMinecraftChannelID\" to: " + channelID);
        }
        #endregion

        #region Melissa's ID's
        [Command("supportchannelid"), Summary("")]
        public async Task SetSupportChannelID(ulong channelID)
        {
            Configuration.UpdateJson("SupportChannelID", channelID);
            await ReplyAsync(Context.User.Mention + " has updated \"SupportChannelID\" to: " + channelID);
        }

        [Command("suggestchannelid"), Summary("")]
        public async Task SetSuggestChannelID(ulong channelID)
        {
            Configuration.UpdateJson("SuggestChannelID", channelID);
            await ReplyAsync(Context.User.Mention + " has updated \"SuggestChannelID\" to: " + channelID);
        }

        [Command("ownerlogchannelid"), Summary("")]
        public async Task SetOwnerLogChannelID(ulong channelID)
        {
            Configuration.UpdateJson("LogChannelID", channelID);
            await ReplyAsync(Context.User.Mention + " has updated \"LogChannelID\" to: " + channelID);
        }

        [Command("coinmodifier"), Summary("")]
        public async Task SetCoinModifierValue(int coinModifierValue)
        {
            Configuration.UpdateJson("CoinModifier", coinModifierValue);
            await ReplyAsync(Context.User.Mention + " has updated \"CoinModifier\" to: " + coinModifierValue);
        }
        #endregion
    }
}
