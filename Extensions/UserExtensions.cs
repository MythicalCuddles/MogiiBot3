using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    public static class UserExtensions
    {
        public static Int32 GetCoins(this IUser user)
        {
            return User.Load(user.Id).Coins;
        }

        public static String GetName(this IUser user)
        {
            return User.Load(user.Id).Name;
        }
        public static String GetGender(this IUser user)
        {
            return User.Load(user.Id).Gender;
        }
        public static String GetPronouns(this IUser user)
        {
            return User.Load(user.Id).Pronouns;
        }
        public static String GetSnapchatUsername(this IUser user)
        {
            return User.Load(user.Id).Snapchat;
        }
    }
}
