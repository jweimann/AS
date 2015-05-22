﻿using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using Akka.Actor;
using AS.Messages.Entities;
using AS.Messages.Game;
using System;
using AS.Messages;

namespace AS.Actors.GameActors
{
    public class Game : ReceiveActor, IWithUnboundedStash
    {
        private IActorRef _entityManager;
        private IActorRef _users;
        private Props _regionsProps;
        private IActorRef _regions;

        public List<IActorRef> Players { get; private set; } = new List<IActorRef>();
        public int MinimumPlayersToStart { get; private set; } = 1;
        private IActorRef Users
        {
            get
            {
                if (_users == null)
                    _users = Context.System.ActorSelection("/user/users").ResolveOne(TimeSpan.FromSeconds(5)).Result;
                return _users;
            }
        }

        public IStash Stash { get; set; }
        public Game()
        {
            Debug.WriteLine($"Game Spawned at path {Self.Path.ToString()}");
            _regionsProps = Props.Create<RegionManager>(100.0f, 3);
            Become(Initializing);
        }

        public Game(CreateGame createGameMessage)
        {
            Debug.WriteLine($"Game Spawned at path {Self.Path.ToString()}");
            _regionsProps = Props.Create<RegionManager>(100.0f, 3);
            Become(Initializing);
        }

        public Game(Props regionsProps)
        {
            _regionsProps = regionsProps;
            Become(Initializing);
        }

        private void Initializing()
        {
            _regions = Context.ActorOf(_regionsProps, "RegionManager");
            Debug.WriteLine($"Regions Path: {_regions.Path.ToString()}");

            _entityManager = Context.ActorOf<EntityManager>("EntityManager");

            Receive<GetGameState>(message => Sender.Tell("NotStarted"));

            Receive<JoinGame>(message =>
            {
                Players.Add(message.ActorRef);
                Sender.Tell(new JoinGameSuccess(Self));
            });

            Receive<StartGame>(message =>
            {
                if (Players.Count < MinimumPlayersToStart)
                    Sender.Tell("Failed to start, not enough players");
                else
                    Become(Playing);
                //Become(Starting);
            });

            ReceiveAny(message =>
                Stash.Stash()
            );
        }

        private void Starting()
        {
            Receive<GetGameState>(message => Sender.Tell("Starting"));
            ReceiveAny(message => {
                Debug.WriteLine($"{Self.Path.ToString()}Stashing: " + message.ToString());
                Stash.Stash();
            });
            Become(Playing);
        }

        private void Playing()
        {
            Debug.WriteLine($"{Self.Path.ToString()}Becoming Playing");
            Players.ForEach(t => t.Tell(new GameStarted()));
            Receive<EntityMessage>(message => _entityManager.Tell(message));
            Stash.UnstashAll();
            Debug.WriteLine($"{Self.Path.ToString()}Became Playing");

            Receive<ForwardToPlayers>(message => MessagePlayers(message.Message));
        }

        private void MessagePlayers(object message)
        {
            //ActorSelection playerSelection = new ActorSelection(Users, "*");//, Players.Select(t => t.Path.Elements.Last()));
            //ActorSelection playerSelection = Context.ActorSelection("/user/users");
            //Debug.WriteLine($"Messaging ActorSelection: {playerSelection.PathString}");

            //playerSelection.Tell(message);

            foreach (var player in Players)
                player.Tell(message, Sender);
        }
    }
}