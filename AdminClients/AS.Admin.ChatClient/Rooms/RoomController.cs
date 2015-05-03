using Akka.Actor;
using AS.Messages;
using System;
using System.Collections.ObjectModel;

namespace AS.Admin.ChatClient
{
    public class RoomController : ClientControllerBase
    {
        private Action<UserList> OnUserListReceived;
        private Action<UserJoinedRoom> OnUserJoined;
        private Action<Chat> OnChat;

        public Action<UserCreated> OnUserCreated { get; set; }

        private RoomViewModel _roomViewModel;
        private IActorRef _userConnectionActor;

        public IActorRef ClientRoom { get; private set; }

        public RoomController(ActorSystem system, ref RoomViewModel roomViewModel) : base(system)
        {
            _roomViewModel = roomViewModel;

            OnUserListReceived += obj => _roomViewModel.Users = new ObservableCollection<string>(obj.Usernames);
            OnUserJoined += obj => _roomViewModel.Users.Add(obj.Username);
            OnChat += obj => _roomViewModel.ChatText += obj.Username + ": " + obj.Text + Environment.NewLine;

            ClientRoom = Sys.ActorOf(
                Props.Create(() => new ClientRoom(OnUserListReceived, OnUserJoined, OnChat, OnUserCreated))
                 .WithDispatcher("akka.actor.synchronized-dispatcher") //dispatch on GUI thread
                ,"dispatcher");
        }

        internal void DoStuff()
        {
            ClientRoom.Tell("Hello");
        }

        internal void SendChat(string p)
        {
            ClientRoom.Tell(new Chat(ClientRoom, p));
        }
    }
}
