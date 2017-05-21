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
            var userSpecified = user ?? Context.User;

            EmbedAuthorBuilder eab = new EmbedAuthorBuilder()
                .WithName("About " + userSpecified.Username);
            if (User.Load(userSpecified.Id).BOTDevelopmentTeamMember)
            {
                eab.WithIconUrl("http://i.imgur.com/Ny5Qcto.png");
            }

            EmbedFooterBuilder efb = new EmbedFooterBuilder();
            if (User.Load(userSpecified.Id).MythicalCuddlesTeamMember)
            {
                efb.WithText(userSpecified.Username + " is a member of the MythicalCuddles Team.");
                efb.WithIconUrl("http://i.imgur.com/Ny5Qcto.png");
            }

            EmbedBuilder eb = new EmbedBuilder()
                .WithAuthor(eab)
                .WithFooter(efb)
                .WithThumbnailUrl(userSpecified.GetAvatarUrl())
                .WithDescription(User.Load(userSpecified.Id).About)
                .WithColor(new Color(140, 90, 210));


            if (User.Load(userSpecified.Id).Name != null)
                eb.AddInlineField("Name", User.Load(userSpecified.Id).Name);

            if (User.Load(userSpecified.Id).Gender != null)
                eb.AddInlineField("Gender", User.Load(userSpecified.Id).Gender);

            if (User.Load(userSpecified.Id).Pronouns != null)
                eb.AddInlineField("Pronouns", User.Load(userSpecified.Id).Pronouns);
            
            eb.AddInlineField("Coin(s)", User.Load(userSpecified.Id).Coins);

            if(User.Load(userSpecified.Id).Chips != 0)
                eb.AddInlineField("Chip(s)", User.Load(userSpecified.Id).Chips);

            if (User.Load(userSpecified.Id).MinecraftUsername != null)
                eb.AddInlineField("Minecraft Username", User.Load(userSpecified.Id).MinecraftUsername);

            if(User.Load(userSpecified.Id).TumblrUsername != null)
            {
                if(User.Load(userSpecified.Id).IsTumblrNSFW)
                {
                    eb.AddInlineField("Tumblr [NSFW]", User.Load(userSpecified.Id).TumblrUsername);
                }
                else
                {
                    eb.AddInlineField("Tumblr", User.Load(userSpecified.Id).TumblrUsername);
                }
            }
            
            if(User.Load(userSpecified.Id).XboxGamertag != null)
                eb.AddInlineField("Xbox", User.Load(userSpecified.Id).XboxGamertag);

            if (User.Load(userSpecified.Id).PSN != null)
                eb.AddInlineField("Playstation", User.Load(userSpecified.Id).PSN);

            if (User.Load(userSpecified.Id).NintendoID != null)
                eb.AddInlineField("Nintendo ID", User.Load(userSpecified.Id).NintendoID);

            if (User.Load(userSpecified.Id).SteamID != null)
                eb.AddInlineField("Steam", User.Load(userSpecified.Id).SteamID);

            await ReplyAsync("", false, eb);
        }

        [Command("setpronouns"), Summary("Set your pronouns!")]
        public async Task SetUserPronouns([Remainder]string pronouns)
        {
            User.UpdateJson(Context.User.Id, "Pronouns", pronouns);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setmcusername"), Summary("")]
        [Alias("setminecraftusername", "mcreg", "mcregister")]
        public async Task SetMinecraftUsername([Remainder]string username)
        {
            User.UpdateJson(Context.User.Id, "MinecraftUsername", username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("settumblr"), Summary("")]
        public async Task SetTumblrUsername([Remainder]string username)
        {
            User.UpdateJson(Context.User.Id, "TumblrUsername", username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("toggletumblensfw"), Summary("")]
        public async Task ToggleNSFWTumblr()
        {
            bool isNSFW = User.Load(Context.User.Id).IsTumblrNSFW;

            if(isNSFW)
            {
                User.UpdateJson(Context.User.Id, "IsTumblrNSFW", !isNSFW);
                await ReplyAsync("Your Tumblr is no longer marked as NSFW, " + Context.User.Mention);
            }
            else
            {
                User.UpdateJson(Context.User.Id, "IsTumblrNSFW", !isNSFW);
                await ReplyAsync("Your Tumblr has been marked as NSFW, " + Context.User.Mention);
            }
        }

        [Command("setxbox"), Summary("")]
        public async Task SetXboxGamertag([Remainder]string username)
        {
            User.UpdateJson(Context.User.Id, "XboxGamertag", username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setpsn"), Summary("")]
        public async Task SetPSN([Remainder]string username)
        {
            User.UpdateJson(Context.User.Id, "PSN", username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setnintendoid"), Summary("")]
        public async Task SetNintendoID([Remainder]string username)
        {
            User.UpdateJson(Context.User.Id, "NintendoID", username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setsteam"), Summary("")]
        public async Task SetSteamID([Remainder]string username)
        {
            User.UpdateJson(Context.User.Id, "SteamID", username);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setname"), Summary("")]
        public async Task SetName([Remainder]string name)
        {
            User.UpdateJson(Context.User.Id, "Name", name);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
        }

        [Command("setgender"), Summary("")]
        public async Task SetGender([Remainder]string gender)
        {
            User.UpdateJson(Context.User.Id, "Gender", gender);
            await ReplyAsync("Updated successfully, " + Context.User.Mention);
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
