using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using model = LSPApi.DataLayer.Model;


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
        //public async Task<model.AuthorDto> GetById(int id) => await _author.GetById(id);
        public async Task<model.AuthorDto> GetById(int id)
        {
            var result = await _author.GetById(id);
            return result;
        }


        [HttpGet, Route("{username}/{password}")]
        public async Task<model.AuthorDto> GetByUsernamePassword(string username, string password)
        {
            model.AuthorDto status = new model.AuthorDto();

            try
            {
                status = await _author.GetByUsernamePassword(username, password);
            }
            catch(Exception ex)
            {
                var error = ex.Message;
            }
            return status;
        }

        [HttpPost, Route("search")]
        public async Task<List<model.AuthorListResultsModel>> GetSearch([FromBody] model.AuthorSearchModel searchterm)
        {
            List<model.AuthorListResultsModel> result = new List<model.AuthorListResultsModel>();

            try
            {
                result = await _author.GetBySearchTerm(searchterm);
            }
            catch (Exception ex)
            {
                var error = ex.Message;
            }
            return result;
        }

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

        [HttpPost, Route("update")]
        public async Task<IActionResult> Update([FromBody] model.AuthorDto author)
        {

            if (string.IsNullOrEmpty(author.Username)) return BadRequest();
            if (string.IsNullOrEmpty(author.FirstName)) return BadRequest();
            if (string.IsNullOrEmpty(author.LastName)) return BadRequest();
            if (string.IsNullOrEmpty(author.Email)) return BadRequest();

            author.DateUpdated = DateTime.Now.ToString();


            // hack to accomodate non-priveleged updates
            if (author.Admin == null || author.Password == null)
            {
                var result = await _author.GetById(author.AuthorID);
                if (author.Admin == null) author.Admin = result.Admin;
                if( author.Password == null) author.Password = result.Password;
            }


            await _author.Update(author);

            return Ok();
        }
    }
}
