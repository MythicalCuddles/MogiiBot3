using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Common;
using Discord.Commands;

namespace DiscordBot.Extensions
{
    public static class Extensions
    {
        public const string arrow_left = "⬅";
        public const string arrow_right = "➡";
        public static List<string> SlotEmotes = new List<string>()
        {           
            ":green_apple:",
            ":apple:",
            ":pear:",
            ":tangerine:",
            ":lemon:",
            ":banana:",
            ":watermelon:",
            ":grapes:",
            ":strawberry:",
            ":melon:",
            ":cherries:",
            ":peach:",
            ":pineapple:",
            ":tomato:",
            ":eggplant:",
            ":hot_pepper:",
            ":corn:",
            ":sweet_potato:",
            ":carrot:"
            //":honey_pot:",
            //":bread:",
            //":cheese:",
            //":poultry_leg:",
            //":meat_on_bone:",
            //":fried_shrimp:",
            //":cooking:",
            //":hamburger:",
            //":fries:",
            //":hotdog:",
            //":pizza:",
            //":rice_ball:"
        };

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

        public static String GetTimestamp(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy] [HH:mm:ss");
        }

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

        public static IMessage ModifyAfter(this IUserMessage msg, string message, int seconds)
        {
            Task.Run(async () =>
            {
                await Task.Delay(seconds * 1000);
                try { await msg.ModifyAsync(x => x.Content = message).ConfigureAwait(false); }
                catch { }
            });
            return msg;
        }

        public static string CreatedDate(this IUser user)
        {
            return user.CreatedAt.Day + " " + getMonthText(user.CreatedAt.Month) + " " + user.CreatedAt.Year;
        }

        public static string GuildJoinDate(this IUser user)
        {
            SocketGuildUser u = user as SocketGuildUser;

            return u.JoinedAt.Value.Day + " " + getMonthText(u.JoinedAt.Value.Month) + " " + u.JoinedAt.Value.Year;
        }

        public static string getMonthText(int monthValue)
        {
            string month = "";
            switch (monthValue)
            {
                case 1: month = "January"; break;
                case 2: month = "February"; break;
                case 3: month = "March"; break;
                case 4: month = "April"; break;
                case 5: month = "May"; break;
                case 6: month = "June"; break;
                case 7: month = "July"; break;
                case 8: month = "August"; break;
                case 9: month = "September"; break;
                case 10: month = "October"; break;
                case 11: month = "November"; break;
                case 12: month = "December"; break;
                default: month = "Unable to get month"; break;
            }
            return month;
        }
        
        public static SocketGuild GetGuild(this SocketChannel channel)
        {
            foreach(SocketGuild g in MogiiBot3._bot.Guilds)
            {
                foreach(SocketTextChannel t in g.TextChannels)
                {
                    if(channel.Id == t.Id)
                    {
                        return g;
                    }
                }

                foreach(SocketVoiceChannel v in g.VoiceChannels)
                {
                    if(channel.Id == v.Id)
                    {
                        return g;
                    }
                }
            }

            return null;
        }

        public static IGuild getGuild(this ISocketMessageChannel textChannel)
        {
            foreach (SocketGuild g in MogiiBot3._bot.Guilds)
            {
                foreach (SocketTextChannel tc in g.TextChannels)
                {
                    if (tc.Id == textChannel.Id)
                    {
                        return g;
                    }
                }
            }

            return null;
        }

        public static bool NSFWMember(this IUser iUser)
        {
            var cUser = iUser as SocketGuildUser;

            foreach(SocketGuild g in MogiiBot3._bot.Guilds)
            {
                if(g.Id == Configuration.Load().NSFWServerID)
                {
                    foreach(SocketGuildUser u in g.Users)
                    {
                        if(u == cUser)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
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

        public static string GetDiceFace(this int value)
        {
            switch(value)
            {
                case 1: return "http://i.imgur.com/IMdC6hG.png";
                case 2: return "http://i.imgur.com/3F0qYkC.png";
                case 3: return "http://i.imgur.com/vje4R3X.png";
                case 4: return "http://i.imgur.com/o5UiOW6.png";
                case 5: return "http://i.imgur.com/iM3HSaU.png";
                case 6: return "http://i.imgur.com/2KTFim8.png";
                default: return "";
            }
        }
    }
}
