namespace Svendeproeve.GameHubs.Classes
{
    public class Player
    {
        public string ServerID { get; set; }
        public int DBID { get; set; }
        public string Name { get; set; }
        public bool IsReady { get; set; } = false;
        public int Wins { get; set; } = 0;
        public bool IsAlive { get; set; } = true;
        public int Kills { get; set; } = 0;
        public int Deaths { get; set; } = 0;

        public Player(string serverId, int dbid, string name)
        {
            ServerID = serverId;
            DBID = dbid;
            Name = name;
        }
    }
}
