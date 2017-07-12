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
using System.Diagnostics;

namespace DiscordBot.Modules.Public
{
    [MinPermissions(PermissionLevel.User)]
    public class InfoModule : ModuleBase
    {
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
    }
}
