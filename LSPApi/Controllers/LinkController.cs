using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using model = LSPApi.DataLayer.Model;

namespace LSPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinkController : ControllerBase
    {
        private readonly ILogger<LinkController> _logger;
        private readonly ILinkRepository _Link;
        private readonly IConfiguration _configuration;

        public LinkController(ILogger<LinkController> logger, IConfiguration configuration, ILinkRepository Link)
        {
            _logger = logger;
            _Link = Link;
            _configuration = configuration;
        }


        [HttpGet, Route("{id:int}")]
        public async Task<model.LinkDto> GetById(int id) => await _Link.GetById(id);

        [HttpGet]
        public async Task<IEnumerable<model.LinkDto>> GetAll() => await _Link.GetAll();

        [HttpPost]
        public async Task Insert([FromBody] model.LinkDto Link) => await _Link.Add(Link);
    }
}
