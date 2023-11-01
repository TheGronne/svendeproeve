namespace GameHandler.Classes
{
    public class Player
    {
        public string ServerID { get; set; }
        public int DBID { get; set; }
        public string Name { get; set; }
        public bool IsReady { get; set; } = false;

        public Player(string serverId, int dbid, string name)
        {
            ServerID = serverId;
            DBID = dbid;
            Name = name;
            IsReady = false;
        }
    }
}
