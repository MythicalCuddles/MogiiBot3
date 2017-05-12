using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DiscordBot.Extensions;

namespace DiscordBot.Other
{
    class QuoteHandler
    {
        public static string fileName { get; private set; } = "common/quotes.txt";
        public static List<string> quoteList = new List<string>();
        public static List<List<string>> splicedQuoteList = new List<List<string>>();

        public static List<ulong> quoteMessages = new List<ulong>();
        public static List<int> pageNumber = new List<int>();

        private static void LoadQuotes()
        {
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            quoteList = File.ReadAllLines(file).ToList();

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

                SaveQuotes();

                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("ok");
                Console.ResetColor();
                Console.WriteLine("]    " + fileName + ": created.");
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

        public static void UpdateQuote(int index, string quote)
        {
            quoteList[index] = quote;
            SaveQuotes();
        }

        public static void SpliceQuotes()
        {
            splicedQuoteList = quoteList.splitList();
        }
        public static List<string> getQuotes(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return splicedQuoteList[pageNumber];
        }
        public static int getQuotesListLength
        {
            get { return splicedQuoteList.Count(); }
        }
    }
}
