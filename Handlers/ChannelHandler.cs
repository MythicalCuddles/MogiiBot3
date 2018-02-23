using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;
using Discord.WebSocket;

using DiscordBot.Common;
using DiscordBot.Extensions;

namespace DiscordBot.Handlers
{
	public class ChannelHandler
	{
		public static async Task ChannelCreated(SocketChannel channel)
		{
			if (channel is ITextChannel)
			{
				var channelParam = channel as ITextChannel;
				await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**New Text Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
					+ "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
					+ "\nTopic: " + channelParam.Topic + "```");
			}
			else if (channel is IVoiceChannel)
			{
				var channelParam = channel as IVoiceChannel;
				await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**New Voice Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
					+ "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
					+ "\nUser Limit: " + channelParam.UserLimit + "```");
			}
			else
			{
				Console.Write("status: [");
				Console.ForegroundColor = ConsoleColor.DarkRed;
				Console.Write("error");
				Console.ResetColor();
				Console.WriteLine("]    " + ": " + channel.Id + " type is unknown.");
			}

            Channel.EnsureExists(channel.Id);
		}

		public static async Task ChannelDestroyed(SocketChannel channel)
		{
			if (channel is ITextChannel)
			{
				var channelParam = channel as ITextChannel;
				await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**Removed Text Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
					+ "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
					+ "\nTopic: " + channelParam.Topic + "```");
			}
			else if (channel is IVoiceChannel)
			{
				var channelParam = channel as IVoiceChannel;
				await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("**Removed Voice Channel**\n```ID: " + channelParam.Id + "\nName: " + channelParam.Name
					+ "\nGuild ID: " + channelParam.GuildId + "\nGuild: " + channelParam.Guild.Name
					+ "\nUser Limit: " + channelParam.UserLimit + "```");
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
