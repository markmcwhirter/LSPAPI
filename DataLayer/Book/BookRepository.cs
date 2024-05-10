using LSPApi.DataLayer.Model;
using Microsoft.EntityFrameworkCore;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace LSPApi.DataLayer;
public class BookRepository : IBookRepository
{
    private readonly LSPContext _context;

    public BookRepository(LSPContext context)
    {
        _context = context;
    }

#pragma warning disable CS8603 // Possible null reference return.
    public async Task<BookDto> GetById(int id) =>  await _context.Book.FirstOrDefaultAsync(a => a.BookID == id);
#pragma warning restore CS8603 // Possible null reference return.

    public async Task<List<BookDto>> GetByAuthorId(int id) => await _context.Book.Where(a => a.AuthorID == id).ToListAsync();

    public async Task<IEnumerable<BookDto>> GetAll() => await _context.Book.ToListAsync();



    public async Task Add(BookDto Book)
    {
        _context.Book.Add(Book);
        await _context.SaveChangesAsync();
    }

    public async Task Update(BookDto Book)
    {
        _context.Book.Update(Book);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteByAuthorId(int id)
    {
        var bookList =  _context.Book.Where(a => a.AuthorID == id).ToList();

        if (bookList != null)
        {
            _context.Book.RemoveRange(bookList);
            await _context.SaveChangesAsync();
        }
    }


    public async Task Delete(int id)
    {
        var Book = await _context.Book.FirstOrDefaultAsync(a => a.BookID == id);
        if (Book != null)
        {
            _context.Book.Remove(Book);
            await _context.SaveChangesAsync();
        }
    }
}
