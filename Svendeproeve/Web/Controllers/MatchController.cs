using Microsoft.AspNetCore.Mvc;
using Svendeproeve.Repositories;
using Svendeproeve.Objects.DTOObjects;
using Svendeproeve.Objects.DomainObjects;
using System.Net;
using Svendeproeve.GameHubs.Classes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Svendeproeve.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : ControllerBase
    {
        private readonly IMatchRepository matchRepository;
        private readonly IMatchStatRepository matchStatRepository;

        public MatchController(IMatchRepository _matchRepository, IMatchStatRepository _matchStatRepository)
        {
            matchRepository = _matchRepository;
            matchStatRepository = _matchStatRepository;

            if (matchRepository == null)
                throw new ArgumentNullException(nameof(IMatchRepository));
            if (matchStatRepository == null)
                throw new ArgumentNullException(nameof(IMatchStatRepository));
        }

        // POST api/Match/
        [HttpPost]
        public IActionResult Create([FromBody] MatchCreateDTO matchDto)
        {
            try
            {
                matchRepository.Create(matchDto);
                var match = matchRepository.GetLatest(matchDto);
                matchStatRepository.Create(new MatchStatCreateDTO(matchDto, match.MatchId));
                return Ok();
            } catch (Exception ex)
            {
                throw ex;
            }
        }

        // GET api/Match
        [HttpGet]
        public List<MatchStatDTO> GetStats(int id)
        {
            try
            {
                return matchStatRepository.GetMatchStats(id);
            } catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
