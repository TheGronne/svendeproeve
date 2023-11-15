using Svendeproeve.Objects.DomainObjects;
using Svendeproeve.Objects.DTOObjects;

namespace Svendeproeve.Repositories
{
    public interface IMatchStatRepository
    {
        public void Create(MatchStatCreateDTO matchDTO);
        public List<MatchStatDTO> GetMatchStats(int playerId);
    }
}
