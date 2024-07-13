using LSPApi.DataLayer;
using LSPApi.DataLayer.Model;

using Microsoft.AspNetCore.Mvc;

using System.Configuration;

using Model = LSPApi.DataLayer.Model;


namespace LSPApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookController : ControllerBase
{

    private readonly IBookRepository _book;
    private readonly ILogger<BookController> _logger;
    private readonly IConfiguration _configuration;

    public BookController(IBookRepository Book, ILogger<BookController> logger, IConfiguration configuration)
    {
        _book = Book;
        _logger = logger;
        _configuration = configuration;
    }


    [HttpGet, Route("{id:int}")]
    public async Task<Model.BookDto> GetById(int id)
    {
        try
        {             
            _logger.LogInformation($"*** Book GetById: {id}");
            return await _book.GetById(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return new Model.BookDto();
        }
    }


    [HttpGet("gridsearch")]
    public async Task<List<Model.BookListResultsModel>> GetBooks(int startRow, int endRow, string sortColumn, string sortDirection, string filter = "")
    {
        try
        {
            _logger.LogInformation($"*** Book GridSearch: start: {startRow} end: {endRow} sort: {sortColumn} direction: {sortDirection} filter: {filter}");
            return await _book.GetBooks(startRow, endRow, sortColumn, sortDirection, filter);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return null;
        }
    }

    [HttpGet, Route("getidbyauthor/{id:int}")]
    public async Task<List<BookListSummaryModel>> GetByAuthor(int id)
    {
        try
        {
            _logger.LogInformation($"*** Book GetByAuthor: {id}");
            return await _book.GetIdsByAuthor(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return [];
        }
    }

    [HttpGet, Route("author/{id:int}")]
    public async Task<List<Model.BookSummaryModel>> GetByAuthorId(int id)
    { 
        try
        {
            _logger.LogInformation($"*** Book GetByAuthorId: {id}");
            return await _book.GetByAuthorId(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
            return new List<Model.BookSummaryModel>();
        }
    }


    [HttpGet]
    public async Task<IEnumerable<Model.BookDto>> GetAll() => await _book.GetAll();

    [HttpPost]
    public async Task Insert([FromBody] Model.BookDto Book)
    {
        try
        {
            Book.DateCreated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            await _book.Add(Book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message, ex);
        }

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
            _logger.LogError(ex.Message, ex);
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
            _logger.LogError(ex.Message, ex);
        }

    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFiles(List<IFormFile> files)
    {
        try
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("No files received.");
            }

            foreach (var file in files)
            {
                if (file.Length > 0)
                {
                    //var filePath = Path.Combine("uploads", file.FileName); // Adjust "uploads" folder path as needed
                    var sequrl = _configuration.GetValue<string>("seq");
                    var filePath = Path.Combine(sequrl, "data", file.FileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
            }

            return Ok("Files uploaded successfully.");
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
