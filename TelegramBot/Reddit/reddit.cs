using Reddit;
using Reddit.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Reddit.Exceptions;
using TelegramBot.Msc;
namespace TelegramBot.Reddit
{
   public static class reddit
    {
        private static readonly ApiKeys keys = new();
        static RedditClient r = new RedditClient(appId: keys.appId, appSecret: keys.appSecret, userAgent: keys.useragent, refreshToken: keys.refreshToken);
        private static object lockMe;

        internal static async Task Grab_Reddit(TelegramBotClient bot, string subreddit,MessageEventArgs message)
        {
            await bot.SendChatActionAsync(chatId: message.Message.Chat.Id, chatAction: ChatAction.UploadPhoto);
            List<InputMediaPhoto> photos = new();
            List<string> Urls = GrabPosts(r, subreddit.Trim());
            if (Urls != null)
            {

                Parallel.ForEach(Urls.Take(10), url =>


                {

                    {

                        photos.Add(new InputMediaPhoto(new InputMedia(content: url.Convert2Memory(), fileName: Helper.RandomName())));

                    }


                });
                if (Urls.Count > 0)

                {
                    await bot.SendMediaGroupAsync(chatId: message.Message.Chat.Id, media: photos);

                }
                else
                {

                }
            }
            else { Console.WriteLine("Someone Fucked UP"); }




        }


        private static List<string> GrabPosts(RedditClient r, string subreddit)
        {
            Regex rx = new Regex(@".*\.(jpg|png)?$");
            List<string> Url = new List<string> { };
            try
            {
                Subreddit subs = r.Subreddit(subreddit);
                List<Post> Posts = subs.Posts.Top;
                foreach (Post post in Posts.Take(15))
                {

                    Post info = post.About();

                    string url = post.Listing.URL;
                    Console.WriteLine(url);
                    if (rx.IsMatch(url))
                    {
                        Url.Add(url);
                    }
                }
                Console.WriteLine("Done");
                return Url;
            }
            catch (RedditForbiddenException)
            {
                return null;
                
            }
            
            

        }
    }
}
