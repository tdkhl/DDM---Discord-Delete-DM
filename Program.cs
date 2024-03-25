using System;
using System.Threading;
using Discord;
using Discord.Gateway;
using System.Collections.Generic;
using System.Threading.Tasks;
using Console = System.Console;
using System.Drawing;
namespace DeleteDM
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string tkn = "";
            UInt64 chn = 0;
            UInt64 offset = 0;
            DiscordClient client = new DiscordClient();
            Console.Title = "Discord Delete DMs (DDM) tdetp";

            while(true)
            {
                Console.Clear();
                Colorful.Console.WriteAscii("toom", Color.FromArgb(190, 104, 255));
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1 - Submit token");
                Console.WriteLine("2 - Submit private channel ID");
                Console.WriteLine("3 - Submit message offset (optionnal)");
                Console.WriteLine("4 - Delete !");
                Console.WriteLine("0 - Exit");
                Console.ForegroundColor = ConsoleColor.White;
                
                switch (Console.ReadLine())
                {
                    case "1":
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Your token :");
                        var token = Console.ReadLine();
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.WriteLine("Connecting..");
                        try
                        {
                            client = new DiscordClient(token);
                            Console.ForegroundColor = ConsoleColor.Green;
                            tkn = token;
                            Console.WriteLine("Sucessfully connected !");
                            Thread.Sleep(3000);
                        }
                        catch (Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Uncorrect token :(");
                            Thread.Sleep(3000);
                        }

                        break;


                    case "2":
                        if(string.IsNullOrWhiteSpace(client.Token))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error : client not connected ! Don't forget to use the 1st method");
                            Thread.Sleep(3000);
                            break;
                        }
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("Private channel ID :");
                        UInt64 channelID;
                        if(!UInt64.TryParse(Console.ReadLine(), out channelID))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Channel ID isn't on the right format. Check readme");
                            Thread.Sleep(3000);
                            break;
                        }
                        try
                        {
                            DiscordChannel chan = client.GetChannel(channelID);
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine($"Successfully attached to {chan.Id}");
                            chn = chan;
                            Thread.Sleep(3000);
                        }
                        catch(Exception e)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Uncorrect private channel id");
                            Thread.Sleep(3000);
                        }
                        Thread.Sleep(3000);
                        break;
                    case "3":
                        if(chn == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You must use 2nd method first !");
                            Thread.Sleep(3000);
                            break;
                        }
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.WriteLine("MessageID to delete messages from");
                        if (UInt64.TryParse(Console.ReadLine(), out offset))
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.WriteLine("Offset registered");
                            Thread.Sleep(3000);
                            break;
                        }
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Message ID isn't on the right format. Check readme");
                        Thread.Sleep(3000);
                        break;

                    case "4":

                        if (chn == 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("You must use 2nd method first !");
                            Thread.Sleep(3000);
                            break;
                        }
                        IReadOnlyList<DiscordMessage> msg;
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        Console.WriteLine("Getting messages.. please wait..");
                        try
                        {
                             msg = await client.GetChannelMessagesAsync(chn);
                        }
                        catch(Exception ex)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("Error while getting messages..");
                            Thread.Sleep(3000);
                            break;
                        }
                        
                        if (msg.Count <= 0)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.WriteLine("No messages found");
                            Thread.Sleep(3000);
                            break;
                        }

                        bool shouldDelete = true;
                        if(offset != 0)
                        {
                            shouldDelete = false;
                        }

                        foreach(DiscordMessage mess in msg)
                        {
                            if(mess.Author.User.Username == client.User.Username)
                            {
                                if(shouldDelete)
                                {

                                    Console.ForegroundColor = ConsoleColor.Magenta;
                                    Console.WriteLine($"Deleting : {mess.Content}");
                                    try
                                    {
                                        await mess.DeleteAsync();
                                    }
                                    catch(Exception ex)
                                    {
                                        Console.ForegroundColor = ConsoleColor.Red;
                                        Console.WriteLine($"Error while deleting message : {mess.Content}");
                                        Thread.Sleep(3000);
                                        break;
                                    }
                                }
                                else
                                {
                                    if(mess.Id != offset)
                                    {
                                        continue;
                                    }
                                    shouldDelete = true;
                                }
                                Thread.Sleep(500);
                            }
                        }

                        break;
                    case "0":
                        Environment.Exit(0);
                        break;

                }
            }




        }

    }
}
