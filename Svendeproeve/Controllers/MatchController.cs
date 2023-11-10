using Microsoft.AspNetCore.Mvc;
using Svendeproeve.Repositories;
using Svendeproeve.DTOObjects;
using Svendeproeve.DomainObjects;
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
        public MatchController(IMatchRepository _matchRepository)
        {
            matchRepository = _matchRepository;

            if (matchRepository == null)
                throw new ArgumentNullException(nameof(IMatchRepository));
        }

        // GET api/<UserController>/id=5
        [HttpGet]
        public string Get(int id)
        {
            return "YUHUHUU";
        }

        // GET api/<UserController>/
        [HttpPost]
        public IActionResult Create([FromBody] MatchCreateDTO matchDto)
        {
            try
            {
                matchRepository.Create(matchDto);
                var match = matchRepository.GetLatest(matchDto);
                return Ok();
            } catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
