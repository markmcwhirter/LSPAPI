using LSPApi.DataLayer.Model;

namespace LSPApi.DataLayer;

public interface IBookRepository
{
    Task<BookDto> GetById(int id);
    Task<List<BookListResultsModel>> GetBooks(int startRow, int endRow, string sortColumn, string sortDirection, string filter);

    Task<IEnumerable<BookDto>> GetAll();
    Task Add(BookDto Book);
    Task Update(BookDto Book);
    Task Delete(int id);
    Task DeleteByAuthorId(int id);
    Task<List<BookSummaryModel>> GetByAuthorId(int id);
}

