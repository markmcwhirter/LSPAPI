//using LSPApi.DataLayer.Model;

//namespace LSPApi.DataLayer;
//public class BookService : IBookService
//{
//    private readonly IRepository<BookDto> _BookRepository;

//    public BookService(IRepository<BookDto> BookRepository)
//    {
//        _BookRepository = BookRepository;
//    }

//    public async Task CreateBook(BookDto a)
//    {
//        var newBook = new BookDto
//        {
//            AuthorBio = a.AuthorBio,
//            AuthorID = a.AuthorID,
//            AuthorPhoto = a.AuthorPhoto,
//            BookID = a.BookID,
//            Cover = a.Cover,
//            CoverIdea = a.CoverIdea,
//            DateCreated = a.DateCreated,
//            DateUpdated = a.DateUpdated,
//            Description = a.Description,
//            Interior = a.Interior,
//            ISBN = a.ISBN,
//            Subtitle = a.Subtitle,
//            Title = a.Title
//        };

//        await _BookRepository.AddAsync(newBook);
//    }

//    public async Task UpdateBook(BookDto a)
//    {
//        var existingBook = await _BookRepository.GetByIdAsync(a.BookID);

//        if (existingBook == null)
//        {
//            throw new ArgumentException("Book not found");
//        }

//        existingBook.AuthorBio = a.AuthorBio;
//        existingBook.AuthorID = a.AuthorID;
//        existingBook.AuthorPhoto = a.AuthorPhoto;
//        existingBook.BookID = a.BookID;
//        existingBook.Cover = a.Cover;
//        existingBook.CoverIdea = a.CoverIdea;
//        existingBook.DateCreated = a.DateCreated;
//        existingBook.DateUpdated = a.DateUpdated;
//        existingBook.Description = a.Description;
//        existingBook.Interior = a.Interior;
//        existingBook.ISBN = a.ISBN;
//        existingBook.Subtitle = a.Subtitle;
//        existingBook.Title = a.Title;

//        await _BookRepository.UpdateAsync(existingBook);
//    }

//    public async Task<BookDto> GetBookById(int id)
//    {
//        return await _BookRepository.GetByIdAsync(id);
//    }

//    public async Task<IEnumerable<BookDto>> GetAllBooks()
//    {
//        return await _BookRepository.GetAllAsync();
//    }

//    public async Task DeleteBook(int id)
//    {
//        var existingBook = await _BookRepository.GetByIdAsync(id);

//        if (existingBook == null)
//        {
//            throw new ArgumentException("Book not found");
//        }

//        await _BookRepository.DeleteAsync(existingBook);
//    }
//}
