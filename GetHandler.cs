using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.WebSocket;

using MelissasCode;

namespace DiscordBot
{
    class GetHandler
    {
        public static SocketUser getUser(ulong id)
        {
            var user = Program._bot.GetUser(id) as SocketUser;
            if (user == null) return null;
            return user;
        }

        public static SocketTextChannel getTextChannel(ulong id)
        {
            var channel = Program._bot.GetChannel(id) as SocketTextChannel;
            if (channel == null) return null;
            return channel;
        }

        public static SocketVoiceChannel getVoiceChannel(ulong id)
        {
            var channel = Program._bot.GetChannel(id) as SocketVoiceChannel;
            if (channel == null) return null;
            return channel;
        }

        public static SocketGuild getGuild(ulong id)
        {
            var guild = Program._bot.GetGuild(id) as SocketGuild;
            if (guild == null) return null;
            return guild;
        }
    }
}
