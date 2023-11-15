using Svendeproeve.Objects.DomainObjects;
using Svendeproeve.Objects.DTOObjects;

namespace Svendeproeve.Repositories
{
    public interface IMatchRepository
    {
        public void Create(MatchCreateDTO matchDTO);
        public Match GetLatest(MatchCreateDTO matchDTO);
    }
}
