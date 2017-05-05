using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using MelissasCode;
using System.IO;
using Discord.WebSocket;

namespace DiscordBot.Logging
{
    class MessageLogger
    {
        private static string MESSAGELOGFILE = "messages.log";

        public static void logNewMessage(SocketUserMessage message)
        {
            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + MESSAGELOGFILE))
                {
                    File.Create(AppDomain.CurrentDomain.BaseDirectory + MESSAGELOGFILE).Dispose();
                }

                using (StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + MESSAGELOGFILE, true))
                {
                    String timeStamp = GetTimestamp(DateTime.Now);


                    if (!(message.Channel is ITextChannel))
                    {
                        sWriter.WriteLine("[NEW] [" + timeStamp + "] PRIVATE MESSAGE | MID: " + message.Id + " | UID: " + message.Author.Id + " | @" + message.Author.Username + " : " + message.Content);
                    }
                    else
                    {
                        sWriter.WriteLine("[NEW] [" + timeStamp + "] CID: " + message.Channel.Id + " | MID: " + message.Id + " | UID: " + message.Author.Id + " | @" + message.Author.Username + " : " + message.Content);
                    }
                    
                    sWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("File Exception");
                Console.ResetColor();
                Console.WriteLine("]: " + ex.ToString());
            }
        }

        public static void logDeleteMessage(SocketUserMessage message)
        {
            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + MESSAGELOGFILE))
                {
                    File.Create(AppDomain.CurrentDomain.BaseDirectory + MESSAGELOGFILE).Dispose();
                }

                using (StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + MESSAGELOGFILE, true))
                {
                    String timeStamp = GetTimestamp(DateTime.Now);
                    sWriter.WriteLine("[DELETE] [" + timeStamp + "] CID: " + message.Channel.Id + " | MID: " + message.Id + " | UID: " + message.Author.Id + " | @" + message.Author.Username + " : " + message.Content);
                    sWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("File Exception");
                Console.ResetColor();
                Console.WriteLine("]: " + ex.ToString());
            }
        }

        public static void logEditMessage(SocketUserMessage message)
        {
            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + MESSAGELOGFILE))
                {
                    File.Create(AppDomain.CurrentDomain.BaseDirectory + MESSAGELOGFILE).Dispose();
                }

                using (StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + MESSAGELOGFILE, true))
                {
                    String timeStamp = GetTimestamp(DateTime.Now);
                    sWriter.WriteLine("[EDIT] [" + timeStamp + "] CID: " + message.Channel.Id + " | MID: " + message.Id + " | UID: " + message.Author.Id + " | @" + message.Author.Username + " : " + message.Content);
                    sWriter.Close();
                }
            }
            catch (Exception ex)
            {
                Console.Write("[");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write("File Exception");
                Console.ResetColor();
                Console.WriteLine("]: " + ex.ToString());
            }
        }

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("dd/MM/yyyy] [HH:mm:ss");
        }
    }
}
