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
    [Name("Moderator Commands")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerMod)]
    public class ModeratorModule : ModuleBase
    {
        private Version _v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        [Command("stats"), Summary("Sends information about the bot.")]
        public async Task ShowStatistics()
        {
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

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName(MogiiBot3._bot.CurrentUser.Username + " Version " + _v.Major + "." + _v.Minor + "." + _v.Build + "." + _v.Revision);
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("MelissasCode Version " + MelissaCode.Version);
            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)

                .AddField("Bot Information", "**Name:** " + MogiiBot3._bot.CurrentUser.Username + "\n**Discriminator:** #" + MogiiBot3._bot.CurrentUser.Discriminator + "\n**Id:** " + MogiiBot3._bot.CurrentUser.Id)
                .AddField("Developer Information", "**Name:** " + DiscordWorker.getMelissaID.GetUser().Username + "\n**Discriminator:** #" + DiscordWorker.getMelissaID.GetUser().Discriminator + "\n**Id:** " + DiscordWorker.getMelissaID)
                .AddField("Bot Statistics", "**Active for:** " + DevelopmentSince() + "\n" + 
                "**Latency:** " + MogiiBot3._bot.Latency + "ms" + "\n" +
                "**Guild Count:** " + MogiiBot3._bot.Guilds.Count() + "\n" +
                "**User Count:** " + totalUserCount + "\n" +
                "**Channel Count:** " + totalChannelCount + " (" + totalTextChannelCount + "/" + (totalChannelCount - totalTextChannelCount) + ")" + "\n" +
                "**Overall Coins:** " + totalCoins + "\n")
                .AddField("Guild Statistics - " + Context.Guild.Name,
                "**Owner:** " + (Context.Guild.GetOwnerAsync().GetAwaiter().GetResult() as SocketGuildUser).Username + "\n" +
                "**Owner Discriminator:** #" + (Context.Guild.GetOwnerAsync().GetAwaiter().GetResult() as SocketGuildUser).Discriminator + "\n" +
                "**Owner Id:** " + (Context.Guild.GetOwnerAsync().GetAwaiter().GetResult() as SocketGuildUser).Id + "\n" +
                "**Channel Count:** " + totalGuildChannelCount + " (" + totalGuildTextChannelCount + "/" + (totalGuildChannelCount - totalGuildTextChannelCount) + ")" + "\n" +
                "**User Count:** " + totalGuildUserCount + "\n" +
                "**Coins:** " + totalGuildCoins)

                .WithFooter(efb)
                .WithThumbnailUrl(MogiiBot3._bot.CurrentUser.GetAvatarUrl())
                .WithColor(new Color(255, 116, 140));

            await ReplyAsync("", false, eb);
        }
        private static TimeSpan _uptime;
        public static DateTime _dt = new DateTime();
        private string CalculateUptime()
        {
            _uptime = DateTime.Now - _dt;
            return (_uptime.Days.ToString() + " day(s), " + _uptime.Hours.ToString() + " hour(s), " + _uptime.Minutes.ToString() + " minute(s), " + _uptime.Seconds.ToString() + " second(s)");
        }
        private static DateTime startDevelopment = DiscordWorker.developmentTime(MogiiBot3._bot.CurrentUser.Id);
        private string DevelopmentSince()
        {
            TimeSpan DevelopmentCounter = DateTime.Now - startDevelopment;
            return (DevelopmentCounter.Days.ToString() + " day(s), " + DevelopmentCounter.Hours.ToString() + " hour(s), " + DevelopmentCounter.Minutes.ToString() + " minute(s), " + DevelopmentCounter.Seconds.ToString() + " second(s)");
        }
    }
}
