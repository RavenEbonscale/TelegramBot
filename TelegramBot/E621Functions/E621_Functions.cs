using E621_Wrapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.InlineQueryResults;

namespace TelegramBot.E621Functions
{
    public static class E621_Functions
    {
        private static readonly object lockMe = new();

        internal static async Task SendImageAsync(Api e621, string tags, MessageEventArgs messagae, TelegramBotClient bot)
        {


            List<E621json> responses = e621.Get_Posts(tags.Taghelper(), 2);

            List<string> urls = GetUrls(responses);
            if (urls.Count > 0)
            {

                await bot.SendPhotoAsync(messagae.Message.Chat.Id, urls.Pick());
            }



        }

        private static List<string> GetUrls(List<E621json> responses)
        {
            List<string> urls = new();
            foreach (var response in responses)
            {

                Parallel.ForEach(response.posts, post =>
                {

                    int SizeinMb = ((int)(post.file.size / 1e+6));
                    if (post.file.url != null & SizeinMb < 5)
                    {
                        lock (lockMe)
                        {
                            urls.Add(post.file.url);
                        }
                    }

                });
            }
            return urls;
        }

        internal static async Task Inline(Api e621, InlineQueryEventArgs message, TelegramBotClient bot)
        {
            List<InlineQueryResultBase> results = new();

            string tags = message.InlineQuery.Query.Taghelper();

            List<E621json> response = e621.Get_Posts(tags, 2);


            List<string> urls = GetUrls(response);

            Parallel.ForEach(urls.Skip(0).Take(30), (iurl) =>
            {
                if (iurl != null)
                {

                    try
                    {
                        results.Add(new InlineQueryResultPhoto(Helper.RandomName(), iurl, iurl));
                    }
                    catch
                    {

                    }


                }
            });

            await bot.AnswerInlineQueryAsync(message.InlineQuery.Id, results, isPersonal: true, cacheTime: 5);
        }

        internal static async Task SendGroup(Api e621, string tags, MessageEventArgs messagae, TelegramBotClient bot)
        {
            List<InputMediaPhoto> photos = new();


            List<E621json> responses = e621.Get_Posts(tags.Taghelper(), 2);
            List<string> urls = GetUrls(responses);
            Parallel.ForEach(urls.Take(10), url => { photos.Add(new InputMediaPhoto(new InputMedia(url.Convert2Memory(), Helper.RandomName()))); });
            if (urls.Count > 0)
            {
                await bot.SendMediaGroupAsync(chatId: messagae.Message.Chat.Id, media: photos);
            }
            else
            {
 
            }
        }
        


        internal static async Task SendComic(Api e621,string tags,MessageEventArgs message,TelegramBotClient bot) 
        {
            List<InputMediaPhoto> photos = new();
            List<E621pools> Pool = await e621.Get_Pool(tags);
            IEnumerable<int> list = Pool[0].post_ids.Reverse().Take(10);

            List<string> urls = new();
           
                Parallel.ForEach( list.AsParallel().AsOrdered(), async post_id => 
            {
                Singlepost Post = await e621.Get_Id(post_id);
                string url = Post.post.file.url;
                lock (lockMe) 
                {
                    urls.Add(url);
                    photos.Add(new InputMediaPhoto(new InputMedia(content: url.Convert2Memory(), fileName: Helper.RandomName())));
           
                }



            });
            if (urls.Count > 0)
            {
                await bot.SendMediaGroupAsync(chatId: message.Message.Chat.Id, media: photos);
            }
            else
            {

            }
        }
        
    }
}
