using E621_Wrapper;
using System;
using System.Threading;
using Telegram.Bot;
using TelegramBot.Pooling;


namespace TelegramBot
{
    class Program
    {
        private static TelegramBotClient bot;
        private static Api e621;
        static void Main()
        {
            
            
            ApiKeys keys = new();
            e621 = new(keys.ApiKeyE621, keys.username, keys.useragent);
            bot = new TelegramBotClient(keys.ApiKeytele);

            var me = bot.GetMeAsync().Result;
            Polling polling = new(bot,e621);
            Console.Title = me.Username;
            Console.WriteLine(me);
            
            //This is for polling Telegram messages
            bot.OnMessage += polling.Dergmessage;
            //This is how Inline works!!
            bot.OnInlineQuery += polling.DergQueryAsync;

            bot.StartReceiving();

            Thread.Sleep(-1);
            


        }


      








        }




    }

