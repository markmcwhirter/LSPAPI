using LSPApi.DataLayer;
using LSPApi.DataLayer.Model;

using Microsoft.AspNetCore.Mvc;

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
    public async Task<Model.BookDto> GetById(int id) => await _book.GetById(id);

    [HttpGet("gridsearch")]
    public async Task<List<Model.BookListResultsModel>> GetBooks(int startRow, int endRow, string sortColumn, string sortDirection, string filter = "") =>
              await _book.GetBooks(startRow, endRow, sortColumn, sortDirection, filter);

    [HttpGet, Route("getidbyauthor/{id:int}")]
    public async Task<List<BookListSummaryModel>> GetByAuthor(int id) => await _book.GetIdsByAuthor(id);

    [HttpGet, Route("author/{id:int}")]
    public async Task<List<Model.BookSummaryModel>> GetByAuthorId(int id) => await _book.GetByAuthorId(id);


    [HttpGet]
    public async Task<IEnumerable<Model.BookDto>> GetAll() => await _book.GetAll();

    [HttpPost]
    public async Task Insert([FromBody] Model.BookDto Book) => await _book.Add(Book);

    [HttpPost, Route("update")]
    public async Task Update([FromBody] Model.BookUpdateModel BookUpdate)
    {
        Model.BookDto Book = new BookDto
        {
            AuthorBio = BookUpdate.AuthorBio,
            AuthorID = BookUpdate.AuthorID,
            AuthorPhoto = BookUpdate.AuthorPhoto,
            BookID = BookUpdate.BookID,
            Cover = BookUpdate.Cover,
            CoverIdea = BookUpdate.CoverIdea,
            DateCreated = BookUpdate.DateCreated,
            DateUpdated = BookUpdate.DateUpdated,
            Description = BookUpdate.Description,
            Document = BookUpdate.Document,
            Interior = BookUpdate.Interior,
            ISBN = BookUpdate.ISBN,
            Notes = BookUpdate.Notes,
            Subtitle = BookUpdate.Subtitle,
            Title = BookUpdate.Title

        };

        if (BookUpdate.BookID == 0)
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

    [HttpGet, Route("delete/{id:int}")]
    public async Task Delete(int id) => await _book.Delete(id);

    [HttpPost("upload")]
    public async Task<IActionResult> UploadFiles(List<IFormFile> files)
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

}
