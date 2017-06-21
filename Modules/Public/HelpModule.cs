using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;

using MelissasCode;
using Discord.WebSocket;

namespace DiscordBot.Modules.Public
{
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild | ContextType.DM)]
    [Group("help")]
    [Alias("commands")]
    public class HelpModule : ModuleBase
    {
        //[Command(""), Summary("Sends a list of all the available commands.")]
        //public async Task HelpAsync()
        //{
        //    await ReplyAsync(Context.User.Mention + ", you can find the full list of commands here: [GitHub/MythicalCuddles/MogiiBot3/Wiki](https://github.com/MythicalCuddles/MogiiBot3/wiki/Commands)");
        //    //await Context.User.CreateDMChannelAsync().Result.SendMessageAsync("Syntax: `$command` or `@MogiiBot#1772 command`." + "\n" + "You can also run some commands in this DM.To do so, simply use command with no prefix.");

        //    //try
        //    //{
        //    //    string prefix = Configuration.Load().Prefix;

        //    //    foreach (var module in MogiiBot3.commandService.Modules)
        //    //    {
        //    //        EmbedBuilder eb = new EmbedBuilder().WithColor(new Color(114, 137, 218)).WithTitle("Command List");
        //    //        string description = null;

        //    //        foreach (var cmd in module.Commands)
        //    //        {
        //    //            var result = await cmd.CheckPreconditionsAsync(Context);
        //    //            if (result.IsSuccess)
        //    //                description += prefix + cmd.Aliases.First() + " - " + cmd.Summary + "\n";
        //    //        }

        //    //        if (!string.IsNullOrWhiteSpace(description))
        //    //        {
        //    //            eb.AddField(x =>
        //    //            {
        //    //                x.Name = module.Name;
        //    //                x.Value = description;
        //    //                x.IsInline = false;
        //    //            });
        //    //        }

        //    //        await Context.User.CreateDMChannelAsync().Result.SendMessageAsync("", false, eb);
        //    //    }

        //    //    await Context.Message.DeleteAsync();

        //    //}
        //    //catch(Exception ex)
        //    //{
        //    //    Console.WriteLine(ex.ToString());
        //    //}            
        //}

        //[Command("")]
        //public async Task MainHelpMenu()
        //{

        //}

        //[Command("about"), Summary("Sends information about the available command to do with the users about embed.")]
        //public async Task HelpAbout() 
        //{

        //}

        [Command("")]
        public async Task HelpStuff()
        {
            await Context.User.CreateDMChannelAsync().Result.SendMessageAsync("**Help**\n"  +
                "**Available Commands for Help**\n```" +
                "$help about - Sends information about the commands used for your about embed.```\n" +
                "\n" +
                "For a full list of commands, please visit the Wiki : <http://mogiibot.mythicalcuddles.xyz>\n");
        }

        [Command("about")]
        public async Task HelpAbout()
        {
            await Context.User.CreateDMChannelAsync().Result.SendMessageAsync("**Help - About Embed & Your Profile**\n```" +
                "$setabout [Description] - Set your about message. This is just a message displayed telling others a little bit about yourself. You can use emotes and text formatting here.\n" +
                "$setaboutrgb [R value] [G value] [B value] - Set the colour for your about embed. You can find RGB values for colours here: <http://www.colorhexa.com/> - If you need any help setting this up, don't be afraid to ask.\n" +
                "$setpronouns [Pronouns] - Set your pronouns so others can know what they are.\n" +
                "$setmcusername [Minecraft Username] - Set your Minecraft Username so others can see who you are.\n" +
                "$setxbox [Xbox Gamertag] - Set your Xbox Gamertag if you have one so others can follow you.\n" +
                "$setpsn [PSN Username] - Set your PSN Username if you have one so others can follow you.\n" +
                "$setnintendoid [Nintendo ID] - Set your Nintendo ID if you have one so others can follow you.\n" +
                "$setsteam [Steam ID] - Set your Steam ID so others can add you.\n" +
                "$setsnapchat [Snapchat Username] - Set your Snapchat Username so others can add you.\n" +
                "$setname [Name] - Set your name so others know what to call you.\n" +
                "$setgender [Gender Identity] - Set your gender so others know.\n" +
                "```");
        }
    }
}
