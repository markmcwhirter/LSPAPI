using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using model = LSPApi.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;


namespace LSPApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController : ControllerBase
{
    private readonly IAuthorRepository _author;
    private readonly IBookRepository _book;
    private readonly IMemoryCache _cache;

    public AuthorController(IAuthorRepository author, IBookRepository book, IMemoryCache cache)
    {
        _author = author;
        _book = book;
        _cache = cache;
    }


    [HttpGet, Route("{id:int}")]
    public async Task<model.AuthorDto> GetById(int id) => await _author.GetById(id);


    [HttpGet("gridsearch")]
    public async Task<List<model.AuthorListResultsModel>> GetAuthors(int startRow, int endRow, string sortColumn, string sortDirection)
    {
        List<model.AuthorListResultsModel> result = new();

        try
        {
            sortColumn = sortColumn  == "null" ? "lastName" : sortColumn;
            sortDirection = sortDirection == "null" ? "ASC" : sortDirection;

            string key = $"{sortColumn.PadLeft(20)}{sortDirection.PadLeft(20)}{startRow.ToString().PadLeft(20)}{endRow.ToString().PadLeft(5)}";


            if (!_cache.TryGetValue(key, out result))
            {
                result = await _author.GetAuthors(startRow, endRow, sortColumn, sortDirection);

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));

                _cache.Set(key, result, cacheEntryOptions);
            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }


        return result;
    }


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
            _ = ex.Message;
        }

    }

    [HttpPost, Route("search")]
    public async Task<List<model.AuthorListResultsModel>> GetSearch([FromBody] model.AuthorSearchModel s)
    {
        List<model.AuthorListResultsModel> result = new();

        try
        {
            string key = $"{s.LastName.PadLeft(20)}{s.FirstName.PadLeft(20)}{s.SortOrder.PadLeft(20)}{s.Direction.PadLeft(5)}";


            if (!_cache.TryGetValue(key, out result))
            {
                result = await _author.GetBySearchTerm(s);

                MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromHours(1));

                _cache.Set(key, result, cacheEntryOptions);
            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
        return result;
    }


    [HttpPost]
    public async Task<IActionResult> Insert([FromBody] model.AuthorDto author)
    {

        if ( string.IsNullOrEmpty(author.Username) ) return BadRequest();
        if (string.IsNullOrEmpty(author.FirstName)) return BadRequest();
        if (string.IsNullOrEmpty(author.LastName)) return BadRequest();
        if (string.IsNullOrEmpty(author.Email)) return BadRequest();

        var duplicate = await _author.CheckForUsername(author.Username);

        if (duplicate) return BadRequest();

        // TODO: encrypt password here
        await _author.Add(author);

        return Ok();
    }

    [HttpPost, Route("update")]
    public async Task<IActionResult> Update([FromBody] model.AuthorDto author)
    {

        //if (string.IsNullOrEmpty(author.Username)) return BadRequest();
        //if (string.IsNullOrEmpty(author.FirstName)) return BadRequest();
        //if (string.IsNullOrEmpty(author.LastName)) return BadRequest();
        //if (string.IsNullOrEmpty(author.Email)) return BadRequest();

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

