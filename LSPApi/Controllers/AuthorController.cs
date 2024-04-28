using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using model = LSPApi.DataLayer.Model;


namespace LSPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorRepository _author;

        public AuthorController(IAuthorRepository author)
        {
            _author = author;
        }


        [HttpGet, Route("{id:int}")]
        public async Task<model.AuthorDto> GetById(int id) => await _author.GetById(id);

        [HttpGet, Route("{username}")]
        public bool CheckUsername(string username) =>
            _author.CheckUsername(username);

        [HttpGet, Route("{username}/{password}")]
        public async Task<model.AuthorDto> GetByUsernamePassword(string username, string password)
        {
            model.AuthorDto status = new();

            try
            {
                status = await _author.GetByUsernamePassword(username, password);
            }
            catch(Exception ex)
            {
                _ = ex.Message;
            }
            return status;
        }

        [HttpPost, Route("search")]
        public async Task<List<model.AuthorListResultsModel>> GetSearch([FromBody] model.AuthorSearchModel searchterm)
        {
            List<model.AuthorListResultsModel> result = new();

            try
            {
                result = await _author.GetBySearchTerm(searchterm);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }
            return result;
        }

        //[HttpGet]
        //public async Task<IEnumerable<model.AuthorDto>> GetAll() => await _author.GetAll();

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
                author.Admin ??= result.Admin;
                author.Password ??= result.Password;
            }


            await _author.Update(author);

            return Ok();
        }
    }
}
