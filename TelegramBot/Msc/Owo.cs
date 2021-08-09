using Telegram.Bot.Args;

namespace TelegramBot.Msc
{
    public class Owo
    {
        public long Chat_id {get;}
        public string User  { get; }
        public string Command { get; }
        public string Check { get; }
        public string Text { get; }
        internal  Owo (MessageEventArgs message)
            {
            this.Chat_id = message.Message.Chat.Id;
            this.User = message.Message.Chat.FirstName;
            this.Text = message.Message.Text;
            if (Text != null)
            {
                this.Command = message.Message.Text.Split(' ')[0];
            }
            else 
            {
                this.Command = null;
            }
            this.Check= message.Message.Type.ToString();


        }
    
    }
}
