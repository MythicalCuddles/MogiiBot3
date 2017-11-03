using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Common.Preconditions;
using DiscordBot.Extensions;
using DiscordBot.Logging;
using DiscordBot.Other;

using MelissasCode;

namespace DiscordBot.Common.Preconditions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MinPermissionsAttribute : PreconditionAttribute
    {
        private readonly PermissionLevel _level;

        public MinPermissionsAttribute(PermissionLevel level)
        {
            _level = level;
        }

        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider service)
        {
            var permission = GetPermission(context);

            return Task.FromResult(permission >= _level ? PreconditionResult.FromSuccess() : PreconditionResult.FromError("you don't have permission to do that!"));
        }

        public PermissionLevel GetPermission(ICommandContext context)
        {
            var user = ((SocketGuildUser)context.User);

            if (user.IsBot)
                return PermissionLevel.Bot;

			if (user.IsBotOwner())
				return PermissionLevel.BotOwner;

			if (user.IsTeamMember())
				return PermissionLevel.TeamMember;

            if (user.IsGuildOwner(context.Guild))
                return PermissionLevel.ServerOwner;

            if (user.IsGuildAdministrator())
                return PermissionLevel.ServerAdmin;

            if (user.IsGuildModerator())
                return PermissionLevel.ServerMod;

            return PermissionLevel.User;
        }
    }
}
