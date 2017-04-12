using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;

using MelissasCode;
using Discord.WebSocket;

namespace DiscordBot.Modules.Public
{
    public class InfoModule : ModuleBase
    {
        private Version _v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        [Command("about"), Summary("Sends information about the bot.")]
        [MinPermissions(PermissionLevel.User)]
        public async Task About()
        {
            StringBuilder sb = new StringBuilder()
                .Append("---------------------------------------------\n")
                .Append("Bot: " + Program._bot.CurrentUser.Mention + "\n")
                .Append("Bot ID: " + Program._bot.CurrentUser.Id + "\n")
                .Append("[MogiiBot Repository](https://github.com/MythicalCuddles/MogiiBot3)" + "\n")
                .Append("---------------------------------------------\n")
                .Append("Developer Name: " + GetHandler.getUser(DiscordWorker.getMelissasID).Mention + "\n")
                .Append("Developer ID: " + DiscordWorker.getMelissasID + "\n")
                .Append("[GitHub/MythicalCuddles](https://github.com/MythicalCuddles)" + "\n")
                .Append("---------------------------------------------\n")
                .Append("With help from " + GetHandler.getUser(DiscordWorker.getAmbersID).Mention + " | [GitHub/Amperpil](https://github.com/AmperPil)" + "\n")
                .Append("---------------------------------------------\n")
                .Append("Uptime: " + calculateUptime());

            int userCount = 0, channelCount = 0, tChannelCount = 0;
            foreach(SocketGuild g in Program._bot.Guilds)
            {
                foreach (SocketChannel c in g.Channels)
                    channelCount++;
                foreach (SocketTextChannel t in g.TextChannels)
                    tChannelCount++;
                foreach (SocketUser u in g.Users)
                    userCount++;
            }

            Color color = new Color(255, 116, 140);
            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName(Program._bot.CurrentUser.Username + " Version " + _v.Major + "." + _v.Minor + "." + _v.Build + "." + _v.Revision);
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("Total Guilds: " + Program._bot.Guilds.Count() + " | Total Users: " + userCount + " | Total Channels: " + channelCount + " (" + tChannelCount + "/" + (channelCount - tChannelCount) + ")");
            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithDescription(sb.ToString())
                .WithColor(color)
                .WithTitle("MelissasCode Version " + MelissaCode.Version)
                .WithThumbnailUrl(Program._bot.CurrentUser.GetAvatarUrl())
                .WithFooter(efb);

            await ReplyAsync("", false, eb);
        }

        [Command("hotlines"), Summary("Sends hotline links for the user.")]
        [MinPermissions(PermissionLevel.User)]
        public async Task LinkHotlines()
        {
            await ReplyAsync("**International Helplines** \nhttp://togetherweare-strong.tumblr.com/helpline \nhttps://reddit.com/r/SuicideWatch/wiki/hotlines");
        }

        private static TimeSpan _uptime;
        public static DateTime _dt = new DateTime();
        private string calculateUptime()
        {
            _uptime = DateTime.Now - _dt;
            return (_uptime.Days.ToString() + " day(s), " + _uptime.Hours.ToString() + " hour(s), " + _uptime.Minutes.ToString() + " minute(s), " + _uptime.Seconds.ToString() + " second(s)");
        }
    }
}
