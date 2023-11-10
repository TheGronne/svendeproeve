using GameHandler.Classes;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace GameHandler.GameHubs
{
    public class GameHub : Hub
    {
        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected");
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            await Clients.All.SendAsync("Disconnected", Lobby.GetPlayer(Context.ConnectionId).DBID);
            Lobby.RemovePlayer(Context.ConnectionId);
        }

        public async Task HandShake(int playerID, string username)
        {
            Lobby.AddPlayer(new Player(Context.ConnectionId, playerID, username));
            var players = Lobby.GetAllPlayers();
            var playerDtos = PlayerDTO.PlayerListToDTO(players);

            await Clients.All.SendAsync("HandShake", JsonSerializer.Serialize(playerDtos));
        }

        public async Task SendInput(int input)
        {
            if (!ValidateUser())
                return;

            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.All.SendAsync("ReceiveInput", player.DBID, input);
        }

        public async Task SendMousePos(decimal x, decimal y)
        {
            if (!ValidateUser())
                return;

            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.All.SendAsync("ReceiveMousePos", player.DBID, x, y);
        }

        public async Task SendReady(bool rdy)
        {
            if (!ValidateUser())
                return;

            Lobby.ChangeReadyState(Context.ConnectionId, rdy);
            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.All.SendAsync("ChangeReady", player.DBID, player.IsReady);
        }

        public async Task StartGame()
        {
            await Clients.All.SendAsync("StartGame");
        }

        public async Task SendRotation(float rotation)
        {
            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveRotation", player.DBID, rotation);
        }

        public async Task SendPlayerPosition(float posX, float posY)
        {
            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceivePlayerPosition", player.DBID, posX, posY);
        }

        public async Task SendMousePosition(float angle)
        {
            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveMousePosition", player.DBID, angle);
        }

        public async Task SendBulletSpawn(float posX, float posY, float angle)
        {
            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveBulletSpawn", player.DBID, posX, posY, angle);
        }

        public async Task SendBulletDestroy(int bulletId)
        {
            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveBulletDestroy", player.DBID, bulletId);
        }

        public async Task SendPlayerHit()
        {
            Lobby.ChangeIsAlive(Context.ConnectionId, false);
            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceivePlayerHit", player.DBID);

            if (Lobby.GetAlivePlayers().Count <= 1)
            {
                var winningPlayer = Lobby.GetAlivePlayers()[0];
                winningPlayer.Wins++;

                if (winningPlayer.Wins >= Lobby.GetWinsNecessary())
                    await Clients.All.SendAsync("EndGame", winningPlayer.DBID);

                var players = Lobby.GetAllPlayers();
                foreach (var item in players)
                {
                    item.IsAlive = true;
                }
                await Clients.All.SendAsync("StartRound", winningPlayer.DBID, winningPlayer.Wins, GetLevelInteger());
            }
        }

        private bool ValidateUser()
        {
            return Lobby.DoesPlayerExist(Context.ConnectionId);
        }

        private int GetLevelInteger()
        {
            Random rnd = new Random();
            return rnd.Next(0, 7);
        }
    }
}
