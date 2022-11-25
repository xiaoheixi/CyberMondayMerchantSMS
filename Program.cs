using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MessageMedia.Messages;
using MessageMedia.Messages.Controllers;
using MessageMedia.Messages.Exceptions;
using MessageMedia.Messages.Models;

namespace TestConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            String textFile = @"C:\Users\Justin Zhao\Documents\CyberMondayMerchantsTable.csv";
            String[] lines = File.ReadAllLines(textFile);
            lines = lines.Skip(1).ToArray();
            String API_KEY = "9uxSgUOcEtLuCyLWV6vy";
            String API_SECRET = "xmSBziqxBPbIje2H4Z6BYv6GbLqZkC";
            Boolean HMAC = false;
            MessageMediaMessagesClient client = new MessageMediaMessagesClient(API_KEY, API_SECRET, HMAC);
            MessagesController messages = client.Messages;
            SendMessagesRequest body = new SendMessagesRequest();
            body.Messages = new List<Message>();
            var count = 1;
            var regex = new Regex(Regex.Escape("0"));
            foreach (string x in lines)
            {
                Message body_messages_0 = new Message();
                body_messages_0.Content = "Dear " + x.Split(',')[2].TrimStart().Replace("'", "") + ", \nMonday 28th November is Cyber Monday. Here at YQme we are planning to give all your customers 5% off all their orders for the day when they order through YQme. This discount will be covered by YQme. We will be sending out an email to your customers, informing them of the promotion this Thursday, the 24th. This will be followed up on Cyber Monday with an SMS and a link to your website. This promotion will help drive sales for a Monday. If you would like to add your own promotion on top of the 5% please get in contact with us. \n \n Enjoy! \n The Team at YQme";
                body_messages_0.DestinationNumber = regex.Replace(x.Split(',')[4].TrimStart().Replace("'", ""), "+61", 1);
                body.Messages.Add(body_messages_0);
                try
                {
                    Console.WriteLine(body_messages_0.DestinationNumber);
                    SendMessagesResponse result = messages.SendMessagesAsync(body).Result;
                    Console.WriteLine(result);
                    var tempFile = Path.GetTempFileName();
                    var linesToKeep = File.ReadLines(textFile).Where(l => l != x);
                    File.WriteAllLines(tempFile, linesToKeep);
                    File.Delete(textFile);
                    File.Move(tempFile, textFile);
                    Console.WriteLine("SMS " + count + " sent successfully!");
                }
                catch (APIException e)
                {
                    Console.WriteLine(e.Message + e.ResponseCode + e.HttpContext.ToString());
                    Console.WriteLine("SMS " + count + " failed!");
                }
                body.Messages.Remove(body_messages_0);
                count++;
            }
        }
    }
}