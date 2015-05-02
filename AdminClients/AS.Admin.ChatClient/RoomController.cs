using Akka.Actor;
using AS.Messages;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AS.Admin.ChatClient
{
    public class RoomController
    {
        private Action<UserList> OnUserListReceived;
        private Action<UserJoinedRoom> OnUserJoined;
        private Action<Chat> OnChat;

        private ActorSelection _connectionManager;
        private IActorRef _clientRoom;
        private RoomViewModel _roomViewModel;
        public ActorSystem Sys { get; set; }
        public RoomController(ActorSystem system, ref RoomViewModel roomViewModel)
        {
            _roomViewModel = roomViewModel;
            this.Sys = system;

            OnUserListReceived += HandleUserListReceived;
            OnUserJoined += HandleUserJoined;
            OnChat += HandleChat;

            _connectionManager = Sys.ActorSelection("akka.tcp://as@localhost:8081/user/ConnectionManager");

            _clientRoom = Sys.ActorOf(
                Props.Create(() => new ClientRoom(OnUserListReceived, OnUserJoined, OnChat))
                 .WithDispatcher("akka.actor.synchronized-dispatcher") //dispatch on GUI thread
                ,"dispatcher");
            
        }

        private void HandleChat(Chat obj)
        {
            _roomViewModel.ChatText += obj.Username + ": " + obj.Text + Environment.NewLine;
        }

        private void HandleUserJoined(UserJoinedRoom obj)
        {
            _roomViewModel.Users.Add(obj.Username);
        }

        private void HandleUserListReceived(UserList obj)
        {
            _roomViewModel.Users = new ObservableCollection<string>(obj.Usernames);
            //MessageBox.Show("Hello");
        }

        internal void DoStuff()
        {
            _clientRoom.Tell("Hello");
        }

        internal void SendChat(string p)
        {
            _clientRoom.Tell(new Chat(_clientRoom, p));
        }
    }
}
