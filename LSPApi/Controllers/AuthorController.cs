using LSPApi.DataLayer;

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
    public async Task<Model.AuthorDto> GetById(int id) => await _author.GetById(id);


    [HttpGet("GetAll")]
    public async Task<List<Model.AuthorListResultsModel>?> GetAll() => await _author.GetAll();


    [HttpGet("gridsearch")]
    public async Task<List<Model.AuthorListResultsModel>> GetAuthors(int startRow, int endRow, string sortColumn, string sortDirection, string filter = "") =>
        await _author.GetAuthors(startRow, endRow, sortColumn, sortDirection, filter);


    [HttpGet, Route("{username}/{password}")]
    public async Task<Model.AuthorDto> GetByUsernamePassword(string username, string password) =>
        await _author.GetByUsernamePassword(username, password);

    [HttpGet, Route("delete/{id:int}")]
    public async Task Delete(int id)
    {
        await _book.DeleteByAuthorId(id);
        await _author.Delete(id);
    }

    [HttpPost, Route("search")]
    public async Task<List<Model.AuthorListResultsModel>?> GetSearch([FromBody] Model.AuthorSearchModel? s)
    {
        List<Model.AuthorListResultsModel>? result = [];

        s.LastName = string.IsNullOrEmpty(s.LastName) ? "" : s.LastName;
        s.FirstName = string.IsNullOrEmpty(s.FirstName) ? "" : s.FirstName;
        s.SortOrder = string.IsNullOrEmpty(s.SortOrder) ? "LastName" : s.SortOrder;
        s.Direction = string.IsNullOrEmpty(s.Direction) ? "ASC" : s.Direction;

        string key = $"{s.LastName,20}{s.FirstName,20}{s.SortOrder,20}{s.Direction,20}";


        return await _author.GetBySearchTerm(s);

    }


    [HttpPost, Route("add")]
    public async Task<IActionResult> Insert([FromBody] Model.AuthorDto author)
    {
        if (string.IsNullOrEmpty(author.Username)) return BadRequest();
        if (string.IsNullOrEmpty(author.FirstName)) return BadRequest();
        if (string.IsNullOrEmpty(author.LastName)) return BadRequest();
        if (string.IsNullOrEmpty(author.Email)) return BadRequest();

        var duplicate = await _author.CheckForUsername(author.Username);

        if (duplicate) return BadRequest();

        await _author.Add(author);

        return Ok();
    }

    [HttpPost, Route("update")]
    public async Task Update([FromBody] Model.AuthorDto author) => await _author.Update(author);

}

