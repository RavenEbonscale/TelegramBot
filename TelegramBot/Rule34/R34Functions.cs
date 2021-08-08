using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

namespace TelegramBot.Rule34
{
    public static class R34Functions
    {
        internal static async Task r34search(string tag, Telegram.Bot.Args.MessageEventArgs message, Telegram.Bot.TelegramBotClient bot)
        {
            string url = @$"https://rule34.xxx/index.php?page=dapi&s=post&q=index&tags={tag}";

            List<string> urls = new();
            List<(string urls, string ext)> responses = await url.Deserializetion();

            Parallel.ForEach(responses, lewdies =>
            {
                urls.Add(lewdies.urls);
            });
            if (urls.Count > 0)
            {
                await bot.SendPhotoAsync(message.Message.Chat.Id, urls.Pick());
            }


        }
        public static async Task<List<(string urls, string ext)>> Deserializetion(this string url)
        {

            HttpClient client = new HttpClient();
            // Desealize the XML file and put it into the e621 class
            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            HttpResponseMessage responseMessage = await client.SendAsync(requestMessage);
            var response = await responseMessage.Content.ReadAsStringAsync();
            //Has to be turned into an io stream so it can be used as an async
            //MemoryStream stream = new MemoryStream(response);

            XElement xelement = XElement.Parse(response);
            IEnumerable<XElement> menus = xelement.Elements();
            List<(string urls, string ext)> lewders = new List<(string urls, string ext)>();
            foreach (XAttribute lewds in menus.Attributes("file_url"))
            {
                string thing = lewds.Value;
                Uri imageurl = new Uri(thing);
                FileInfo fi = new FileInfo(imageurl.AbsolutePath);
                string ext = fi.Extension;
                lewders.Add((thing, ext));
            }
            return lewders;
        }

        internal static async Task R34GetGroup(string tag, MessageEventArgs messagae, TelegramBotClient bot)
        {
            string url = @$"https://rule34.xxx/index.php?page=dapi&s=post&q=index&tags={tag}";
            List<InputMediaPhoto> photos = new();

            List<string> urls = new();
            List<(string urls, string ext)> responses = await url.Deserializetion();
            Parallel.ForEach(responses, lewdies =>
            {

                urls.Add(lewdies.urls);
            });

            Parallel.ForEach(urls.Take(10), url => { photos.Add(new InputMediaPhoto(new InputMedia(url.Convert2Memory(), Helper.RandomName()))); });

            await bot.SendMediaGroupAsync(chatId: messagae.Message.Chat.Id, media: photos);
        }
    }
}
