using E621_Wrapper;
using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot.E621Functions;
using TelegramBot.Msc;
using TelegramBot.Rule34;

namespace TelegramBot.Pooling
{
    //Where all the the telegram polling events are
    internal class Polling
    {
        private   TelegramBotClient Bot { get; }
        private Api E621 { get; }
       internal  Polling(TelegramBotClient Bot, Api E621) 
        {
            this.Bot = Bot;
            this.E621 = E621;
           
        }
    
        internal async void Dergmessage(object sender, MessageEventArgs messagae)
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
                            await Bot.SendTextMessageAsync(owo.Chat_id, "Please enter Your tags now~");
                            Bot.OnMessage += E621Search;
                        }
                        else { await E621_Functions.SendImageAsync(E621, owo.Text.Replace("/search", " ").Trim(), messagae, Bot); }

                        break;
                    case "/getgroup":
                        if (owo.Text.Split(" ").Length <= 1)
                        {
                            await Bot.SendTextMessageAsync(owo.Chat_id, "Please enter Your tags now~");

                            Bot.OnMessage += E621GroupAsync;
                        }
                        else
                        {
                            await E621_Functions.SendGroup(E621, owo.Text.Replace("/getgroup", " ").Trim(), messagae, Bot);
                        }
                        break;
                    case "/r34search":
                        if (owo.Text.Split(" ").Length <= 1)
                        {
                            await Bot.SendTextMessageAsync(owo.Chat_id, "Please enter Your tags now~");

                            Bot.OnMessage += Rule34SearchAsync;
                        }
                        else
                        {
                            await R34Functions.R34search(owo.Text.Replace("/r34search", " ").Trim(), messagae, Bot);
                        }
                        break;
                    case "/r34getgroup":
                        if (owo.Text.Split(" ").Length <= 1)
                        {
                            await Bot.SendTextMessageAsync(owo.Chat_id, "Please enter Your tags now~");

                            Bot.OnMessage += Rule34GetgrouphAsync;
                        }
                        else
                        {
                            await R34Functions.R34GetGroup(owo.Text.Replace("/r34getgroup", " ").Trim(), messagae, Bot);
                        }
                        break;
                    case "/getcomic":
                        if (owo.Text.Split(" ").Length <= 1)
                        {
                            await Bot.SendTextMessageAsync(owo.Chat_id, "Please enter Your tags now~");

                            Bot.OnMessage +=  GetComicAsync;
                        }
                        else
                        {
                            await E621_Functions.SendComic(E621,messagae.Message.Text.Replace("/getcomic",""),messagae,Bot);
                        }
                        break;
                    case "/reddit":
                        if (owo.Text.Split(" ").Length <= 1) 
                        {
                            await Bot.SendTextMessageAsync(owo.Chat_id, "Please enter the Sub youd like");
                            Bot.OnMessage += reddit;
                        }
                        else 
                        {
                            await Reddit.reddit.Grab_Reddit(Bot, messagae.Message.Text.Replace("/reddit", ""), messagae);
                        }

                        break;
                    case "/searcha":
                        if(owo.Text.Split(" ").Length <= 1) 
                        {
                            await Bot.SendTextMessageAsync(owo.Chat_id, "Please enter Your tags now~");
                            Bot.OnMessage += E621SearchA;
                        }
                        else 
                        {
                            await E621_Functions.SendAnimatedAysnc(E621, messagae.Message.Text.Replace("/searcha",""), messagae, Bot);
                        }
                        break;


                    default:

                        break;


                }
            }
            else
            {
                await Bot.SendStickerAsync(
                    chatId: messagae.Message.Chat,
                    sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }

        }

        private async void reddit(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {
                await Reddit.reddit.Grab_Reddit(Bot, text, message);
            }
            else
            {
                await Bot.SendStickerAsync(
                chatId: message.Message.Chat.Id,
                sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            Bot.OnMessage -= reddit;
        }

        private async void GetComicAsync(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {
                await E621_Functions.SendComic(E621, text.Replace("/getcomic", " ").Trim(), message, Bot);
            }
            else
            {
                await Bot.SendStickerAsync(
                chatId: message.Message.Chat.Id,
                sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            Bot.OnMessage -= GetComicAsync;
        }

        internal async void DergQueryAsync(object sender, InlineQueryEventArgs message)
        {

                await E621_Functions.Inline(E621, message, Bot);
            
 

        }
        

        private async void Rule34GetgrouphAsync(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {
                await R34Functions.R34GetGroup(text.Replace("/r34getgroup", " ").Trim(), message, Bot);
            }
            else
            {
                await Bot.SendStickerAsync(
                chatId: message.Message.Chat.Id,
                sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            Bot.OnMessage -= Rule34GetgrouphAsync;

        }

        private async void Rule34SearchAsync(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {
                await R34Functions.R34search(text.Replace("/r34search", " ").Trim(), message, Bot);
            }
            else
            {
                await Bot.SendStickerAsync(
                chatId: message.Message.Chat.Id,
                sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            Bot.OnMessage -= Rule34SearchAsync;

        }

        private async void E621GroupAsync(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {
                await E621_Functions.SendGroup(E621, text.Replace("/getgroup", " ").Trim(), message, Bot);
            }
            else
            {
                await Bot.SendStickerAsync(
                chatId: message.Message.Chat.Id,
                sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            Bot.OnMessage -= E621GroupAsync;
        }

        private async void E621Search(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {

                await E621_Functions.SendImageAsync(E621, text, message, Bot);
            }
            else
            {
                await Bot.SendStickerAsync(
             chatId: message.Message.Chat.Id,
             sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            Bot.OnMessage -= E621Search;
        }
        private async void E621SearchA(object sender, MessageEventArgs message)
        {
            string check = message.Message.Type.ToString();
            string text = message.Message.Text;
            if (check == "Text")
            {

                await E621_Functions.SendImageAsync(E621, text, message, Bot);
            }
            else
            {
                await Bot.SendStickerAsync(
             chatId: message.Message.Chat.Id,
             sticker: "CAACAgEAAxkBAAIDPF-nkCcsrKdDafNV1JoOONY55pLjAAIbAAMuHvUPFTQxeYJHEfceBA");
            }
            //Break out of the event once a message has been read!!
            Bot.OnMessage -= E621SearchA;
        }
    }
}
