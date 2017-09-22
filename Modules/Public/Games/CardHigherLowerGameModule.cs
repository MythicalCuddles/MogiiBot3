using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;
using DiscordBot.Other;
using DiscordBot.Logging;

using MelissasCode;

namespace DiscordBot.Modules.Public.Games
{
    // TODO: Finish this game: Bot updated early due to Unsync Bug and Channel Categories Update.

	[Name("Card Game Commands")]
	[Group("beta.card")]
	[MinPermissions(PermissionLevel.TeamMember)]
	[RequireContext(ContextType.Guild)]
	public class CardHigherLowerGameModule : ModuleBase
	{
		private readonly Random _random = new Random();
		
		[Command("drawfirst")]
		public async Task DrawFirst()
		{
			if(User.Load(Context.User.Id).LastCard == 0)
			{
				int val = _random.RandomNumber(1, 13);
				User.UpdateJson(Context.User.Id, "LastCard", val);

				await ReplyAsync("");
			}
			else
			{
				await ReplyAsync("You have already drawn your first card " + Context.User.Mention + ". Your last card has a value of: **" + User.Load(Context.User.Id).LastCard + "**.");
			}
		}
		// TODO Add Higher option for Card Game
		[Command("higher")]
		public async Task CardHigher()
		{
			int userLastCard = User.Load(Context.User.Id).LastCard;

			if (userLastCard != 0)
			{
				int drawValue = _random.RandomNumber(1, 13);

				while (drawValue == userLastCard)
					drawValue = _random.RandomNumber(1, 13);

				if(drawValue > userLastCard)
				{
					// Win
				}
				else
				{
					// Lose
				}
			}
			else
			{
				await ReplyAsync("You haven't drawn your first card yet. To do that, type: **" + GuildConfiguration.Load(Context.Guild.Id).Prefix + "card drawfirst**");
			}
		}

		// TODO Add Lower option for Card Game
		[Command("lower")]
		public async Task CardLower()
		{
			int userLastCard = User.Load(Context.User.Id).LastCard;

			if (userLastCard != 0)
			{
				int drawValue = _random.RandomNumber(1, 13);

				while (drawValue == userLastCard)
					drawValue = _random.RandomNumber(1, 13);

				if (drawValue < userLastCard)
				{
					// Win
				}
				else
				{
					// Lose
				}
			}
			else
			{
				await ReplyAsync("You haven't drawn your first card yet. To do that, type: **" + GuildConfiguration.Load(Context.Guild.Id).Prefix + "card drawfirst**");
			}
		}

		// TODO Add Info option for Card Game
		[Command("info")]
		public async Task Info()
		{
			var userLastCard = User.Load(Context.User.Id).LastCard;

			await ReplyAsync("");
		}
	}
}
