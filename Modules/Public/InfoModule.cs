using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Other;

using MelissasCode;
using Discord.WebSocket;

namespace DiscordBot.Modules.Public
{
    [MinPermissions(PermissionLevel.User)]
    public class InfoModule : ModuleBase
    {
        private Version _v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

        [Command("stats"), Summary("Sends information about the bot.")]
        public async Task About()
        {
            StringBuilder sb = new StringBuilder()
                .Append("---------------------------------------------\n")
                .Append("Bot: " + MogiiBot3._bot.CurrentUser.Mention + "\n")
                .Append("Bot ID: " + MogiiBot3._bot.CurrentUser.Id + "\n")
                .Append("[MogiiBot Repository](https://github.com/MythicalCuddles/MogiiBot3)" + "\n")
                .Append("---------------------------------------------\n")
                .Append("Developer Name: " + GetHandler.getUser(DiscordWorker.getMelissaID).Username + "\n")
                .Append("Developer ID: " + DiscordWorker.getMelissaID + "\n")
                .Append("[MythicalCuddles.xyz](http://www.mythicalcuddles.xyz)" + " | [GitHub/MythicalCuddles](https://github.com/MythicalCuddles)" + "\n")
                .Append("---------------------------------------------\n")
                .Append("With testing help from:" + "\n")
                .Append("[" + GetHandler.getUser(DiscordWorker.getAmberID).Username + "](https://github.com/AmperPil)" + ", ")
                .Append(GetHandler.getUser(DiscordWorker.getOscarID).Username)
                .Append("\n")
                .Append("---------------------------------------------\n")
                .Append("Development: " + developmentSince() + "\n")
                .Append("Uptime: " + calculateUptime());

            int userCount = 0, channelCount = 0, tChannelCount = 0;
            foreach(SocketGuild g in MogiiBot3._bot.Guilds)
            {
                foreach (SocketChannel c in g.Channels)
                    channelCount++;
                foreach (SocketTextChannel t in g.TextChannels)
                    tChannelCount++;
                foreach (SocketUser u in g.Users) 
                    userCount++;
            }

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName(MogiiBot3._bot.CurrentUser.Username + " Version " + _v.Major + "." + _v.Minor + "." + _v.Build + "." + _v.Revision);
            EmbedFooterBuilder efb = new EmbedFooterBuilder()
                .WithText("Total Guilds: " + MogiiBot3._bot.Guilds.Count() + " | Total Users: " + userCount + " | Total Channels: " + channelCount + " (" + tChannelCount + "/" + (channelCount - tChannelCount) + ")");
            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithDescription(sb.ToString())
                .WithColor(new Color(255, 116, 140))
                .WithTitle("MelissasCode Version " + MelissaCode.Version)
                .WithThumbnailUrl(MogiiBot3._bot.CurrentUser.GetAvatarUrl())
                .WithFooter(efb);

            await ReplyAsync("", false, eb);
        }

        [Command("hotlines"), Summary("Sends hotline links for the user.")]
        public async Task LinkHotlines()
        {
            await ReplyAsync("**International Helplines** \nhttp://togetherweare-strong.tumblr.com/helpline \nhttps://reddit.com/r/SuicideWatch/wiki/hotlines");
        }

        [Command("poll"), Summary("Sends a link to the poll for the minecraft server.")]
        public async Task SendPollLink()
        {
            await ReplyAsync("https://docs.google.com/forms/d/e/1FAIpQLSe9CFsWWBlInGqgVqt4SieG6JW1E81zAjtWZeEoDUTH3xPE1w/viewform?c=0&w=1");
        }

        [Command("website"), Summary("Sends a link to the forums.")]
        public async Task SendWebsiteLink()
        {
            await ReplyAsync("We currently have a forums, but it isn't active. Anyways, you can visit it here: http://mogiicraft.proboards.com/");
        }

        [Command("minecraftip"), Summary("Posts the Minecraft IP into the chat.")]
        [Alias("ip")]
        public async Task SendMinecraftIP()
        {
            await ReplyAsync("The Minecraft Server IP is: mogiicraft.ddns.net:25635");
        }

        [Command("email"), Summary("Posts the email address into the chat.")]
        public async Task PostEmailAddress()
        {
            await ReplyAsync("Send complaints to `MogiiCraft.@pizza@gmail.com`");
        }

        [Command("vote"), Summary("Sends links to the voting websites for Minecraft.")]
        public async Task SendVotingLinks()
        {
            StringBuilder sb = new StringBuilder()
                .Append("**Vote Links**\n" + Context.User.Mention + " use the following links to vote and support the server. You'll be given some diamonds in-game to say thanks :D\n");

            for(int i = 0; i < VoteLinkHandler.voteLinkList.Count; i++)
            {
                sb.Append("<" + VoteLinkHandler.voteLinkList[i] + ">\n");
            }

            sb.Append("");

            await ReplyAsync(sb.ToString());
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
