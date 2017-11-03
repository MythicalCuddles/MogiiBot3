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
using DiscordBot.Extensions;
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
            int listAmount = Configuration.Load().LeaderboardAmount;
            List<Tuple<int, SocketGuildUser>> userList = new List<Tuple<int, SocketGuildUser>>();

            foreach (SocketGuild g in MogiiBot3.Bot.Guilds)
            {
                foreach (SocketGuildUser u in g.Users)
                {
                    if (userList.All(i => i.Item2.Id != u.Id) && !u.IsBot)
                    {
                        userList.Add(new Tuple<int, SocketGuildUser>(User.Load(u.Id).Coins, u));
                    }
                }
            }

            List<Tuple<int, SocketGuildUser>> sortedList =
                userList.OrderByDescending(intTuple => intTuple.Item1).ToList();

            if (sortedList.Count < listAmount)
                listAmount = sortedList.Count;

            StringBuilder sb = new StringBuilder()
                .Append("**Leaderboard - Top " + listAmount + "**\n```");

            List<Tuple<int, SocketGuildUser>> shownList = new List<Tuple<int, SocketGuildUser>>();
            for (int i = 0; i < listAmount; i++)
            {
                sb.Append((i + 1) + ". @" + sortedList[i].Item2.Username + ": " + sortedList[i].Item1 + " coin(s)\n");
                shownList.Add(new Tuple<int, SocketGuildUser>(sortedList[i].Item1, sortedList[i].Item2));
            }

            if (shownList.All(i => i.Item2.Id != Context.User.Id))
            {
                sb.Append("...\n");
                int pos = sortedList.FindIndex(t => t.Item2.Id == Context.User.Id);

                sb.Append((pos) + ". @" + sortedList[pos - 1].Item2.Username + ": " + sortedList[pos - 1].Item1 + " coin(s)\n");
                sb.Append((pos + 1) + ". @" + sortedList[pos].Item2.Username + ": " + sortedList[pos].Item1 + " coin(s)\n"); // Shown for User
                sb.Append((pos + 2) + ". @" + sortedList[pos + 1].Item2.Username + ": " + sortedList[pos + 2].Item1 + " coin(s)\n");
            }
            

            sb.Append("```");
            await ReplyAsync(sb.ToString());
        }

        //[Command("leaderboard"), Summary("Leaderboards for the coins system.")]
        //public async Task GetCoinLeaderboard()
        //{
        //    int topList = Configuration.Load().LeaderboardAmount;
        //    List<Tuple<int, string>> list = new List<Tuple<int, string>>();

        //    var guild = Context.Guild as SocketGuild;
        //    foreach (SocketGuildUser u in guild.Users)
        //    {
        //        if (u != null)
        //            list.Add(new Tuple<int, string>(User.Load(u.Id).Coins, u.Username));
        //    }

        //    List<Tuple<int, string>> sorted = list.OrderByDescending(iTuple => iTuple.Item1).ToList();
        //    if (sorted.Count() < topList)
        //        topList = sorted.Count();

        //    StringBuilder sb = new StringBuilder()
        //        .Append("**Coin Leaderboards** - *Top " + topList + "*\n```");

        //    for (int i = 0; i < topList; i++)
        //    {
        //        sb.Append((i + 1) + ". @" + sorted[i].Item2 + ": " + sorted[i].Item1 + " coin(s)\n");
        //    }

        //    sb.Append("```");
        //    await ReplyAsync(sb.ToString());
        //}
    }
}
