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
    public class HelpModule : ModuleBase
    {
        [Command("help"), Summary("Sends a list of all the available commands.")]
        [Alias("commands")]
        public async Task HelpAsync()
        {
            await ReplyAsync(Context.User.Mention + ", you can find the full list of commands here: [GitHub/MythicalCuddles/MogiiBot3/Wiki](https://github.com/MythicalCuddles/MogiiBot3/wiki/Commands)");
            //await Context.User.CreateDMChannelAsync().Result.SendMessageAsync("Syntax: `$command` or `@MogiiBot#1772 command`." + "\n" + "You can also run some commands in this DM.To do so, simply use command with no prefix.");

            //try
            //{
            //    string prefix = Configuration.Load().Prefix;

            //    foreach (var module in MogiiBot3.commandService.Modules)
            //    {
            //        EmbedBuilder eb = new EmbedBuilder().WithColor(new Color(114, 137, 218)).WithTitle("Command List");
            //        string description = null;

            //        foreach (var cmd in module.Commands)
            //        {
            //            var result = await cmd.CheckPreconditionsAsync(Context);
            //            if (result.IsSuccess)
            //                description += prefix + cmd.Aliases.First() + " - " + cmd.Summary + "\n";
            //        }

            //        if (!string.IsNullOrWhiteSpace(description))
            //        {
            //            eb.AddField(x =>
            //            {
            //                x.Name = module.Name;
            //                x.Value = description;
            //                x.IsInline = false;
            //            });
            //        }

            //        await Context.User.CreateDMChannelAsync().Result.SendMessageAsync("", false, eb);
            //    }

            //    await Context.Message.DeleteAsync();

            //}
            //catch(Exception ex)
            //{
            //    Console.WriteLine(ex.ToString());
            //}            
        }

        [Command("support"), Summary("Sends a message out for support.")]
        public async Task SendSupportRequest([Remainder]string message = null)
        {
            if (message == null)
            {
                await GetHandler.getTextChannel(Configuration.Load().SupportChannelID).SendMessageAsync("**Support Needed**" + "\n" +
                    Context.User.Mention + " has issued the support command in <#" + Context.Channel.Id + ">\n" +
                    "*User Added Notes*" + "\n" +
                    "User has not provided any notes.");
            }
            else
            {
                await GetHandler.getTextChannel(Configuration.Load().SupportChannelID).SendMessageAsync("**Support Needed**" + "\n" +
                     Context.User.Mention + " has issued the support command in <#" + Context.Channel.Id + ">\n" +
                     "*User Added Notes*" + "\n" + message);
            }
        }
    }
}
