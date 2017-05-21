using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBot.Common.Preconditions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class MinPermissionsAttribute : PreconditionAttribute
    {
        private PermissionLevel _level;

        public MinPermissionsAttribute(PermissionLevel level)
        {
            _level = level;
        }

        public override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IDependencyMap map)
        {
            var permission = GetPermission(context);

            if (permission >= _level)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError("Insufficient permissions."));
            }
        }

        public PermissionLevel GetPermission(ICommandContext context)
        {
            if (context.User.IsBot)
                return PermissionLevel.Bot;

            if (Configuration.Load().Developer == context.User.Id)
                return PermissionLevel.BotOwner;

            var user = context.User as SocketGuildUser;

            if (user != null)
            {
                if (context.Guild.OwnerId == user.Id)
                    return PermissionLevel.ServerOwner;

                if (user.GuildPermissions.Administrator)
                    return PermissionLevel.ServerAdmin;
                
                if (user.GuildPermissions.KickMembers && user.GuildPermissions.ManageMessages && user.GuildPermissions.ManageChannels)
                    return PermissionLevel.ServerMod;
            }

            return PermissionLevel.User;
        }
    }
}
