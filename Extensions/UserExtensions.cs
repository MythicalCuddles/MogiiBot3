using System;

using Discord;

using DiscordBot.Common;

namespace DiscordBot.Extensions
{
    public static class UserExtensions
    {
        public static int GetCoins(this IUser user)
        {
            return User.Load(user.Id).Coins;
        }
        public static int GetMythicalTokens(this IUser user)
        {
            return User.Load(user.Id).MythicalTokens;
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
        
        public static String GetAbout(this IUser user)
        {
            return User.Load(user.Id).About;
        }

        public static String GetCustomPrefix(this IUser user)
        {
            return User.Load(user.Id).CustomPrefix;
        }

        public static String GetMinecraftUser(this IUser user)
        {
            return User.Load(user.Id).MinecraftUsername;
        }
        
        public static String GetSnapchatUsername(this IUser user)
        {
            return User.Load(user.Id).Snapchat;
        }

        public static String GetFooterText(this IUser user)
        {
            return User.Load(user.Id).FooterText;
        }

        public static String GetWebsiteName(this IUser user)
        {
            return User.Load(user.Id).WebsiteName;
        }

        public static String GetWebsiteUrl(this IUser user)
        {
            return User.Load(user.Id).WebsiteUrl;
        }


        public static String GetEmbedAuthorBuilderIconUrl(this IUser user)
        {
            return User.Load(user.Id).EmbedAuthorBuilderIconUrl;
        }

        public static String GetEmbedFooterBuilderIconUrl(this IUser user)
        {
            return User.Load(user.Id).EmbedFooterBuilderIconUrl;
        }
    }
}
