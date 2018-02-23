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

using System.Runtime.InteropServices;

namespace DiscordBot.Modules.Mod
{
    [Name("Moderator Commands")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerMod)]
    public class ModeratorModule : ModuleBase
    {
        private readonly Version ProgramVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        [Command("stats"), Summary("Sends information about the bot.")]
        public async Task ShowStatistics()
        {
            int totalUserCount = 0, totalChannelCount = 0, totalTextChannelCount = 0, totalCoins = 0;
            int totalGuildUserCount = 0, totalGuildChannelCount = 0, totalGuildTextChannelCount = 0, totalGuildCoins = 0;

            foreach (SocketGuild g in MogiiBot3.Bot.Guilds)
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
                .WithName(MogiiBot3.Bot.CurrentUser.Username + " Version " + ProgramVersion.Major + "." + ProgramVersion.Minor + "." + ProgramVersion.Build + "." + ProgramVersion.Revision);
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("MelissaNet Version " + MelissaNet.VersionInfo.Version);
            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)

                .AddField("Bot Information", "**Name:** " + MogiiBot3.Bot.CurrentUser.Username + "\n**Discriminator:** #" + MogiiBot3.Bot.CurrentUser.Discriminator + "\n**Id:** " + MogiiBot3.Bot.CurrentUser.Id)
                .AddField("Developer Information", "**Name:** " + MelissaNet.Discord.GetMelissaId().GetUser().Username + "\n**Discriminator:** #" + MelissaNet.Discord.GetMelissaId().GetUser().Discriminator + "\n**Id:** " + MelissaNet.Discord.GetMelissaId())
                .AddField("Bot Statistics", "**Development for:** " + DevelopmentSince() + "\n" +
                "**Active for:** " + CalculateUptime() + "\n" +
                "**Latency:** " + MogiiBot3.Bot.Latency + "ms" + "\n" +
                "**Guild Count:** " + MogiiBot3.Bot.Guilds.Count() + "\n" +
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
                .WithThumbnailUrl(MogiiBot3.Bot.CurrentUser.GetAvatarUrl())
                .WithColor(new Color(255, 116, 140));

            await ReplyAsync("", false, eb.Build());
        }
        
        public static DateTime ActiveForDateTime = new DateTime();
        private string CalculateUptime()
        {
            TimeSpan uptime = DateTime.Now - ActiveForDateTime;
            return (uptime.Days.ToString() + " day(s), " + uptime.Hours.ToString() + " hour(s), " + uptime.Minutes.ToString() + " minute(s), " + uptime.Seconds.ToString() + " second(s)");
        }
        private static readonly DateTime StartDevelopment = MelissaNet.Discord.MogiiBotDevelopmentSince();
        private static string DevelopmentSince()
        {
            TimeSpan developmentCounter = DateTime.Now - StartDevelopment;
            return (developmentCounter.Days.ToString() + " day(s), " + developmentCounter.Hours.ToString() + " hour(s), " + developmentCounter.Minutes.ToString() + " minute(s), " + developmentCounter.Seconds.ToString() + " second(s)");
        }
    }
}
