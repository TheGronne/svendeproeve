namespace Svendeproeve.DomainObjects
{
    public class Match
    {
        public List<int?> Players { get; set; }
        public int WinnerID { get; set; }

        public Match(List<int?> playerIds, int winnerId)
        {
            Players = playerIds;
            WinnerID = winnerId;
        }

        public Match() { }
    }
}
