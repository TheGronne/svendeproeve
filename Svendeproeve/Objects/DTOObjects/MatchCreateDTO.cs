using Svendeproeve.GameHubs.Classes;

namespace Svendeproeve.Objects.DTOObjects
{
    public class MatchCreateDTO
    {
        public List<Player> Players { get; set; }
        public int WinnerId { get; set; }

        public MatchCreateDTO(List<Player> players, int winnerId)
        {
            Players = players;
            WinnerId = winnerId;
        }
    }
}
