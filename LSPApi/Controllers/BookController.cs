using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using model = LSPApi.DataLayer.Model;

namespace LSPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IBookRepository _Book;
        private readonly IConfiguration _configuration;

        public BookController(ILogger<BookController> logger, IConfiguration configuration, IBookRepository Book)
        {
            _logger = logger;
            _Book = Book;
            _configuration = configuration;
        }


        [HttpGet, Route("{id:int}")]
        public async Task<model.BookDto> GetById(int id) => await _Book.GetById(id);


        [HttpGet, Route("author/{id:int}")]
        public async Task<List<model.BookSummaryModel>> GetByAuthorId(int id) => await _Book.GetByAuthorId(id);

        [HttpGet]
        public async Task<IEnumerable<model.BookDto>> GetAll() => await _Book.GetAll();

        [HttpPost]
        public async Task Insert([FromBody] model.BookDto Book)
        {
            Book.DateCreated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
            await _Book.Add(Book);
        }

        [HttpPost, Route("update")]
        public async Task Update([FromBody] model.BookDto Book)
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
    }
}
