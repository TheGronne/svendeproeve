namespace Svendeproeve.Objects.DomainObjects
{
    public class Match
    {
        public List<int?> Players { get; set; }
        public int WinnerID { get; set; }
        public int MatchId { get; set; }

        public Match(List<int?> playerIds, int winnerId, int matchId)
        {
            Players = playerIds;
            WinnerID = winnerId;
            MatchId = matchId;
        }

        public Match() { }
    }
}
