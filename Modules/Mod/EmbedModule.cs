using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;

namespace DiscordBot.Modules.Mod
{
    [Name("Embed Commands")]
    [Group("embed")]
    [MinPermissions(PermissionLevel.ServerMod)]
    public class EmbedModule : ModuleBase
    {
        private static List<IUser> users = new List<IUser>();
        private static List<EmbedBuilder> userEmbeds = new List<EmbedBuilder>();

        [Command("")]
        public async Task SendEmbedCommands()
        {
            await ReplyAsync("**Syntax:** " +
                             GuildConfiguration.Load(Context.Guild.Id).Prefix + "embed [subcommand] [parameters...] \n" +
                             "Available Commands\n" +
                             "```ini\n" +
                             "[1] embed new\n" +
                             "[2] embed withtitle [\"title\"]\n" +
                             "[3] embed withdescription [\"description\"]\n" +
                             "[4] embed withfooter [\"footer\"] [\"footer url = null\"]\n" +
                             "[5] embed withcolor [\"R Value\"] [\"G Value\"] [\"B Value\"]\n" +
                             "[6] embed send [\"#channel = #" + Context.Channel.Name + "\"]\n" +
                             "```");
        }

        [Command("new")]
        public async Task NewEmbed()
        {
            if (users.Contains(Context.User))
            {
                int index = users.FindIndex(user => user == Context.User);
                userEmbeds[index] = new EmbedBuilder();

                var message = await ReplyAsync("Your embed has been reset.");
                message.DeleteAfter(5);
            }
            else
            {
                users.Add(Context.User);
                userEmbeds.Add(new EmbedBuilder());

                var message = await ReplyAsync("Your embed has been created. Please type `" + GuildConfiguration.Load(Context.Guild.Id).Prefix + "embed` to see the available commands.");
                message.DeleteAfter(5);
            }
        }

        [Command("withtitle")]
        public async Task EmbedWithTitle(string title)
        {
            if (users.Contains(Context.User))
            {
                int index = users.FindIndex(user => user == Context.User);
                userEmbeds[index].WithTitle(title);

                var message = await ReplyAsync("added");
                message.DeleteAfter(10);
            }
            else
            {
                await NewEmbed();
                await EmbedWithTitle(title);
            }
        }

        [Command("withdescription")]
        public async Task EmbedWithDescription(string description)
        {
            if (users.Contains(Context.User))
            {
                int index = users.FindIndex(user => user == Context.User);

                userEmbeds[index].WithDescription(description);

                var message = await ReplyAsync("added");
                message.DeleteAfter(10);
            }
            else
            {
                await NewEmbed();
                await EmbedWithDescription(description);
            }
        }

        [Command("withfooter")]
        public async Task EmbedWithFooter(string footerText, string footerURL = null)
        {
            if (users.Contains(Context.User))
            {
                int index = users.FindIndex(user => user == Context.User);

                userEmbeds[index].WithFooter(footerText, footerURL);

                var message = await ReplyAsync("added");
                message.DeleteAfter(10);
            }
            else
            {
                await NewEmbed();
                await EmbedWithFooter(footerText, footerURL);
            }
        }

        [Command("withcolor")]
        [Alias("withcolour")]   
        public async Task EmbedWithColor(int r = -1, int g = -1, int b = -1)
        {
            if (r < 0 || r > 255 || g < 0 || g > 255 || b < 0 || b > 255)
            {
                await ReplyAsync(Context.User.Mention + ", you have entered an invalid value. You can use this website to help get your RGB values - <http://www.colorhexa.com/>\n\n" +
                                 "**Syntax:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "embed withcolor [R value] [G value] [B value]\n**Example:** " + GuildConfiguration.Load(Context.Guild.Id).Prefix + "embed withcolor 140 90 210");
                return;
            }

            byte rValue, gValue, bValue;

            try
            {
                byte.TryParse(r.ToString(), out rValue);
                byte.TryParse(g.ToString(), out gValue);
                byte.TryParse(b.ToString(), out bValue);
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR\n" + e.ToString());
                await ReplyAsync("An unexpected error has happened. Please ensure that you have passed through a byte value! (A number between 0 and 255)");
                return;
            }

            if (users.Contains(Context.User))
            {
                int index = users.FindIndex(user => user == Context.User);

                userEmbeds[index].WithColor(rValue, gValue, bValue);

                var message = await ReplyAsync("added");
                message.DeleteAfter(10);
            }
            else
            {
                await NewEmbed();
                await EmbedWithColor(r, g, b);
            }
        }

        [Command("send")]
        public async Task SendEmbed(SocketTextChannel channel = null)
        {
            if (users.Contains(Context.User))
            {
                int index = users.FindIndex(user => user == Context.User);

                if (channel == null)
                    channel = Context.Channel as SocketTextChannel;

                await channel.SendMessageAsync("", false, userEmbeds[index].Build());
            }
            else
            {
                await NewEmbed();
                await SendEmbed(channel);
            }
        }
    }
}
