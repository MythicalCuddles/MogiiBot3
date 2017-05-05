using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Other
{
    class QuoteHandler
    {
        public static string fileName { get; private set; } = "common/quotes.txt";
        public static List<string> quoteList = new List<string>();

        private static void LoadQuotes()
        {
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            quoteList = File.ReadAllLines(file).ToList();
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

                SaveQuotes();

                Console.WriteLine(fileName + " Created.");
            }
            LoadQuotes();
        }

        private static void SaveQuotes()
        {
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            File.WriteAllLines(file, quoteList);
        }

        public static void AddAndUpdateQuotes(string quote)
        {
            quoteList.Add(quote);
            SaveQuotes();
        }
        
        public static void RemoveAndUpdateQuotes(int index)
        {
            quoteList.RemoveAt(index);
            SaveQuotes();
        }
    }
}
