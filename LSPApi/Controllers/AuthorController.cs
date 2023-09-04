using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using model = LSPApi.DataLayer.Model;
using System.Net;
using Org.BouncyCastle.Asn1.Ocsp;

namespace LSPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly ILogger<AuthorController> _logger;
        private readonly IAuthorRepository _author;
        private readonly IConfiguration _configuration;

        public AuthorController(ILogger<AuthorController> logger, IConfiguration configuration, IAuthorRepository author)
        {
            _logger = logger;
            _author = author;
            _configuration = configuration;
        }


        [HttpGet, Route("{id:int}")]
        public async Task<model.AuthorDto> GetById(int id) => await _author.GetById(id);

        [HttpGet, Route("{username}/{password}")]
        public async Task<model.AuthorDto> GetByUsername(string username, string password) => await _author.GetByUsername(username, password);


        [HttpGet]
        public async Task<IEnumerable<model.AuthorDto>> GetAll() => await _author.GetAll();

        [HttpPost]
        public async Task<IActionResult> Insert([FromBody] model.AuthorDto author)
        {

            if ( string.IsNullOrEmpty(author.Username) ) return BadRequest();
            if (string.IsNullOrEmpty(author.FirstName)) return BadRequest();
            if (string.IsNullOrEmpty(author.LastName)) return BadRequest();
            if (string.IsNullOrEmpty(author.Email)) return BadRequest();

            var duplicate = await _author.CheckForUsername(author.Username);

            if (duplicate) return BadRequest();

            await _author.Add(author);

            return Ok();
        }
    }
}
