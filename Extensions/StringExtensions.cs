using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Other;
using DiscordBot.Logging;

using MelissasCode;

namespace DiscordBot.Extensions
{
    public static class StringExtensions
    {
        public static String FormatWelcomeMessage(this string WelcomeMessage, SocketGuildUser e)
        {
            string msg = WelcomeMessage;

            msg = Regex.Replace(msg, "{USERMENTION}", e.Mention, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USERNAME}", e.Username, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILDNAME}", e.Guild.Name, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{DISCRIMINATOR}", e.Discriminator, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{AVATARURL}", e.GetAvatarUrl(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USERID}", e.Id.ToString(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USERCREATEDATE}", e.UserCreateDate(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USERJOINDATE}", e.GuildJoinDate(), RegexOptions.IgnoreCase);

            return msg;
        }
    }
}
