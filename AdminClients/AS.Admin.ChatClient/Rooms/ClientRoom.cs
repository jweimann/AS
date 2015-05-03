using Akka.Actor;
using AS.Messages;
using System;

namespace AS.Admin.ChatClient
{
    public class ClientRoom : ClientUIActorBase // actually a user in chat.. should rename
    {
        private Action<UserList> _onUserListReceived;
        private Action<UserJoinedRoom> _onUserJoined;
        private Action<Chat> _onChat;

        public ClientRoom(Action<UserList> onUserListReceived, Action<UserJoinedRoom> onUserJoined, Action<Chat> onChat, Action<UserCreated> onUserCreated)
        {
            _onChat = onChat;
            _onUserListReceived = onUserListReceived;
            _onUserJoined = onUserJoined;
        }
    

        private void Authenticated()
        {
            Receive<string>(message =>
            {
                if (_myUserConnection != null)
                    _myUserConnection.Tell(new JoinRoom(Self, "Chat1"));
                Console.Write(message);
            });

            Receive<UserList>(message =>
            {
                if (_onUserListReceived != null)
                    _onUserListReceived(message);
                Console.WriteLine(message);
            });

            Receive<UserJoinedRoom>(message =>
            {
                if (_onUserJoined != null)
                    _onUserJoined(message);
                Console.WriteLine(message);
            });

            Receive<Chat>(message =>
                {
                    if (Sender.Path.Equals(_myUserConnection.Path))
                    {
                        if (_onChat != null)
                            _onChat(message);
                    }
                    else
                    {
                        _myUserConnection.Tell(message);
                    }
                });
        }

    }
}
