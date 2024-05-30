using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using Model = LSPApi.DataLayer.Model;

namespace LSPApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{

    private readonly IBookRepository _Book;

    public BookController( IBookRepository Book)
    {
        _Book = Book;
    }


    [HttpGet, Route("{id:int}")]
    public async Task<Model.BookDto> GetById(int id) => await _Book.GetById(id);


    [HttpGet, Route("author/{id:int}")]
    public async Task<List<Model.BookSummaryModel>> GetByAuthorId(int id) => await _Book.GetByAuthorId(id);

    [HttpGet]
    public async Task<IEnumerable<Model.BookDto>> GetAll() => await _Book.GetAll();

    [HttpPost]
    public async Task Insert([FromBody] Model.BookDto Book)
    {
        Book.DateCreated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
        await _Book.Add(Book);
    }

    [HttpPost, Route("update")]
    public async Task Update([FromBody] Model.BookDto Book)
    {
        try
        {
            if (Book.BookID == 0)
            {
                Book.DateCreated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
                await _Book.Add(Book);
            }
            else
            {
                Book.DateUpdated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
                await _Book.Update(Book);
            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }
    }

    [HttpGet, Route("delete/{id:int}")]
    public async Task Delete(int id)
    {
        try
        {
            await _Book.Delete(id);
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }

    }

}
