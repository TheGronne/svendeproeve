namespace Svendeproeve.Objects.DTOObjects
{
    public class MatchStatDTO
    {
        public List<string> PlayerNames { get; set; }
        public int Kills { get; set; }
        public int Deaths { get; set; }
        public string Winner { get; set; }

        public MatchStatDTO(List<string> playerNames, int kills, int deaths, string winner)
        {
            PlayerNames = playerNames;
            Kills = kills;
            Deaths = deaths;
            Winner = winner;
        }

        public MatchStatDTO()
        {

        }
    }
}
