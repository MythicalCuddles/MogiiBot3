using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord.Commands;
using Discord;

using DiscordBot.Common.Preconditions;
using DiscordBot.Common;
using DiscordBot.Extensions;

using MelissasCode;

using System.Net;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace DiscordBot.Modules.NSFW
{
    [Name("NSFW Commands")]
    [RequireContext(ContextType.Guild)]
    [MinPermissions(PermissionLevel.User)]
    public class NSFWModule : ModuleBase
    {
        Random _r = new Random();

        // Rule 34 Gamble for NSFW Server - WARNING! NSFW CONTENT!
        WebClient client = new WebClient();
        HtmlDocument doc = new HtmlDocument();
        string html, url;
        int id, errorCount = 0;
        [Command("rule34gamble"), Summary("Head to #nsfw-rule34gamble and read the description for more information.")]
        [Alias("34gamble")]
        public async Task Rule34Gamble()
        {
            if (GuildConfiguration.Load(Context.Guild.Id).EnableNSFWCommands && Context.Channel.Id == GuildConfiguration.Load(Context.Guild.Id).RuleGambleChannelId || Context.User.Id == Configuration.Load().Developer)
            {
                if (Context.Channel.IsNsfw)
                {
                    var message = await ReplyAsync("Please wait while we draw your lucky number! (This shouldn't take long)");

                    try
                    {
                        id = _r.Next(1, Configuration.Load().MaxRuleXGamble);
                        url = "https://rule34.xxx/index.php?page=post&s=view&id=" + id.ToString();
                        html = client.DownloadString(url);
                        doc.LoadHtml(html);

                        List<HtmlNode> imageNodes = null;
                        imageNodes = (from HtmlNode node in doc.DocumentNode.SelectNodes("//img")
                                      where node.Name == "img"
                                      select node).ToList();

                        List<string> images = new List<string>();
                        foreach (HtmlNode node in imageNodes)
                        {
                            images.Add(node.Attributes["src"].Value);
                        }

						// -------------------------------------------------------

						Console.Write("[");
						Console.ForegroundColor = ConsoleColor.Cyan;
						Console.Write("RULE34 GAMBLE");
						Console.ResetColor();
						Console.WriteLine("]: " + Context.User.Username + " got the Gamble Id: " + id.ToString() + "\nThe following images were gathered using that Id:");
						foreach (string s in images)
						{
							Console.WriteLine(s);
						}

						// -------------------------------------------------------

						//var regex = new Regex(Regex.Escape("//"));
						//var newText = regex.Replace(images[2], "https://", 1);
						//var final = regex.Replace(newText, "/", 1, 10);

						//await ReplyAsync(Context.User.Mention + ", congratulations, you won the following image: \n" + final.ToString());
						//await ReplyAsync(Context.User.Mention + ", congratulations, you won the following image: \n" + images[2].ToString());
						message.ModifyAfter(Context.User.Mention + ", congratulations, you won the following image: \n" + images[2].ToString(), 1);
						errorCount = 0;

						//regex = null;
						//newText = null;
						//final = null;
					}
                    catch (Exception ex)
                    {
						errorCount++;
                        Console.Write("[");
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write("Gamble Exception");
                        Console.ResetColor();
                        Console.WriteLine("]: " + ex.ToString());

						if(errorCount >= 3)
						{
							await ReplyAsync(Context.User.Mention + ", the random Id you got returned no image. Lucky you! Why not try again?");
						}
						else
						{
							await Rule34Gamble();
						}
                    }

                    //await message.DeleteAsync();
                }
                else
                {
                    await ReplyAsync("**Warning!** This command requires the channel to have its NSFW setting enabled!\nDue to current restrictions with Discord.Net, this means that the channel must start with *nsfw-*");
                }
            }
        }
    }
}
