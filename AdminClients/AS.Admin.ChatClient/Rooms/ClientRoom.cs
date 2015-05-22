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
        private Action<UserLeftRoom> _onUserLeft;

        public ClientRoom(RoomController roomController)// Action<UserList> onUserListReceived, Action<UserJoinedRoom> onUserJoined, Action<Chat> onChat, Action<UserCreated> onUserCreated)
        {
            _onChat = roomController.OnChat;
            _onUserListReceived = roomController.OnUserListReceived;
            _onUserJoined = roomController.OnUserJoined;
            _onUserLeft = roomController.OnUserLeft;

            Become(Authenticated);
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

            Receive<UserLeftRoom>(message =>
            {
                if (_onUserLeft != null)
                    _onUserLeft(message);
            });

            Receive<Chat>(message =>
                {
                if (_onChat != null)
                    _onChat(message);
                });
        }

    }
}
