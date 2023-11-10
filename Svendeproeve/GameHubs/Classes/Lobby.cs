using System.Linq;

namespace Svendeproeve.GameHubs.Classes
{
    public static class Lobby
    {
        static List<Player> players = new List<Player>();

        public static void AddPlayer(Player player)
        {
            if (players.Count >= 4)
                return;

            players.Add(player);
        }

        public static void RemovePlayer(string serverId)
        {
            if (players.Count <= 0)
                return;

            var removedPlayer = players.Find(p => p.ServerID == serverId);
            players.Remove(removedPlayer);
        }

        public static void ChangeReadyState(string serverId, bool rdy)
        {
            var player = players.Find(p => p.ServerID == serverId);
            player.IsReady = rdy;
        }

        public static Player GetPlayer(string serverId)
        {
            return players.Find(p => p.ServerID == serverId);
        }

        public static bool DoesPlayerExist(string serverId)
        {
            return players.Any(p => p.ServerID == serverId);
        }

        public static List<Player> GetAllPlayers()
        {
            return players;
        }

        public static void AddWin(string serverId)
        {
            players.Find(p => p.ServerID == serverId).Wins++;
        }

        public static void ChangeIsAlive(string serverId, bool isAlive)
        {
            players.Find(p => p.ServerID == serverId).IsAlive = isAlive;
        }

        public static List<Player> GetAlivePlayers()
        {
            return players.FindAll(p => p.IsAlive == true);
        }

        public static int GetWinsNecessary()
        {
            return 7 - players.Count;
        }
    }
}
