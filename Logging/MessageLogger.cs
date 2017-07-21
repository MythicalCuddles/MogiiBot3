using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Discord;
using MelissasCode;
using System.IO;
using Discord.WebSocket;
using DiscordBot.Extensions;

namespace DiscordBot.Logging
{
    class MessageLogger
    {
        //private static string MESSAGELOGFILE = "log/messages.log";
        private static string directory = "log/messages/";
        private static string extension = ".txt";
        private static string serverDirectory;
        private static string logFile;

        public static void LogNewMessage(SocketUserMessage message)
        {
            if(!(message.Channel is ITextChannel))
            {
                logFile = directory + "0#PRIVATE MESSAGE" + extension;
            }
            else
            {
                IGuild g = message.Channel.GetGuild();
                serverDirectory = g.Id + "#" + g.Name + "/";
                logFile = directory + serverDirectory + message.Channel.Id + "#" + message.Channel.Name + extension;
            }

            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + logFile))
                {
                    //File.Create(AppDomain.CurrentDomain.BaseDirectory + logFile).Dispose();
                    
                    if (!File.Exists(logFile))
                    {
                        string path = Path.GetDirectoryName(logFile);
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        Console.Write("status: [");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("ok");
                        Console.ResetColor();
                        Console.WriteLine("]    " + logFile + ": created.");
                    }
                }
                
                using (StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + logFile, true))
                {
                    String timeStamp = DateTime.Now.GetTimestamp();
                    
                    sWriter.WriteLine("[NEW] [" + timeStamp + "] MID: " + message.Id + " | UID: " + message.Author.Id + " | @" + message.Author.Username + " : " + message.Content);

                    if(message.Attachments != null)
                    {
                        foreach (Attachment a in message.Attachments)
                        {
                            sWriter.WriteLine("[NEW ATTACHMENT] [" + timeStamp + "] MID: " + message.Id + " | UID: " + message.Author.Id + " | @" + message.Author.Username + " : " + a.Url);
                        }
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

        public static void LogDeleteMessage(SocketUserMessage message)
        {
            if (!(message.Channel is ITextChannel))
            {
                logFile = directory + "0#PRIVATE MESSAGE" + extension;
            }
            else
            {
                IGuild g = message.Channel.GetGuild();
                serverDirectory = g.Id + "#" + g.Name + "/";
                logFile = directory + serverDirectory + message.Channel.Id + "#" + message.Channel.Name + extension;
            }

            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + logFile))
                {
                    //File.Create(AppDomain.CurrentDomain.BaseDirectory + logFile).Dispose();

                    if (!File.Exists(logFile))
                    {
                        string path = Path.GetDirectoryName(logFile);
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        Console.Write("status: [");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("ok");
                        Console.ResetColor();
                        Console.WriteLine("]    " + logFile + ": created.");
                    }
                }

                using (StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + logFile, true))
                {
                    String timeStamp = DateTime.Now.GetTimestamp();

                    if (message.Attachments != null)
                    {
                        foreach (Attachment a in message.Attachments)
                        {
                            sWriter.WriteLine("[DELETE] [" + timeStamp + "] MID: " + message.Id + " | UID: " + message.Author.Id + " | @" + message.Author.Username + " : " + a.Url);
                            sWriter.Close();
                        }
                    }
                    else
                    {
                        sWriter.WriteLine("[DELETE] [" + timeStamp + "] MID: " + message.Id + " | UID: " + message.Author.Id + " | @" + message.Author.Username + " : " + message.Content);

                        if (message.Attachments != null)
                        {
                            foreach (Attachment a in message.Attachments)
                            {
                                sWriter.WriteLine("[DELETE ATTACHMENT] [" + timeStamp + "] MID: " + message.Id + " | UID: " + message.Author.Id + " | @" + message.Author.Username + " : " + a.Url);
                            }
                        }

                        sWriter.Close();
                    }
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

        public static void LogEditMessage(SocketUserMessage message)
        {
            if (!(message.Channel is ITextChannel))
            {
                logFile = directory + "0#PRIVATE MESSAGE" + extension;
            }
            else
            {
                IGuild g = message.Channel.GetGuild();
                serverDirectory = g.Id + "#" + g.Name + "/";
                logFile = directory + serverDirectory + message.Channel.Id + "#" + message.Channel.Name + extension;
            }

            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + logFile))
                {
                    //File.Create(AppDomain.CurrentDomain.BaseDirectory + logFile).Dispose();

                    if (!File.Exists(logFile))
                    {
                        string path = Path.GetDirectoryName(logFile);
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        Console.Write("status: [");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("ok");
                        Console.ResetColor();
                        Console.WriteLine("]    " + logFile + ": created.");
                    }
                }

                using (StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + logFile, true))
                {
                    String timeStamp = DateTime.Now.GetTimestamp();

                    sWriter.WriteLine("[EDIT] [" + timeStamp + "] MID: " + message.Id + " | UID: " + message.Author.Id + " | @" + message.Author.Username + " : " + message.Content);
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
    }
}
