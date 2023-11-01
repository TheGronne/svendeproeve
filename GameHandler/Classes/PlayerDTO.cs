namespace GameHandler.Classes
{
    public class PlayerDTO
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public bool IsReady { get; set; }

        public PlayerDTO(Player player)
        {
            ID = player.DBID;
            Username = player.Name;
            IsReady = player.IsReady;
        }

        public static List<PlayerDTO> PlayerListToDTO(List<Player> players)
        {
            var playerDtos = new List<PlayerDTO>();
            players.ForEach(p => playerDtos.Add(new PlayerDTO(p)));
            return playerDtos;
        }
    }
}
