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

        private bool ValidateUser()
        {
            return Lobby.DoesPlayerExist(Context.ConnectionId);
        }
    }
}
