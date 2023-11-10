using Svendeproeve.DomainObjects;
using Svendeproeve.SQLRepo;
using Svendeproeve.DTOObjects;
using System.Text;
using Svendeproeve.GameHubs.Classes;

namespace Svendeproeve.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private ISQLProvider provider;
        public MatchRepository(ISQLProvider _provider)
        {
            provider = _provider;

            if (provider == null)
                throw new ArgumentNullException(nameof(ISQLProvider));
        }

        public void Create(MatchCreateDTO matchDTO)
        {
            var query = new StringBuilder("INSERT INTO Matches (Player1, Player2, Player3, Player4, Winner) VALUES (");
            for (int i = 0; i < 4; i++)
            {
                if (i >= matchDTO.Players.Count)
                    query.Append(", NULL");
                else
                {
                    if (i == 0)
                        query.Append($"{matchDTO.Players[i].DBID}");
                    else
                        query.Append($", {matchDTO.Players[i].DBID}");
                }
            }
            query.Append($", {matchDTO.WinnerId}");
            query.Append(");");

            provider.Create(query.ToString());
        }
        

        public Match GetLatest(MatchCreateDTO matchDTO)
        {
            var query = new StringBuilder("SELECT ID, Player1, Player2, Player3, Player4, Winner FROM Matches WHERE ");
            for (int i = 0; i < matchDTO.Players.Count; i++)
            {
                if (i == 0)
                    query.Append($"Player{i + 1} = {matchDTO.Players[i].DBID}");
                else
                    query.Append($" AND Player{i + 1} = {matchDTO.Players[i].DBID}");
            }
            query.Append(" ORDER BY ID DESC LIMIT 1;");

            var result = provider.Read(query.ToString());
            var match =  ConvertToMatch(result);
            return match;
        }

        public User GetByPlayerID(int ID)
        {
            var reader = provider.Read($"SELECT ID, Username FROM Users WHERE ID = {ID}");
            return ConvertToMatch(reader);
        }

        public Match ConvertToMatch(dynamic reader)
        {
            List<int?> resultList = new List<int?>();
            Match entity = new Match();
            while (reader.Read())
            {
                try
                {
                    resultList.Add((int)reader["Player1"]);
                    resultList.Add((int)reader["Player2"]);
                    resultList.Add((int?)reader["Player3"]);
                    resultList.Add((int?)reader["Player4"]);

                } catch(Exception ex)
                {

                }

                int winner = (int)reader["Winner"];

                entity = new Match(resultList, winner);
            }

            return entity;
        }
    }
}
