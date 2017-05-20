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
    [MinPermissions(PermissionLevel.BotOwner)]
    public class OwnerModule : ModuleBase
    {
        [Command("prefix"), Summary("Set the prefix for the bot.")]
        public async Task SetPrefix(string prefix)
        {
            Configuration.UpdateJson("Prefix", prefix);
            await ReplyAsync(Context.User.Mention + " has updated the Prefix to: " + prefix);
        }

        [Command("setupdatabase"), Summary("Adds all the users to the database.")]
        public async Task SetUpDatabase()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("**Database Report**\n```");

            foreach (SocketGuild g in MogiiBot3._bot.Guilds)
            {
                if (g.Id == Configuration.Load().ServerID)
                {
                    foreach (SocketGuildUser u in g.Users)
                    {
                        if(User.CreateUserFile(u.Id))
                        {
                            sb.Append(u.Username + " - Added. [" + u.Id + "]\n");
                        }
                        else
                        {
                            sb.Append(u.Username + " - Already Added. [" + u.Id + "]\n");
                        }
                    }
                }
            }

            sb.Append("```");

            await ReplyAsync(sb.ToString());
            sb.Clear();
        }

        [Command("joinme"), Summary("Gets the bot to join the current voice channel.")]
        public async Task JoinVoiceChannel(IVoiceChannel channel = null)
        {
            channel = channel ?? (Context.User as IGuildUser)?.VoiceChannel;
            if(channel == null) { await ReplyAsync("You are not in a voice channel, nor one was mentioned, " + Context.User.Mention); }

            var audioClient = await channel.ConnectAsync();
        }

        [Command("botignore"), Summary("Make the bot ignore a user.")]
        public async Task BotIgnore(IUser user)
        {
            User.UpdateJson(user.Id, "IsBotIgnoringUser", !User.Load(user.Id).IsBotIgnoringUser);

            if(User.Load(user.Id).IsBotIgnoringUser)
            {
                await ReplyAsync(Context.User.Mention + ", " + MogiiBot3._bot.CurrentUser.Username + " will start ignoring " + user.Mention);
            }
            else
            {
                await ReplyAsync(Context.User.Mention + ", " + MogiiBot3._bot.CurrentUser.Username + " will start to listen to " + user.Mention);
            }
        }

        [Command("setcointochipratio"), Summary("")]
        public async Task SetCoinToChipRatio(int coinRatio)
        {
            Configuration.UpdateJson("CoinToChipRatio", coinRatio);
            await ReplyAsync(Context.User.Mention + " has changed the coin:chip ratio to: " + coinRatio + ":1");
        }

        [Command("setchiptocoinratio"), Summary("")]
        public async Task SetChipToCoinRatio(int chipRatio)
        {
            Configuration.UpdateJson("ChipToCoinRatio", chipRatio);
            await ReplyAsync(Context.User.Mention + " has changed the chip:coin ratio to: " + chipRatio + ":1");
        }

    }
}