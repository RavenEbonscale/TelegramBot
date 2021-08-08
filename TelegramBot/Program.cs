using E621_Wrapper;
using System;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot.E621Functions;
using TelegramBot.Rule34;
using TelegramBot.Msc;
using Telegram.Bot.Types.ReplyMarkups;

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

            Console.Title = me.Username;
            Console.WriteLine(me);
            
            //This is for polling Telegram messages
            bot.OnMessage += Dergmessage;
            //This is how Inline works!!
            bot.OnInlineQuery += DergQueryAsync;

            bot.StartReceiving();

            Thread.Sleep(-1);



        }

        private static async void DergQueryAsync(object sender, InlineQueryEventArgs message)
        {

            await E621_Functions.Inline(e621, message, bot);


        }

        private static async void Dergmessage(object sender, MessageEventArgs messagae)
        {
           
            Owo owo = new(messagae);
                       

            Console.WriteLine(owo.User);

            if (owo.Check == "Text")
            {

              

                switch (owo.Command)
                {
                    case "/search":
                        if (owo.Text.Split(" ").Length <= 1)
                        {
                            await bot.SendTextMessageAsync(owo.Chat_id, "Please enter Your tags now~");
                            bot.OnMessage += E621Search;
                        }
                        else { await E621_Functions.SendImageAsync(e621, owo.Text.Replace("/search", " ").Trim(), messagae, bot); }

                        break;
                    case "/getgroup":
                        if (owo.Text.Split(" ").Length <= 1)
                        {
                            await bot.SendTextMessageAsync(owo.Chat_id, "Please enter Your tags now~");
                            
                            bot.OnMessage += E621GroupAsync;
                        }
                        else
                        {
                            await E621_Functions.SendGroup(e621, owo.Text.Replace("/getgroup", " ").Trim(), messagae, bot);
                        }
                        break;
                    case "/r34search":
                        if (owo.Text.Split(" ").Length <= 1)
                        {
                            await bot.SendTextMessageAsync(owo.Chat_id, "Please enter Your tags now~");
                            
                            bot.OnMessage += Rule34SearchAsync;
                        }
                        else
                        {
                            await Rule34.R34Functions.R34search(owo.Text.Replace("/r34search", " ").Trim(), messagae, bot);
                        }
                        break;
                    case "/r34getgroup":
                        if (owo.Text.Split(" ").Length <= 1)
                        {
                            await bot.SendTextMessageAsync(owo.Chat_id, "Please enter Your tags now~");
                            
                            bot.OnMessage += Rule34GetgrouphAsync;
                        }
                        else
                        {
                            await R34Functions.R34GetGroup(owo.Text.Replace("/r34getgroup", " ").Trim(), messagae, bot);
                        }
                        break;

                    default:

                        break;


                }
            }
            else
            {
                await bot.SendStickerAsync(
                    chatId: messagae.Message.Chat,
                    sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }








        }



        private static async void Rule34GetgrouphAsync(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {
                await R34Functions.R34GetGroup(text.Replace("/r34getgroup", " ").Trim(), message, bot);
            }
            else
            {
                await bot.SendStickerAsync(
                chatId: message.Message.Chat,
                sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            bot.OnMessage -= Rule34GetgrouphAsync;

        }

        private static async void Rule34SearchAsync(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {
                await R34Functions.R34search(text.Replace("/r34search", " ").Trim(), message, bot);
            }
            else
            {
                await bot.SendStickerAsync(
                chatId: message.Message.Chat,
                sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            bot.OnMessage -= Rule34SearchAsync;

        }

        private static async void E621GroupAsync(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {
                await E621_Functions.SendGroup(e621, text.Replace("/getgroup", " ").Trim(), message, bot);
            }
            else
            {
                await bot.SendStickerAsync(
                chatId: message.Message.Chat,
                sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            bot.OnMessage -= E621GroupAsync;
        }

        private static async void E621Search(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {

                await E621_Functions.SendImageAsync(e621, text, message, bot);
            }
            else
            {
                await bot.SendStickerAsync(
             chatId: message.Message.Chat,
             sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            bot.OnMessage -= E621Search;
        }
    }
}
