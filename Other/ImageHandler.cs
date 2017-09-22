using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DiscordBot.Extensions;

namespace DiscordBot.Other
{
    public class ImageHandler
    {
        private const string FileName = "common/ImageLinks.txt";
        
        public static List<string> ImageLinkList = new List<string>();
        public static List<List<string>> SplicedImageList = new List<List<string>>();
        public static List<ulong> ImageMessages = new List<ulong>();
        public static List<int> PageNumber = new List<int>();

        private static void LoadImages()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            ImageLinkList = File.ReadAllLines(file).ToList();

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
            File.WriteAllLines(file, ImageLinkList);
        }

        public static void AddAndUpdateLinks(string link)
        {
            ImageLinkList.Add(link);
            SaveLinks();
        }

        public static void RemoveAndUpdateLinks(int index)
        {
            ImageLinkList.RemoveAt(index);
            SaveLinks();
        }

        public static void UpdateLink(int index, string link)
        {
            ImageLinkList[index] = link;
            SaveLinks();
        }

        public static void SpliceIntoList()
        {
            SplicedImageList = ImageLinkList.SplitList();
        }
        public static List<string> GetSplicedList(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return SplicedImageList[pageNumber];
        }
        public static int GetSplicedListCount
        {
            get { return SplicedImageList.Count(); }
        }
    }
}
