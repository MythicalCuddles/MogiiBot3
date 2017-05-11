﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Other;

using MelissasCode;

namespace DiscordBot.Modules.Admin
{
    [Group("force")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.ServerAdmin)]
    public class ForceModule : ModuleBase
    {
        [Command("about"), Summary("Force set the about message for the specified user.")]
        public async Task ForceAbout(IUser user, [Remainder]string about)
        {
            string oldAbout = User.Load(user.Id).About;
            User.UpdateJson(user.Id, "About", about);
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s about message!");
        }

        [Command("name"), Summary("Force set the name message for the specified user.")]
        public async Task ForceName(IUser user, [Remainder]string name)
        {
            string oldName = User.Load(user.Id).Name;
            User.UpdateJson(user.Id, "Name", name);
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s name message!");
        }

        [Command("gender"), Summary("Force set the gender message for the specified user.")]
        public async Task ForceGender(IUser user, [Remainder]string gender)
        {
            string oldGender = User.Load(user.Id).Gender;
            User.UpdateJson(user.Id, "Gender", gender);
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s gender message!");
        }

        [Command("pronouns"), Summary("Force set the pronouns message for the specified user.")]
        public async Task ForcePronouns(IUser user, [Remainder]string pronouns)
        {
            string oldPronouns = User.Load(user.Id).Pronouns;
            User.UpdateJson(user.Id, "Pronouns", pronouns);
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s pronouns message!");
        }

        [Command("coins"), Summary("Force set the coins for the specified user.")]
        public async Task ForceCoins(IUser user, int newValue)
        {
            int oldCoins = User.Load(user.Id).Coins;
            User.UpdateJson(user.Id, "Coins", newValue);
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s coins value to " + newValue + " (Was: " + oldCoins + ")");
        }

        [Command("minecraftusername"), Summary("Force set the minecraft username for the specified user.")]
        public async Task ForceMinecraftUsername(IUser user, [Remainder]string username)
        {
            string oldName = User.Load(user.Id).MinecraftUsername;
            User.UpdateJson(Context.User.Id, "MinecraftUsername", username);
            await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**LOG MESSAGE**\n" + Context.User.Mention + " has changed " + user.Mention + "'s minecraft username to " + username + " (Was: " + oldName + ")");
        }
    }
}
