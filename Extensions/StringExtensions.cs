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
        public static String ModifyStringFlags(this string message, SocketGuildUser e)
        {
            string msg = message;

            msg = Regex.Replace(msg, "{USER.MENTION}", e.Mention, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.USERNAME}", e.Username, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.DISCRIMINATOR}", e.Discriminator, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.AVATARURL}", e.GetAvatarUrl(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.ID}", e.Id.ToString(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.CREATEDATE}", e.UserCreateDate(), RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{USER.JOINDATE}", e.GuildJoinDate(), RegexOptions.IgnoreCase);

            msg = Regex.Replace(msg, "{GUILD.NAME}", e.Guild.Name, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.USERNAME}", e.Guild.Owner.Username, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.MENTION}", e.Guild.Owner.Mention, RegexOptions.IgnoreCase);
            msg = Regex.Replace(msg, "{GUILD.OWNER.ID}", e.Guild.Owner.Id.ToString(), RegexOptions.IgnoreCase);
			msg = Regex.Replace(msg, "{GUILD.PREFIX}", GuildConfiguration.Load(e.Guild.Id).Prefix, RegexOptions.IgnoreCase);

			return msg;
        }
    }
}
