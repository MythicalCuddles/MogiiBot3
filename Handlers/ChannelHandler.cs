﻿using System;
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

using MelissasCode;

namespace DiscordBot.Handlers
{
	public class ChannelHandler
	{
		public static async Task ChannelCreated(SocketChannel channel)
		{
			if (channel is ITextChannel)
			{
				var channelParam = channel as ITextChannel;
				await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync("**New Text Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
					+ "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
					+ "\nTopic: " + channelParam.Topic + "```");

				//await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**New Text Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
				//    + "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
				//    + "\nTopic: " + channelParam.Topic + "```");
			}
			else if (channel is IVoiceChannel)
			{
				var channelParam = channel as IVoiceChannel;
				await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync("**New Voice Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
					+ "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
					+ "\nUser Limit: " + channelParam.UserLimit + "```");

				//await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**New Voice Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
				//    + "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
				//    + "\nUser Limit: " + channelParam.UserLimit + "```");
			}
			else
			{
				Console.Write("status: [");
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write("error");
				Console.ResetColor();
				Console.WriteLine("]    " + ": " + channel.Id + " type is unknown.");
			}
		}

		public static async Task ChannelDestroyed(SocketChannel channel)
		{
			if (channel is ITextChannel)
			{
				var channelParam = channel as ITextChannel;
				await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync("**Removed Text Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
					+ "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
					+ "\nTopic: " + channelParam.Topic + "```");

				//await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**Removed Text Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
				//    + "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
				//    + "\nTopic: " + channelParam.Topic + "```");
			}
			else if (channel is IVoiceChannel)
			{
				var channelParam = channel as IVoiceChannel;
				await Configuration.Load().LogChannelID.GetTextChannel().SendMessageAsync("**Removed Voice Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
					+ "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
					+ "\nUser Limit: " + channelParam.UserLimit + "```");

				//await GetHandler.getTextChannel(Configuration.Load().LogChannelID).SendMessageAsync("**Removed Voice Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
				//    + "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
				//    + "\nUser Limit: " + channelParam.UserLimit + "```");
			}
			else
			{
				Console.Write("status: [");
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write("error");
				Console.ResetColor();
				Console.WriteLine("]    " + ": " + channel.Id + " type is unknown.");
			}
		}
	}
}