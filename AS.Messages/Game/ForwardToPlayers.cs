namespace AS.Messages
{
    public class ForwardToPlayers
    {
        public object Message { get; private set; }
        public ForwardToPlayers(object message)
        {
            Message = message;
        }
    }
}
