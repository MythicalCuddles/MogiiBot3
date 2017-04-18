﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;

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
            string prefix = Configuration.Load().Prefix;

            EmbedBuilder eb = new EmbedBuilder()
                .WithColor(new Color(114, 137, 218))
                .WithTitle(Program._bot.CurrentUser.Username + " Command List");

            foreach (var module in Program.commandService.Modules)
            {
                string description = null;
                foreach (var cmd in module.Commands)
                {
                    var result = await cmd.CheckPreconditionsAsync(Context);
                    if (result.IsSuccess)
                        description += prefix + cmd.Aliases.First() + " - " + cmd.Summary + "\n";
                }

                if (!string.IsNullOrWhiteSpace(description))
                {
                    eb.AddField(x =>
                    {
                        x.Name = module.Name;
                        x.Value = description;
                        x.IsInline = false;
                    });
                }
            }
            
            await Context.Message.DeleteAsync();
            await Context.User.CreateDMChannelAsync().Result.SendMessageAsync("", false, eb);
        }
    }
}