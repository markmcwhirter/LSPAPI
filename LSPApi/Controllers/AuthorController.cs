using LSPApi.DataLayer;
using LSPApi.DataLayer.Model;

using Microsoft.AspNetCore.Mvc;

using Model = LSPApi.DataLayer.Model;


namespace LSPApi.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorRepository _author;
    private readonly IBookRepository _book;
    private readonly ILogger<AuthorController> _logger;

    public AuthorController(IAuthorRepository author, IBookRepository book, ILogger<AuthorController> logger)
    {
        _author = author;
        _book = book;
        _logger = logger;
    }

    [HttpGet, Route("{id:int}")]
    public async Task<Model.AuthorDto> GetById(int id)
    {
        try
        {            
            return await _author.GetById(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return new Model.AuthorDto();
        }
    }


    [HttpGet("GetAll")]
    public async Task<List<Model.AuthorListResultsModel>?> GetAll()
    {
        List<Model.AuthorListResultsModel>? result = [];

        try
        {
            return await _author.GetAll();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return new List<AuthorListResultsModel>();
        }

    }


    [HttpGet("gridsearch")]
    public async Task<List<Model.AuthorListResultsModel>> GetAuthors(int startRow, int endRow, string sortColumn, string sortDirection, string filter = "")
    {
        List<Model.AuthorListResultsModel>? result = [];

        try
        {
            result = await _author.GetAuthors(startRow, endRow, sortColumn, sortDirection, filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }


        return result;
    }


    [HttpGet, Route("{username}/{password}")]
    public async Task<Model.AuthorDto> GetByUsernamePassword(string username, string password)
    {
        Model.AuthorDto status = new();

        try
        {
            status = await _author.GetByUsernamePassword(username, password);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }
        return status;
    }
    [HttpGet, Route("delete/{id:int}")]
    public async Task Delete(int id)
    {
        try
        {
            await _book.DeleteByAuthorId(id);
            await _author.Delete(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

    }

    [HttpPost, Route("search")]
    public async Task<List<Model.AuthorListResultsModel>?> GetSearch([FromBody] Model.AuthorSearchModel? s)
    {
        List<Model.AuthorListResultsModel>? result = [];

        try
        {
            s.LastName = string.IsNullOrEmpty(s.LastName) ? "" : s.LastName;
            s.FirstName = string.IsNullOrEmpty(s.FirstName) ? "" : s.FirstName;
            s.SortOrder = string.IsNullOrEmpty(s.SortOrder) ? "LastName" : s.SortOrder;
            s.Direction = string.IsNullOrEmpty(s.Direction) ? "ASC" : s.Direction;

            string key = $"{s.LastName,20}{s.FirstName,20}{s.SortOrder,20}{s.Direction,20}";


            result = await _author.GetBySearchTerm(s);


        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }
        return result;
    }


    [HttpPost, Route("add")]
    public async Task<IActionResult> Insert([FromBody] Model.AuthorDto author)
    {
        try
        {
            if (string.IsNullOrEmpty(author.Username)) return BadRequest();
            if (string.IsNullOrEmpty(author.FirstName)) return BadRequest();
            if (string.IsNullOrEmpty(author.LastName)) return BadRequest();
            if (string.IsNullOrEmpty(author.Email)) return BadRequest();

            var duplicate = await _author.CheckForUsername(author.Username);

            if (duplicate) return BadRequest();

            await _author.Add(author);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok();
    }

    [HttpPost, Route("update")]
    public async Task<IActionResult> Update([FromBody] Model.AuthorDto author)
    {
        try
        {

            await _author.Update(author);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

        return Ok();
    }
}

