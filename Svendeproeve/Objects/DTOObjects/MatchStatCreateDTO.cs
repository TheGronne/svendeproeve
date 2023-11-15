using Svendeproeve.GameHubs.Classes;

namespace Svendeproeve.Objects.DTOObjects
{
    public class MatchStatCreateDTO
    {
        public List<Player> Players { get; set; }
        public int WinnerId { get; set; }
        public int MatchId { get; set; }

        public MatchStatCreateDTO(MatchCreateDTO match, int matchId)
        {
            Players = match.Players;
            WinnerId = match.WinnerId;
            MatchId = matchId;
        }
    }
}
