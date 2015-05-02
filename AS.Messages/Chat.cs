using Akka.Actor;

namespace AS.Messages
{
    public class Chat
    {
        public string Text { get; private set; }

        public string Username { get; private set; }

        public IActorRef ChatActor { get; private set; }



        public Chat(IActorRef chatActor, string text, string username = "")
        {
            this.Text = text;
            this.ChatActor = chatActor;
            this.Username = username;
        }
    }
}