using Svendeproeve.DomainObjects;
using Svendeproeve.DTOObjects;

namespace Svendeproeve.Repositories
{
    public interface IMatchRepository
    {
        public void Create(MatchCreateDTO matchDTO);
        public Match GetLatest(MatchCreateDTO matchDTO);
    }
}
