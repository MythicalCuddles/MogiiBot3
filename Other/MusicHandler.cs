using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DiscordBot.Extensions;

namespace DiscordBot.Other
{
    public class MusicHandler
    {
        private const string FileName = "common/musicLinks.txt";
        
        public static List<string> MusicLinkList = new List<string>();
        public static List<List<string>> SplicedMusicList = new List<List<string>>();
        public static List<ulong> MusicMessages = new List<ulong>();
        public static List<int> PageNumber = new List<int>();

        private static void LoadMusic()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            MusicLinkList = File.ReadAllLines(file).ToList();
            
            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]    " + FileName + ": loaded.");
        }

        public static void EnsureExists()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
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
                Console.WriteLine("]    " + FileName + ": created.");
            }
            LoadMusic();
        }

        private static void SaveLinks()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            File.WriteAllLines(file, MusicLinkList);
        }

        public static void AddAndUpdateLinks(string link)
        {
            MusicLinkList.Add(link);
            SaveLinks();
        }

        public static void RemoveAndUpdateLinks(int index)
        {
            MusicLinkList.RemoveAt(index);
            SaveLinks();
        }

        public static void UpdateLink(int index, string link)
        {
            MusicLinkList[index] = link;
            SaveLinks();
        }

        public static void SpliceMusicIntoList()
        {
            SplicedMusicList = MusicLinkList.SplitList();
        }
        public static List<string> GetSplicedMusic(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return SplicedMusicList[pageNumber];
        }
        public static int GetSplicedMusicListCount
        {
            get { return SplicedMusicList.Count(); }
        }
    }
}
