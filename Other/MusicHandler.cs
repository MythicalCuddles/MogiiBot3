﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DiscordBot.Extensions;

namespace DiscordBot.Other
{
    class MusicHandler
    {
        public static string FileName { get; private set; } = "common/musicLinks.txt";
        public static List<string> musicLinkList = new List<string>();
        public static List<List<string>> splicedMusicList = new List<List<string>>();

        public static List<ulong> musicMessages = new List<ulong>();
        public static List<int> pageNumber = new List<int>();

        private static void LoadMusic()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            musicLinkList = File.ReadAllLines(file).ToList();
            
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

        public static void UpdateLink(int index, string link)
        {
            musicLinkList[index] = link;
            SaveLinks();
        }

        public static void SpliceMusicIntoList()
        {
            splicedMusicList = musicLinkList.SplitList();
        }
        public static List<string> GetSplicedMusic(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return splicedMusicList[pageNumber];
        }
        public static int GetSplicedMusicListCount
        {
            get { return splicedMusicList.Count(); }
        }
    }
}
