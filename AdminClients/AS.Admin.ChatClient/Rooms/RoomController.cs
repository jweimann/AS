using Akka.Actor;
using AS.Messages;
using System;
using System.Collections.ObjectModel;

namespace AS.Admin.ChatClient
{
    public class RoomController : ClientControllerBase
    {
        public Action<UserList> OnUserListReceived { get; set; }
        public Action<UserJoinedRoom> OnUserJoined { get; set; }
        public Action<Chat> OnChat { get; set; }
        public Action<UserCreated> OnUserCreated { get; set; }
        public Action<UserLeftRoom> OnUserLeft { get; set; }

        private RoomViewModel _roomViewModel;

        public IActorRef ClientRoom { get; private set; }

        public RoomController(IUntypedActorContext context, ref RoomViewModel roomViewModel) : base(context.System)
        {
            _roomViewModel = roomViewModel;
            _roomViewModel.OnSendChat += message =>
                SendChat(message);

            OnUserListReceived += message => 
                _roomViewModel.Users = new ObservableCollection<string>(message.Usernames);
            OnUserJoined += message =>
                _roomViewModel.Users.Add(message.Username);
            OnUserLeft += message =>
                _roomViewModel.Users.Remove(message.Username);
            OnChat += message => 
                _roomViewModel.ChatText += message.Username + ": " + message.Text + Environment.NewLine;



            ClientRoom = context.ActorOf(
                Props.Create(() => new ClientRoom(this))
                 .WithDispatcher("akka.actor.synchronized-dispatcher") //dispatch on GUI thread
                ,"ClientRoom");
        }

        

        internal void DoStuff()
        {
            ClientRoom.Tell("Hello");
        }

        internal void SendChat(string p)
        {
            _myUserConnection.Tell(new Chat(ClientRoom, p));
        }
    }
}
