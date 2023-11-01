using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Net;
using GameHandler.GameHubs;
using GameHandler.Classes;

namespace GameHandler.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LobbyController : Controller
    {
        private IHubContext<GameHub> gameContext;
        public LobbyController(IHubContext<GameHub> _gameContext)
        {
            gameContext = _gameContext;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Add(int id)
        {
            gameContext.Clients.All.SendAsync("Joined", id);
            return Ok();
        }
    }
}
