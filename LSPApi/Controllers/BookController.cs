using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using Model = LSPApi.DataLayer.Model;
using LSPApi.DataLayer.Model;


namespace LSPApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{

    private readonly IBookRepository _book;

    public BookController( IBookRepository Book)
    {
        _book = Book;
    }


    [HttpGet, Route("{id:int}")]
    public async Task<Model.BookDto> GetById(int id) => await _book.GetById(id);


    [HttpGet("gridsearch")]
    public async Task<List<Model.BookListResultsModel>?> GetBooks(int startRow, int endRow, string sortColumn, string sortDirection, string filter = "")
    {
        List<Model.BookListResultsModel>? result = [];

        try
        {


            result = await _book.GetBooks(startRow, endRow, sortColumn, sortDirection, filter);
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }


        return result;
    }

    [HttpGet, Route("author/{id:int}")]
    public async Task<List<Model.BookSummaryModel>> GetByAuthorId(int id) => await _book.GetByAuthorId(id);

    [HttpGet]
    public async Task<IEnumerable<Model.BookDto>> GetAll() => await _book.GetAll();

    [HttpPost]
    public async Task Insert([FromBody] Model.BookDto Book)
    {
        Book.DateCreated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
        await _book.Add(Book);
    }

    [HttpPost, Route("update")]
    public async Task Update([FromBody] Model.BookDto Book)
    {
        try
        {
            if (Book.BookID == 0)
            {
                Book.DateCreated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
                await _book.Add(Book);
            }
            else
            {
                Book.DateUpdated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
                await _book.Update(Book);
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
            await _book.Delete(id);
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }

    }

}
