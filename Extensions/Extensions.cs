using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Common;

namespace DiscordBot.Extensions
{
    public static class Extensions
    {
        public const string arrow_left = "⬅";
        public const string arrow_right = "➡";

        public static IMessage DeleteAfter(this IUserMessage msg, int seconds)
        {
            Task.Run(async () =>
            {
                await Task.Delay(seconds * 1000);
                try { await msg.DeleteAsync().ConfigureAwait(false); }
                catch { }
            });
            return msg;
        }

        public static bool IsMessageOnNSFWChannel(this IUserMessage message)
        {
            foreach (SocketGuild g in Program._bot.Guilds)
            {
                if (g.Id == Configuration.Load().NSFWServerID)
                {
                    foreach (SocketChannel c in g.Channels)
                    {
                        if (c.Id == message.Channel.Id)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public static List<List<string>> splitList(this List<string> source, int nSize = 10)
        {
            var list = new List<List<string>>();

            for (int i = 0; i < source.Count; i += nSize)
            {
                list.Add(source.GetRange(i, Math.Min(nSize, source.Count - i)));
            }

            return list;
        }
    }
}
