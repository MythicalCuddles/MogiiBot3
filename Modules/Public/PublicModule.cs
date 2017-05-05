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

using MelissasCode;

namespace DiscordBot.Modules.Public
{
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class PublicModule : ModuleBase
    {
        [Command("hug"), Summary("Give your friend a hug!")]
        public async Task HugUser(IUser user)
        {
            await ReplyAsync(Context.User.Mention + " has given a massive hug to " + user.Mention);
        }

        [Command("setabout"), Summary("Set your about message!")]
        public async Task SetUserAbout([Remainder]string aboutMessage)
        {
            User.UpdateJson(Context.User.Id, "About", aboutMessage);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("about"), Summary("Returns the about description about the user specified.")]
        public async Task UserAbout(IUser user = null)
        {
            var userAbout = user ?? Context.User;

            await ReplyAsync("**About " + userAbout.Username + "**\n" + User.Load(userAbout.Id).About);
        }

        [Command("setpronouns"), Summary("Set your pronouns!")]
        public async Task SetUserPronouns([Remainder]string pronouns)
        {
            User.UpdateJson(Context.User.Id, "Pronouns", pronouns);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("pronouns"), Summary("Returns the users pronouns.")]
        public async Task UserPronouns(IUser user = null)
        {
            var userSpecified = user ?? Context.User;

            await ReplyAsync("**" + userSpecified.Username + "'s Pronouns**\n" + User.Load(userSpecified.Id).Pronouns);
        }

        [Command("setmcusername"), Summary("")]
        [Alias("setminecraftusername", "mcreg", "mcregister")]
        public async Task SetMinecraftUsername([Remainder]string username)
        {
            User.UpdateJson(Context.User.Id, "MinecraftUsername", username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("mcusername"), Summary("")]
        public async Task MinecraftUsername(IUser user = null)
        {
            user = user ?? Context.User;
            await ReplyAsync(user.Username + "'s Minecraft Username is: " + User.Load(user.Id).MinecraftUsername);
        }

        [Command("music"), Summary("Replies posting a music link which has been set by staff.")]
        public async Task FavouriteMusicLink()
        {
            await ReplyAsync("Here's the music!\n" + Configuration.Load().musicLink);
        }

        [Command("suggest"), Summary("Send your suggestion for the bot!")]
        public async Task SendSupportRequest([Remainder]string message)
        {
            await GetHandler.getTextChannel(Configuration.Load().SuggestChannelID).SendMessageAsync("**Suggestion**" + "\n" +
                    Context.User.Mention + "\n" +
                    "*User Suggestion: *" + "\n" +
                    message);
        }
    }
}
