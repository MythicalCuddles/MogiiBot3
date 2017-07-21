using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DiscordBot.Extensions;

namespace DiscordBot.Logging
{
    class TransactionLogger
    {
        public static string FileName { get; private set; } = "common/transactions.txt";
        public static List<string> transactionsList = new List<string>();
        public static List<List<string>> splicedTransactionsList = new List<List<string>>();

        public static List<ulong> transactionMessages = new List<ulong>();
        public static List<int> pageNumber = new List<int>();

        private static void LoadTransactions()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            transactionsList = File.ReadAllLines(file).ToList();

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

                SaveTransactionsToFile();

                Console.Write("status: [");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write("ok");
                Console.ResetColor();
                Console.WriteLine("]    " + FileName + ": created.");
            }
            LoadTransactions();
        }

        private static void SaveTransactionsToFile()
        {
            string file = Path.Combine(AppContext.BaseDirectory, FileName);
            File.WriteAllLines(file, transactionsList);
        }

        public static void AddTransaction(string transaction)
        {
            String timeStamp = DateTime.Now.GetTimestamp();

            transactionsList.Add("[" + timeStamp + "] " + transaction);
            SaveTransactionsToFile();
        }

        public static void SpliceTransactionsIntoList()
        {
            splicedTransactionsList = transactionsList.SplitList();
        }
        public static List<string> GetSplicedTransactions(int pageNumber)
        {
            pageNumber = pageNumber - 1;
            return splicedTransactionsList[pageNumber];
        }
        public static int GetSplicedTransactonListCount
        {
            get { return splicedTransactionsList.Count(); }
        }
    }
}
