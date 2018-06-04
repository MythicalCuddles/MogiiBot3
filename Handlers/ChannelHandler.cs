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

			    EmbedBuilder eb = new EmbedBuilder()
			    {
                    Title = "New Text Channel",
                    Description = channelParam.Mention,
                    Color = new Color(0x52cf35)
			    }
			    .AddField("Channel ID", channelParam.Id)
			    .AddField("Channel Name", channelParam.Name)
			    .AddField("Channel Topic", channelParam.Topic)
                .AddField("Guild ID", channelParam.GuildId)
			    .AddField("Guild Name", channelParam.Guild.Name);

                await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
            }
			else if (channel is IVoiceChannel)
			{
			    var channelParam = channel as IVoiceChannel;

			    EmbedBuilder eb = new EmbedBuilder()
			    {
			        Title = "New Voice Channel",
			        Description = channelParam.Name,
			        Color = new Color(0x52cf35)
			    }
			    .AddField("Channel ID", channelParam.Id)
			    .AddField("Channel Name", channelParam.Name)
			    .AddField("User Limit", channelParam.UserLimit)
			    .AddField("Guild ID", channelParam.GuildId)
			    .AddField("Guild Name", channelParam.Guild.Name);

			    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
			}
            else if (channel is IPrivateChannel)
			{
			    var channelParam = channel as IPrivateChannel;

			    EmbedBuilder eb = new EmbedBuilder()
			    {
			        Title = "New Private Channel",
			        Description = channelParam.Name,
			        Color = new Color(0x52cf35)
			    }
			    .AddField("Channel ID", channelParam.Id)
			    .AddField("Channel Name", channelParam.Name);

			    foreach (var u in channelParam.Recipients)
			    {
			        eb.AddField("Recipient", u.Mention, true);
			    }

			    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
			    return;
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

			    EmbedBuilder eb = new EmbedBuilder()
			    {
			        Title = "Removed Text Channel",
			        Description = channelParam.Mention,
			        Color = new Color(0xff003c)
                }
			    .AddField("Channel ID", channelParam.Id)
			    .AddField("Channel Name", channelParam.Name)
			    .AddField("Channel Topic", channelParam.Topic)
			    .AddField("Guild ID", channelParam.GuildId)
			    .AddField("Guild Name", channelParam.Guild.Name);

			    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
            }
			else if (channel is IVoiceChannel)
			{
			    var channelParam = channel as IVoiceChannel;

			    EmbedBuilder eb = new EmbedBuilder()
			    {
			        Title = "Removed Text Channel",
			        Description = channelParam.Name,
			        Color = new Color(0xff003c)
			    }
			    .AddField("Channel ID", channelParam.Id)
			    .AddField("Channel Name", channelParam.Name)
			    .AddField("User Limit", channelParam.UserLimit)
			    .AddField("Guild ID", channelParam.GuildId)
			    .AddField("Guild Name", channelParam.Guild.Name);

			    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
			}
			else if (channel is IPrivateChannel)
			{
			    var channelParam = channel as IPrivateChannel;

			    EmbedBuilder eb = new EmbedBuilder()
			        {
			            Title = "Removed Private Channel",
			            Description = channelParam.Name,
			            Color = new Color(0x52cf35)
			        }
			        .AddField("Channel ID", channelParam.Id)
			        .AddField("Channel Name", channelParam.Name);

			    foreach (var u in channelParam.Recipients)
			    {
			        eb.AddField("Recipient", u.Mention, true);
			    }

			    await Configuration.Load().LogChannelId.GetTextChannel().SendMessageAsync("", false, eb.Build());
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
