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

namespace DiscordBot.Modules.Mod
{
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerMod)]
    public class ModeratorModule : ModuleBase
    {
        private Version _v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        [Command("stats"), Summary("Sends information about the bot.")]
        public async Task About()
        {
            StringBuilder sb = new StringBuilder()
                .Append("**---------------------------------------------**\n")
                .Append("**Bot Name:** " + MogiiBot3._bot.CurrentUser.Username + "\n")
                .Append("**Bot ID:** " + MogiiBot3._bot.CurrentUser.Id + "\n")
                .Append("**---------------------------------------------**\n")
                .Append("**Developer Name:** " + DiscordWorker.getMelissaID.getUser().Username + "\n")
                //.Append("**Developer Name:** " + GetHandler.getUser(DiscordWorker.getMelissaID).Username + "\n")
                .Append("**Developer ID:** " + DiscordWorker.getMelissaID + "\n")
                .Append("**---------------------------------------------**\n")
                .Append("**Development:** " + developmentSince() + "\n")
                .Append("**Uptime:** " + calculateUptime() + "\n")
                .Append("**---------------------------------------------**\n");

            int totalUserCount = 0, totalChannelCount = 0, totalTextChannelCount = 0, totalCoins = 0;
            int totalGuildUserCount = 0, totalGuildChannelCount = 0, totalGuildTextChannelCount = 0, totalGuildCoins = 0;

            foreach (SocketGuild g in MogiiBot3._bot.Guilds)
            {
                foreach (SocketChannel c in g.Channels)
                {
                    totalChannelCount++;

                    if (g.Id == Context.Guild.Id)
                    {
                        totalGuildChannelCount++;
                    }
                }
                foreach (SocketTextChannel t in g.TextChannels)
                {
                    totalTextChannelCount++;

                    if (g.Id == Context.Guild.Id)
                    {
                        totalGuildTextChannelCount++;
                    }
                }
                foreach (SocketUser u in g.Users)
                {
                    totalUserCount++;
                    totalCoins += User.Load(u.Id).Coins;

                    if (g.Id == Context.Guild.Id)
                    {
                        totalGuildUserCount++;
                        totalGuildCoins += User.Load(u.Id).Coins;
                    }
                }
            }

            sb.Append("**Guild Information - " + Context.Guild.Name + "**\n")
                .Append("**Total Channels:** " + totalGuildChannelCount + " (" + totalGuildTextChannelCount + "/" + (totalGuildChannelCount - totalGuildTextChannelCount) + ")" + "\n")
                .Append("**Total Users:** " + totalGuildUserCount + "\n")
                .Append("**Total Coins:** " + totalGuildCoins + "\n")
                .Append("**---------------------------------------------**\n");

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName(MogiiBot3._bot.CurrentUser.Username + " Version " + _v.Major + "." + _v.Minor + "." + _v.Build + "." + _v.Revision);
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("Total Guilds: " + MogiiBot3._bot.Guilds.Count() + " | Total Users: " + totalUserCount + " | Total Channels: " + totalChannelCount + " (" + totalTextChannelCount + "/" + (totalChannelCount - totalTextChannelCount) + ")" + " | Total Coins: " + totalCoins);
            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithDescription(sb.ToString())
                .WithColor(new Color(255, 116, 140))
                .WithTitle("MelissasCode Version " + MelissaCode.Version)
                .WithThumbnailUrl(MogiiBot3._bot.CurrentUser.GetAvatarUrl())
                .WithFooter(efb);

            await ReplyAsync("", false, eb);
        }



        private static TimeSpan _uptime;
        public static DateTime _dt = new DateTime();
        private string calculateUptime()
        {
            _uptime = DateTime.Now - _dt;
            return (_uptime.Days.ToString() + " day(s), " + _uptime.Hours.ToString() + " hour(s), " + _uptime.Minutes.ToString() + " minute(s), " + _uptime.Seconds.ToString() + " second(s)");
        }

        private static DateTime startDevelopment = DiscordWorker.developmentTime(MogiiBot3._bot.CurrentUser.Id);
        private string developmentSince()
        {
            TimeSpan DevelopmentCounter = DateTime.Now - startDevelopment;
            return (DevelopmentCounter.Days.ToString() + " day(s), " + DevelopmentCounter.Hours.ToString() + " hour(s), " + DevelopmentCounter.Minutes.ToString() + " minute(s), " + DevelopmentCounter.Seconds.ToString() + " second(s)");
        }
    }
}
