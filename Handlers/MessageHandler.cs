using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using Discord.WebSocket;

using DiscordBot;
using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Other;
using DiscordBot.Logging;

namespace DiscordBot.Handlers
{
	public class MessageHandler
	{
		public static Task MessageUpdated(Cacheable<IMessage, ulong> cachedMessage, SocketMessage message, ISocketMessageChannel channel)
		{
			var msg = message as SocketUserMessage;
			MessageLogger.LogEditMessage(msg);
			return Task.CompletedTask;
		}

		public static Task MessageDeleted(Cacheable<IMessage, ulong> cachedMessage, ISocketMessageChannel channel)
		{
			var msg = cachedMessage.Value as SocketUserMessage;
			MessageLogger.LogDeleteMessage(msg);
			return Task.CompletedTask;
		}
	}
}
