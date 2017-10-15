using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using Discord.Commands;
using Discord.WebSocket;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Other;

using MelissasCode;

namespace DiscordBot.Modules.Public
{
    [Name("Leaderboard Commands")]
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class LeaderboardModule : ModuleBase
    {
        [Command("leaderboard"), Summary("Leaderboards for the coins system.")]
        public async Task GetCoinLeaderboard()
        {
            int topList = Configuration.Load().LeaderboardAmount;
            List<Tuple<int, SocketGuildUser>> list = new List<Tuple<int, SocketGuildUser>>();
            
            foreach (SocketGuild g in MogiiBot3.Bot.Guilds)
            {
                foreach (SocketGuildUser u in g.Users)
                {
                    if (u != null && !u.IsBot)
                    {
                        if (!list.Any(user => user.Item2.Equals(u)))
                        {
                            list.Add(new Tuple<int, SocketGuildUser>(User.Load(u.Id).Coins, u));
                        }
                    }
                }
            }

            List<Tuple<int, SocketGuildUser>> sorted = list.OrderByDescending(iTuple => iTuple.Item1).ToList();
            if (sorted.Count() < topList)
                topList = sorted.Count();

            StringBuilder sb = new StringBuilder()
                .Append("**Coin Leaderboards** - *Top " + topList + "*\n```");

            for (int i = 0; i < topList; i++)
            {
                sb.Append((i + 1) + ". " + sorted[i].Item2.Username + ": " + sorted[i].Item1 + " coin(s)\n");
            }

            sb.Append("```");
            await ReplyAsync(sb.ToString());
        }
    }
}
