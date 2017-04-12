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

using MelissasCode;

namespace DiscordBot.Modules.Admin
{
    [Group("admin")]
    [RequireContext(ContextType.Guild)]
    public class AdminModule : ModuleBase
    {
        [Command("say"), Summary("Echos a message.")]
        [MinPermissions(PermissionLevel.ServerAdmin)]
        public async Task Say([Remainder, Summary("The text to echo")] string echo)
        {
            await ReplyAsync(echo);
        }

        [Command("playing"), Summary("Changes the playing message of the bot.")]
        [MinPermissions(PermissionLevel.ServerAdmin)]
        public async Task PlayingMessage([Remainder, Summary("Playing Message")] string echo)
        {
            await Program._bot.SetGameAsync(echo);
        }
    }
}
