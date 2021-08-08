using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace TelegramBot
{
    public static class Helper
    {

        public static string Taghelper(this string tag)
        {
            return tag.Replace(" ", "+").Replace(",", "+");

        }
        public static MemoryStream Convert2Memory(this string url)
        {
            using WebClient webClient = new();
            byte[] imageBytes = webClient.DownloadData(url);
            MemoryStream ms = new(imageBytes);
            return ms;
        }

        public static string Pick(this List<string> urls)
        {
            var Random = new Random();
            return urls[Random.Next(urls.Count)];

        }

        public static string RandomName()
        {
            var Random = new Random();
            string name = "";
            string[] alphabet = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "p", "q", "r", "s", "t", "u", "v", "w", "x", "y", "z" };
            for (int i = 0; i < 20; i++)
            {
                name += alphabet[Random.Next(alphabet.Length)];
            }
            return name;
        }
    }
}
