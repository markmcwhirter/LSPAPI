using LSPApi.DataLayer.Model;

namespace LSPApi.DataLayer;

public interface IBookRepository
{
    Task<BookDto> GetById(int id);
    Task<List<BookDto>> GetByAuthorId(int id);
    Task<IEnumerable<BookDto>> GetAll();
    Task Add(BookDto Book);
    Task Update(BookDto Book);
    Task Delete(int id);
}

