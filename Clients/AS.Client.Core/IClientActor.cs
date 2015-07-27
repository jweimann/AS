using AS.Client.Messages;

namespace AS.Client.Core
{
    public interface IClientActor
    {
        void Tell(object message);
    }
}
