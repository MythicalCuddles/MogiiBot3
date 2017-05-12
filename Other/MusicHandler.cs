using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Other
{
    class MusicHandler
    {
        public static string fileName { get; private set; } = "common/musicLinks.txt";
        public static List<string> musicLinkList = new List<string>();

        private static void LoadMusic()
        {
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            musicLinkList = File.ReadAllLines(file).ToList();
            
            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]    " + fileName + ": loaded.");
        }

        public static void EnsureExists()
        {
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                SaveLinks();
                
                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("ok");
                Console.ResetColor();
                Console.WriteLine("]    " + fileName + ": created.");
            }
            LoadMusic();
        }

        private static void SaveLinks()
        {
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            File.WriteAllLines(file, musicLinkList);
        }

        public static void AddAndUpdateLinks(string link)
        {
            musicLinkList.Add(link);
            SaveLinks();
        }

        public static void RemoveAndUpdateLinks(int index)
        {
            musicLinkList.RemoveAt(index);
            SaveLinks();
        }

        public static void updateLink(int index, string link)
        {
            musicLinkList[index] = link;
            SaveLinks();
        }
    }
}
