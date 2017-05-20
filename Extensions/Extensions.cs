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

        private static Dictionary<int, string> reactions = new Dictionary<int, string>
        {
            { 1, "1⃣" },
            { 2, "2⃣" },
            { 3, "3⃣" },
            { 4, "4⃣" },
            { 5, "5⃣" },
            { 6, "6⃣" },
            { 7, "7⃣" },
            { 8, "8⃣" },
            { 9, "9⃣" },
            { 10, "0⃣" },
            { 11, "🇦" },
            { 12, "🇧" },
            { 13, "🇨" },
            { 14, "🇩" },
            { 15, "🇪" },
            { 16, "🇫" },
            { 17, "🇬" },
            { 18, "🇭" },
            { 19, "🇮" },
            { 20, "🇯" }
        };


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
            foreach (SocketGuild g in MogiiBot3._bot.Guilds)
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

        public static int RandomNumber(this Random source, int minValue, int maxValue)
        {
            return source.Next(minValue, maxValue + 1);
        }
    }
}
