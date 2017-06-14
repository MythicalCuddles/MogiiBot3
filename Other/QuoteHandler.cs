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
        public static string requestQuotesFileName { get; private set; } = "common/requestquotes.txt";

        public static List<string> quoteList = new List<string>();
        public static List<string> requestQuoteList = new List<string>();

        public static List<List<string>> splicedQuoteList = new List<List<string>>();
        public static List<List<string>> splicedRequestQuoteList = new List<List<string>>();

        public static List<ulong> quoteMessages = new List<ulong>();
        public static List<ulong> requestQuoteMessages = new List<ulong>();

        public static List<int> pageNumber = new List<int>();
        public static List<int> requestPageNumber = new List<int>();

        private static void LoadAllQuotes()
        {
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            quoteList = File.ReadAllLines(file).ToList();

            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]    " + fileName + ": loaded.");
            
            file = Path.Combine(AppContext.BaseDirectory, requestQuotesFileName);
            requestQuoteList = File.ReadAllLines(file).ToList();

            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]    " + requestQuotesFileName + ": loaded.");
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
            
            file = Path.Combine(AppContext.BaseDirectory, requestQuotesFileName);
            if (!File.Exists(file))
            {
                string path = Path.GetDirectoryName(file);
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                SaveRequestQuotes();

                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("ok");
                Console.ResetColor();
                Console.WriteLine("]    " + requestQuotesFileName + ": created.");
            }

            LoadAllQuotes( );
        }

        private static void SaveQuotes()
        {
            string file = Path.Combine(AppContext.BaseDirectory, fileName);
            File.WriteAllLines(file, quoteList);
        }

        private static void SaveRequestQuotes()
        {
            string file = Path.Combine(AppContext.BaseDirectory, requestQuotesFileName);
            File.WriteAllLines(file, requestQuoteList);
        }

        public static void AddAndUpdateQuotes(string quote)
        {
            quoteList.Add(quote);
            SaveQuotes();
        }

        public static void AddAndUpdateRequestQuotes(string quote)
        {
            requestQuoteList.Add(quote);
            SaveRequestQuotes();
        }

        public static void RemoveAndUpdateQuotes(int index)
        {
            quoteList.RemoveAt(index);
            SaveQuotes();
        }

        public static void RemoveAndUpdateRequestQuotes(int index)
        {
            requestQuoteList.RemoveAt(index);
            SaveRequestQuotes();
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

        public static void SpliceRequestQuotes()
        {
            splicedRequestQuoteList = requestQuoteList.splitList();
        }
        public static List<string> getRequestQuotes(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return splicedRequestQuoteList[pageNumber];
        }
        public static int getRequestQuotesListLength
        {
            get { return splicedRequestQuoteList.Count(); }
        }
    }
}
