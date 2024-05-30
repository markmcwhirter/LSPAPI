using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using Model = LSPApi.DataLayer.Model;

namespace LSPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinkController : ControllerBase
    {
        private readonly ILinkRepository _Link;


        public LinkController(ILinkRepository Link)
        {
            _Link = Link;
        }


        [HttpGet, Route("{id:int}")]
        public async Task<Model.LinkDto> GetById(int id) => await _Link.GetById(id);

        [HttpGet]
        public async Task<IEnumerable<Model.LinkDto>> GetAll() => await _Link.GetAll();

        [HttpPost]
        public async Task Insert([FromBody] Model.LinkDto Link) => await _Link.Add(Link);
    }
}
