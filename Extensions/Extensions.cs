using Discord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Net;
using Discord.WebSocket;
using Discord.Commands;

using Discord.Addons.EmojiTools;

using DiscordBot.Common;

using MelissasCode;

namespace DiscordBot.Extensions
{
    public static class Extensions
    {
        #region Variables
        //public const String arrow_left = "⬅", arrow_right = "➡";
        public static Emoji 
            arrow_left = EmojiExtensions.FromText(":arrow_left:"), 
            arrow_right = EmojiExtensions.FromText(":arrow_right:");
			
        #endregion

        #region Dictionaries
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
        public static Dictionary<int, string> ReactionUnicodes = new Dictionary<int, string>
        {
            { 0, "0⃣" },
            { 1, "1⃣" },
            { 2, "2⃣" },
            { 3, "3⃣" },
            { 4, "4⃣" },
            { 5, "5⃣" },
            { 6, "6⃣" },
            { 7, "7⃣" },
            { 8, "8⃣" },
            { 9, "9⃣" },
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
        public static Dictionary<int, string> ReactionStrings = new Dictionary<int, string>
        {
            { 0, ":zero:" },
            { 1, ":one:" },
            { 2, ":two:" },
            { 3, ":three:" },
            { 4, ":four:" },
            { 5, ":five:" },
            { 6, ":six:" },
            { 7, ":seven:" },
            { 8, ":eight:" },
            { 9, ":nine:" },
            { 10, ":regional_indicator_a:" },
            { 11, ":regional_indicator_b:" },
            { 12, ":regional_indicator_c:" },
            { 13, ":regional_indicator_d:" },
            { 14, ":regional_indicator_e:" },
            { 15, ":regional_indicator_f:" },
            { 16, ":regional_indicator_g:" },
            { 17, ":regional_indicator_h:" },
            { 18, ":regional_indicator_i:" },
            { 19, ":regional_indicator_j:" }
        };
        #endregion

        #region General Extensions
        public static String GetTimestamp(this DateTime value)
        {
            return value.ToString("dd/MM/yyyy] [HH:mm:ss");
        }
        public static String GetMonthText(this int monthValue)
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
        public static String GetDiceFace(this int value)
        {
            switch (value)
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
        public static List<List<string>> SplitList(this List<string> source, int nSize = 10)
        {
            var list = new List<List<string>>();

            for (int i = 0; i < source.Count; i += nSize)
            {
                list.Add(source.GetRange(i, Math.Min(nSize, source.Count - i)));
            }

            return list;
        }
        public static Int32 RandomNumber(this Random source, int minValue, int maxValue)
        {
            return source.Next(minValue, maxValue + 1);
        }
        #endregion
		
        #region Message Extensions
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
        //public static Boolean IsMessageOnNSFWChannel(this IUserMessage message)
        //{
        //    foreach (SocketGuild g in MogiiBot3._bot.Guilds)
        //    {
        //        if (g.Id == Configuration.Load().NSFWServerID)
        //        {
        //            foreach (SocketChannel c in g.Channels)
        //            {
        //                if (c.Id == message.Channel.Id)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}
        #endregion

        #region User Extensions
        public static String UserCreateDate(this IUser user)
        {
            return user.CreatedAt.Day + " " + user.CreatedAt.Month.GetMonthText() + " " + user.CreatedAt.Year;
        }
        public static String GuildJoinDate(this IUser user)
        {
            SocketGuildUser u = user as SocketGuildUser;

            return u.JoinedAt.Value.Day + " " + u.JoinedAt.Value.Month.GetMonthText() + " " + u.JoinedAt.Value.Year;
        }
        //public static Boolean NSFWMember(this IUser user)
        //{
        //    var cUser = user as SocketGuildUser;

        //    foreach (SocketGuild g in MogiiBot3._bot.Guilds)
        //    {
        //        if (g.Id == Configuration.Load().NSFWServerID)
        //        {
        //            foreach (SocketGuildUser u in g.Users)
        //            {
        //                if (u == cUser)
        //                {
        //                    return true;
        //                }
        //            }
        //        }
        //    }

        //    return false;
        //}
        #endregion
        
        #region SocketUser Gets
        public static SocketUser GetUser(this ulong id)
        {
            var user = MogiiBot3._bot.GetUser(id) as SocketUser;
            if (user == null) return null;
            return user;
        }
        #endregion

        #region SocketChannel Gets
        public static SocketTextChannel GetTextChannel(this ulong id)
        {
            var channel = MogiiBot3._bot.GetChannel(id) as SocketTextChannel;
            if (channel == null) return null;
            return channel;
        }
        public static SocketVoiceChannel GetVoiceChannel(this ulong id)
        {
            var channel = MogiiBot3._bot.GetChannel(id) as SocketVoiceChannel;
            if (channel == null) return null;
            return channel;
        }
        #endregion

        #region SocketGuild Gets
        public static SocketGuild GetGuild(this ulong id)
        {
            var guild = MogiiBot3._bot.GetGuild(id) as SocketGuild;
            if (guild == null) return null;
            return guild;
        }
        public static SocketGuild GetGuild(this ISocketMessageChannel textChannel)
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
        public static SocketGuild GetGuild(this SocketChannel channel)
        {
            foreach (SocketGuild g in MogiiBot3._bot.Guilds)
            {
                foreach (SocketTextChannel t in g.TextChannels)
                {
                    if (channel.Id == t.Id)
                    {
                        return g;
                    }
                }

                foreach (SocketVoiceChannel v in g.VoiceChannels)
                {
                    if (channel.Id == v.Id)
                    {
                        return g;
                    }
                }
            }

            return null;
        }
        #endregion
        
    }
}
