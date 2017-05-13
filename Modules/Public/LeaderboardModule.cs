using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord.WebSocket;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Other;

using MelissasCode;

namespace DiscordBot.Modules.Public
{
    [Group("leaderboard")]
    [MinPermissions(PermissionLevel.User)]
    [RequireContext(ContextType.Guild)]
    public class LeaderboardModule : ModuleBase
    {
        [Command("coins"), Summary("Leaderboards for the coins system.")]
        public async Task GetCoinLeaderboard()
        {
            int topList = Configuration.Load().LeaderboardAmount;
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();

            var guild = Context.Guild as SocketGuild;
            foreach (SocketUser u in guild.Users)
            {
                list.Add(new Tuple<int, string>(User.Load(u.Id).Coins, u.Username));
            }

            List<Tuple<int, string>> Sorted = list.OrderByDescending(iTuple => iTuple.Item1).ToList();
            if (Sorted.Count() < topList)
                topList = Sorted.Count();

            StringBuilder sb = new StringBuilder()
                .Append("**Coin Leaderboards** - *Top " + topList + "*\n```");

            for (int i = 0; i < topList; i++)
            {
                sb.Append((i + 1) + ". @" + Sorted[i].Item2 + ": " + Sorted[i].Item1 + " coin(s)\n");
            }

            sb.Append("```");
            await ReplyAsync(sb.ToString());
        }

        [Command("chips"), Summary("Leaderboards for the coins system.")]
        public async Task GetChipLeaderboard()
        {
            int topList = Configuration.Load().LeaderboardAmount;
            List<Tuple<int, string>> list = new List<Tuple<int, string>>();

            var guild = Context.Guild as SocketGuild;
            foreach (SocketUser u in guild.Users)
            {
                list.Add(new Tuple<int, string>(User.Load(u.Id).Chips, u.Username));
            }

            List<Tuple<int, string>> Sorted = list.OrderByDescending(iTuple => iTuple.Item1).ToList();
            if (Sorted.Count() < topList)
                topList = Sorted.Count();

            StringBuilder sb = new StringBuilder()
                .Append("**Chips Leaderboards** - *Top " + topList + "*\n```");

            for (int i = 0; i < topList; i++)
            {
                sb.Append((i + 1) + ". @" + Sorted[i].Item2 + ": " + Sorted[i].Item1 + " chip(s)\n");
            }

            sb.Append("```");
            await ReplyAsync(sb.ToString());
        }

    }
}
