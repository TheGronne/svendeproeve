using Svendeproeve.GameHubs.Classes;
using Svendeproeve.Objects.DTOObjects;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace Svendeproeve.GameHubs
{
    public class GameHub : Hub
    {
        static HttpClient client = new HttpClient();
        private static string serverAddress = "https://localhost:7019/api";
        private static string matchApi = "/match";

        public override async Task OnConnectedAsync()
        {
            await Clients.Caller.SendAsync("Connected");
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            await Clients.All.SendAsync("Disconnected", Lobby.GetPlayer(Context.ConnectionId).DBID);
            Lobby.RemovePlayer(Context.ConnectionId);
            await Clients.All.SendAsync("EndGame");
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

        public async Task SendPlayerState(byte[] playerState)
        {
            if (!ValidateUser())
                return;

            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceivePlayerState", playerState);
        }

        public async Task SendBulletSpawn(float posX, float posY, float angle)
        {
            if (!ValidateUser())
                return;

            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveBulletSpawn", player.DBID, posX, posY, angle);
        }

        public async Task SendBulletDestroy(int bulletId)
        {
            if (!ValidateUser())
                return;

            var player = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceiveBulletDestroy", player.DBID, bulletId);
        }

        //Killing player is the player who shot the bullet that killed the player
        public async Task SendPlayerHit(int killingPlayerId)
        {
            if (!ValidateUser())
                return;

            Lobby.ChangeIsAlive(Context.ConnectionId, false);
            var dyingPlayer = Lobby.GetPlayer(Context.ConnectionId);
            await Clients.AllExcept(Context.ConnectionId).SendAsync("ReceivePlayerHit", dyingPlayer.DBID);

            Lobby.GetPlayerByDBID(killingPlayerId).Kills += 1;
            dyingPlayer.Deaths += 1;

            if (Lobby.GetAlivePlayers().Count <= 1)
            {
                var winningPlayer = Lobby.GetAlivePlayers()[0];
                winningPlayer.Wins++;

                if (winningPlayer.Wins >= Lobby.GetWinsNecessary())
                {
                    await Clients.All.SendAsync("EndGame", winningPlayer.DBID);
                    await CreateMatch(winningPlayer.DBID);
                }

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

        private static async Task CreateMatch(int winnerId)
        {
            var players = Lobby.GetAllPlayers();

            HttpResponseMessage response = await client.PostAsJsonAsync(
                serverAddress + matchApi, new MatchCreateDTO(players, winnerId));
            response.EnsureSuccessStatusCode();

        }
    }
}
