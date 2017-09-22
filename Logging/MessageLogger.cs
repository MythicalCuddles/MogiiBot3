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
    public class MessageLogger
    {
        //private static string MESSAGELOGFILE = "log/messages.log";
        private const string Directory = "log/messages/";
        private const string Extension = ".txt";
        private static string _serverDirectory;
        private static string _logFile;

        public static void LogNewMessage(SocketUserMessage message)
        {
            if(!(message.Channel is ITextChannel))
            {
                _logFile = Directory + "0#PRIVATE MESSAGE" + Extension;
            }
            else
            {
                IGuild g = message.Channel.GetGuild();
                _serverDirectory = g.Id + "#" + g.Name + "/";
                _logFile = Directory + _serverDirectory + message.Channel.Id + "#" + message.Channel.Name + Extension;
            }

            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + _logFile))
                {
                    if (!File.Exists(_logFile))
                    {
                        string path = Path.GetDirectoryName(_logFile);
                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(path);

                        Console.Write("status: [");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("ok");
                        Console.ResetColor();
                        Console.WriteLine("]    " + _logFile + ": created.");
                    }
                }
                
                using (StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + _logFile, true))
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
                _logFile = Directory + "0#PRIVATE MESSAGE" + Extension;
            }
            else
            {
                IGuild g = message.Channel.GetGuild();
                _serverDirectory = g.Id + "#" + g.Name + "/";
                _logFile = Directory + _serverDirectory + message.Channel.Id + "#" + message.Channel.Name + Extension;
            }

            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + _logFile))
                {
                    //File.Create(AppDomain.CurrentDomain.BaseDirectory + logFile).Dispose();

                    if (!File.Exists(_logFile))
                    {
                        string path = Path.GetDirectoryName(_logFile);
                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(path);

                        Console.Write("status: [");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("ok");
                        Console.ResetColor();
                        Console.WriteLine("]    " + _logFile + ": created.");
                    }
                }

                using (StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + _logFile, true))
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
                _logFile = Directory + "0#PRIVATE MESSAGE" + Extension;
            }
            else
            {
                IGuild g = message.Channel.GetGuild();
                _serverDirectory = g.Id + "#" + g.Name + "/";
                _logFile = Directory + _serverDirectory + message.Channel.Id + "#" + message.Channel.Name + Extension;
            }

            try
            {
                if (!File.Exists(AppDomain.CurrentDomain.BaseDirectory + _logFile))
                {
                    //File.Create(AppDomain.CurrentDomain.BaseDirectory + logFile).Dispose();

                    if (!File.Exists(_logFile))
                    {
                        string path = Path.GetDirectoryName(_logFile);
                        if (!System.IO.Directory.Exists(path))
                            System.IO.Directory.CreateDirectory(path);

                        Console.Write("status: [");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.Write("ok");
                        Console.ResetColor();
                        Console.WriteLine("]    " + _logFile + ": created.");
                    }
                }

                using (StreamWriter sWriter = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + _logFile, true))
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
