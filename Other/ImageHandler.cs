using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DiscordBot.Extensions;

namespace DiscordBot.Other
{
    class ImageHandler
    {
        public static string FileName { get; private set; } = "common/ImageLinks.txt";
        public static List<string> imageLinkList = new List<string>();
        public static List<List<string>> splicedImageList = new List<List<string>>();

        public static List<ulong> imageMessages = new List<ulong>();
        public static List<int> pageNumber = new List<int>();

        private static void LoadImages()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            imageLinkList = File.ReadAllLines(file).ToList();

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
            LoadImages();
        }

        private static void SaveLinks()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            File.WriteAllLines(file, imageLinkList);
        }

        public static void AddAndUpdateLinks(string link)
        {
            imageLinkList.Add(link);
            SaveLinks();
        }

        public static void RemoveAndUpdateLinks(int index)
        {
            imageLinkList.RemoveAt(index);
            SaveLinks();
        }

        public static void UpdateLink(int index, string link)
        {
            imageLinkList[index] = link;
            SaveLinks();
        }

        public static void SpliceIntoList()
        {
            splicedImageList = imageLinkList.SplitList();
        }
        public static List<string> GetSplicedList(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return splicedImageList[pageNumber];
        }
        public static int GetSplicedListCount
        {
            get { return splicedImageList.Count(); }
        }
    }
}
