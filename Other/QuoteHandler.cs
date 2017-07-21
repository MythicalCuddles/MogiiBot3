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
        public static string FileName { get; private set; } = "common/quotes.txt";
        public static string RequestQuotesFileName { get; private set; } = "common/requestquotes.txt";

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
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            quoteList = File.ReadAllLines(file).ToList();

            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]    " + FileName + ": loaded.");
            
            file = Path.Combine(AppContext.BaseDirectory, RequestQuotesFileName);
            requestQuoteList = File.ReadAllLines(file).ToList();

            Console.Write("status: [");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("ok");
            Console.ResetColor();
            Console.WriteLine("]    " + RequestQuotesFileName + ": loaded.");
        }

        public static void EnsureExists()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
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
                Console.WriteLine("]    " + FileName + ": created.");
            }
            
            file = Path.Combine(AppContext.BaseDirectory, RequestQuotesFileName);
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
                Console.WriteLine("]    " + RequestQuotesFileName + ": created.");
            }

            LoadAllQuotes( );
        }

        private static void SaveQuotes()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            File.WriteAllLines(file, quoteList);
        }

        private static void SaveRequestQuotes()
        {
            string file = Path.Combine(AppContext.BaseDirectory, RequestQuotesFileName);
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
            splicedQuoteList = quoteList.SplitList();
        }
        public static List<string> GetQuotes(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return splicedQuoteList[pageNumber];
        }
        public static int GetQuotesListLength
        {
            get { return splicedQuoteList.Count(); }
        }

        public static void SpliceRequestQuotes()
        {
            splicedRequestQuoteList = requestQuoteList.SplitList();
        }
        public static List<string> GetRequestQuotes(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return splicedRequestQuoteList[pageNumber];
        }
        public static int GetRequestQuotesListLength
        {
            get { return splicedRequestQuoteList.Count(); }
        }
    }
}
