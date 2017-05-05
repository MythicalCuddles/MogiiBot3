﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Other
{
    class VoteLinkHandler
    {
        public static string fileName { get; private set; } = "common/voteLinks.txt";
        public static List<string> voteLinkList = new List<string>();

        private static void LoadQuotes()
        {
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            voteLinkList = File.ReadAllLines(file).ToList();
            Console.WriteLine(fileName + " Loaded");
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

                Console.WriteLine(fileName + " Created.");
            }
            LoadQuotes();
        }

        private static void SaveLinks()
        {
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            File.WriteAllLines(file, voteLinkList);
        }

        public static void AddAndUpdateLinks(string link)
        {
            voteLinkList.Add(link);
            SaveLinks();
        }

        public static void RemoveAndUpdateLinks(int index)
        {
            voteLinkList.RemoveAt(index);
            SaveLinks();
        }
    }
}