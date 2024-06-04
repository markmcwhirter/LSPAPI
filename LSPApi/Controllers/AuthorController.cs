using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using Model = LSPApi.DataLayer.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;


namespace LSPApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthorController(IAuthorRepository author, IBookRepository book, IMemoryCache cache) : ControllerBase
{
    private readonly IAuthorRepository _author = author;
    private readonly IBookRepository _book = book;
    private readonly IMemoryCache _cache = cache;

    [HttpGet, Route("{id:int}")]
    public async Task<Model.AuthorDto> GetById(int id) => await _author.GetById(id);

    [HttpGet("GetAll")]
    public async Task<List<Model.AuthorListResultsModel>?> GetAll()
    {
        List<Model.AuthorListResultsModel>? result = [];

        try
        {
            string key = $"GetAll";


            if (!_cache.TryGetValue(key, out result))
            {
                result = await _author.GetAll();

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


    [HttpGet("gridsearch")]
    public async Task<List<Model.AuthorListResultsModel>?> GetAuthors(int startRow, int endRow, string sortColumn, string sortDirection, string filter = "")
    {
        List<Model.AuthorListResultsModel>? result = [];

        try
        {
            sortColumn = sortColumn  == "null" ? "lastName" : sortColumn;
            sortDirection = sortDirection == "null" ? "ASC" : sortDirection;

            result = await _author.GetAuthors(startRow, endRow, sortColumn, sortDirection, filter);
        }
        catch (Exception ex)
        {
            _ = ex.Message;
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
    public async Task<List<Model.AuthorListResultsModel>?> GetSearch([FromBody] Model.AuthorSearchModel? s)
    {
        List<Model.AuthorListResultsModel>? result = [];

        try
        {
            s.LastName = string.IsNullOrEmpty(s.LastName) ?  "" : s.LastName;
            s.FirstName = string.IsNullOrEmpty(s.FirstName) ? "" : s.FirstName;
            s.SortOrder = string.IsNullOrEmpty(s.SortOrder) ? "LastName" : s.SortOrder;
            s.Direction = string.IsNullOrEmpty(s.Direction) ? "ASC" : s.Direction;

            string key = $"{s.LastName,20}{s.FirstName,20}{s.SortOrder,20}{s.Direction,20}";


            if (!_cache.TryGetValue(key, value: out result))
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


    [HttpPost, Route("add")]
    public async Task<IActionResult> Insert([FromBody] Model.AuthorDto author)
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
    public async Task<IActionResult> Update([FromBody] Model.AuthorDto author)
    {

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

