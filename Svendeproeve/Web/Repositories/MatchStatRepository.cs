using Svendeproeve.Objects.DomainObjects;
using Svendeproeve.SQLRepo;
using Svendeproeve.Objects.DTOObjects;
using System.Text;

namespace Svendeproeve.Repositories
{
    public class MatchStatRepository : IMatchStatRepository
    {
        private ISQLProvider provider;
        public MatchStatRepository(ISQLProvider _provider)
        {
            provider = _provider;

            if (provider == null)
                throw new ArgumentNullException(nameof(ISQLProvider));
        }

        public void Create(MatchStatCreateDTO matchDTO)
        {
            foreach (var player in matchDTO.Players)
            {
                string query = $"INSERT INTO MatchStats (MatchID, PlayerID, Kills, Deaths) VALUES ({matchDTO.MatchId}, {player.DBID}, {player.Kills}, {player.Deaths});";
                provider.Create(query);
            }
        }

        public List<MatchStatDTO> GetMatchStats(int playerId)
        {
            string query = "SELECT m.ID AS ID, u.Username AS Player1, i.Username AS Player2, o.Username AS Player3, p.Username AS Player4, " +
                                "a.Kills AS Kills, a.Deaths AS Deaths, q.Username AS Winner " +
                            "FROM Matches m " +
                                "LEFT JOIN Users u ON u.ID = m.Player1 " +
                                "LEFT JOIN Users i ON i.ID = m.Player2 " +
                                "LEFT JOIN Users o ON o.ID = m.Player3 " +
                                "LEFT JOIN Users p ON p.ID = m.Player4 " +
                                "LEFT JOIN MatchStats a ON a.MatchID = m.ID " +
                                "LEFT JOIN Users q ON q.ID = m.Winner " +
                            "WHERE( " +
                                $"u.id = {playerId} " +
                                $"OR i.ID = {playerId} " +
                                $"OR o.ID = {playerId} " +
                                $"OR p.ID = {playerId}) " +
                            $"AND a.PlayerID = {playerId}";

            var result = provider.Read(query);

            return ConvertToMatchStatDTO(result);
        }

        public List<MatchStatDTO> ConvertToMatchStatDTO(dynamic reader)
        {
            var entity = new List<MatchStatDTO>();
            while (reader.Read())
            {
                List<string> resultList = new List<string>();
                try
                {
                    resultList.Add((string)reader["Player1"]);
                    resultList.Add((string)reader["Player2"]);
                    resultList.Add((string)reader["Player3"]);
                    resultList.Add((string)reader["Player4"]);

                } catch(Exception ex)
                {

                }

                int kills = (int)reader["Kills"];
                int deaths = (int)reader["Deaths"];
                string winner = (string)reader["Winner"];

                entity.Add(new MatchStatDTO(resultList, kills, deaths, winner));
            }

            return entity;
        }
    }
}
