using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using LogicLayer.Interfaces;
using LogicLayer.Services;

using Microsoft.AspNet.SignalR;

namespace Last.Hubs
{
    public class GameHub : Hub
    {
        ServiceCreator serviceCreator = new ServiceCreator();

        public IGameService GameService
        {
            get
            {
                return serviceCreator.CreateGameService();
            }
        }

        public static List<Player> Players = new List<Player>();

        // Отправка сообщений
        public void Send(string name, string message)
        {
            Clients.All.addMessage(name, message);
        }


        public void GetMyId()
        {
            Clients.Caller.setId(Context.ConnectionId);
        }

        public async Task AddToGroup(string groupName)
        {
            await Groups.Add(Context.ConnectionId, groupName);

            Players.Add(new Player()
            {
                ConnectionId = Context.ConnectionId,
                GameId = groupName
            });
            var gamePlayers = Players.Where(x => x.GameId == groupName).ToList();
            if (Players.Count(x => x.GameId == groupName) == 2)
            {
                string firstPlayer;
                int firstStep = new Random().Next(1, 100);
                if (firstStep % 2 == 0)
                    firstPlayer = gamePlayers[0].ConnectionId;
                else
                {
                    firstPlayer = gamePlayers[1].ConnectionId;
                }
                await Clients.Group(groupName).startGame(gamePlayers, firstPlayer);
            }
            await Clients.Group(groupName).addMessage(gamePlayers, $"подключился игрок");
        }

        public async Task SendSteps(string groupName, int[] fields)
        {
            Clients.Group(groupName).showSteps(fields);
            Clients.Group(groupName).winCheck();
        }

        // Отключение пользователя
        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var id = Context.ConnectionId;
            var player = Players.FirstOrDefault(x => x.ConnectionId == id);

            if (player != null)
            {
                var game = GameService.GetGameById(Int32.Parse(player.GameId));

                if (game != null)
                {

                    if (game.PlayersCount == 1)
                    {
                        GameService.DisconnectPlayer(Int32.Parse(player.GameId));
                        Players.Remove(Players.First(x => x.ConnectionId == id));
                    }

                    if (game.PlayersCount == 2)
                    {
                        try
                        {

                            GameService.DeleteGame(Int32.Parse(player.GameId));
                            Clients.Group(player.GameId).stopGame();
                            Players.RemoveAll(x => x.GameId == player.GameId);
                        }
                        catch (Exception e)
                        {

                        }
                    }
                }
            }
            return base.OnDisconnected(stopCalled);
        }
    }
}