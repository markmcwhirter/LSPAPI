using DataLayer.Model;

using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;

using System.Text.Json;

namespace LSPApi.DataLayer;

public class BookRepository : IBookRepository
{
    private readonly LSPContext _context;

    public BookRepository(LSPContext context) => _context = context;

#pragma warning disable CS8603 // Possible null reference return.
    public async Task<BookDto> GetById(int id) => await _context.Book.FirstOrDefaultAsync(a => a.BookID == id);
#pragma warning restore CS8603 // Possible null reference return.

    public async Task<List<BookListResultsModel>> GetBooks(int startRow, int endRow, string sortColumn, string sortDirection, string filter)
    {
        List<BookListResultsModel> result = [];

        try
        {
            sortColumn = sortColumn == "null" ? "title" : sortColumn;
            sortDirection = sortDirection == "null" ? "ASC" : sortDirection;

            var query = _context.Author
                .Join(_context.Book, a => a.AuthorID, b => b.AuthorID,
                    (a, b) => new 
                    {
                        AuthorId = a.AuthorID,
                        Author = a.LastName + ", " + a.FirstName,
                        BookId = b.BookID,
                        Title = b.Title,
                        SubTitle = b.Subtitle,
                        ISBN = b.ISBN,
                        Notes = b.Notes
                    })
                .Select(p => new BookListResultsModel
                {
                    AuthorID = p.AuthorId,
                    BookID = p.BookId,
                    Author = p.Author,
                    Title = p.Title,
                    SubTitle = p.SubTitle,
                    ISBN = p.ISBN,
                    Notes = p.Notes
                })
                .AsQueryable();

            FilterBookModel? filterList = new();

            // parse filter and add to a where clause
            if (!string.IsNullOrEmpty(filter) && filter != "{}")
                filterList = JsonSerializer.Deserialize<FilterBookModel>(filter);

            if (filterList != null)
            {
                if (filterList.bookID != null)
                {
                    // TODO: implement {"authorID":{"filterType":"number","type":"inRange","filter":500,"filterTo":600}}

                    string? filtertype = filterList.bookID.type.ToLower();
                    int? filterValue = filterList.bookID.filter;

                    if (filterList.bookID.type.ToLower().Equals("inrange"))
                        query = query.Where(a => a.BookID >= filterList.bookID.filter && a.BookID <= filterList.bookID.filterTo);
                    else
                    {

                        if (filtertype.Equals("equals"))
                            query = query.Where(a => a.BookID == filterValue);
                        else if (filtertype.Equals("doesnotequal"))
                            query = query.Where(a => a.BookID != filterValue);
                        else if (filtertype.Equals("greaterthan"))
                            query = query.Where(a => a.BookID > filterValue);
                        else if (filtertype.Equals("greaterthanorequal"))
                            query = query.Where(a => a.BookID >= filterValue);
                        else if (filtertype.Equals("lessthan"))
                            query = query.Where(a => a.BookID < filterValue);
                        else if (filtertype.Equals("lessthanorequal"))
                            query = query.Where(a => a.BookID <= filterValue);
                        else if (filtertype.Equals("between"))
                            query = query.Where(a => a.BookID >= filterValue);
                        else if (filtertype.Equals("blank"))
                            query = query.Where(a => a.BookID == 0);
                        else if (filtertype.Equals("notblank"))
                            query = query.Where(a => a.BookID != 0);
                    }
                }
                if (filterList.author != null)
                    query = query.BuildStringQuery("Author", filterList.author.type.ToLower(), filterList.author.filter);
                if (filterList.title != null)
                    query = query.BuildStringQuery("Title", filterList.title.type.ToLower(), filterList.title.filter);
                if (filterList.subtitle != null)
                    query = query.BuildStringQuery("SubTitle", filterList.subtitle.type.ToLower(), filterList.subtitle.filter);
                if (filterList.isbn != null)
                    query = query.BuildStringQuery("ISBN", filterList.isbn.type.ToLower(), filterList.isbn.filter);
                if (filterList.notes != null)
                    query = query.BuildStringQuery("Notes", filterList.notes.type.ToLower(), filterList.notes.filter);
            }

            if (sortColumn == "AUTHOR")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.Author) : query.OrderByDescending(a => a.Author);
            else if (sortColumn == "TITLE")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.Title) : query.OrderByDescending(a => a.Title);
            else if (sortColumn == "SUBTITLE")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.SubTitle) : query.OrderByDescending(a => a.SubTitle);
            else if (sortColumn == "ISBN")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.ISBN) : query.OrderByDescending(a => a.ISBN);
            else if (sortColumn == "NOTES")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.Notes) : query.OrderByDescending(a => a.Notes);


            result = await query.Skip(startRow).Take(endRow - startRow)
                    .Select(p => new BookListResultsModel
                    {
                        AuthorID = p.AuthorID,
                        BookID = p.BookID,
                        Author = p.Author,
                        Title = p.Title,
                        SubTitle = p.SubTitle,
                        ISBN = p.ISBN,
                        Notes = p.Notes
                    })
                    .ToListAsync();
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }

        return result;

    }
    public async Task<List<BookSummaryModel>> GetByAuthorId(int id)
    {
        var result = new List<BookSummaryModel>();

        var books = await _context.Book.Where(a => a.AuthorID == id).ToListAsync();


        foreach (var b in books)
        {
            BookSummaryModel? summary = new()
            {
                AuthorID = b.AuthorID,
                AuthorBio = b.AuthorBio,
                AuthorPhoto = b.AuthorPhoto,
                BookID = b.BookID,
                Cover = b.Cover,
                CoverIdea = b.CoverIdea,
                DateCreated = b.DateCreated,
                DateUpdated = b.DateUpdated,
                Description = b.Description,
                Interior = b.Interior,
                ISBN = b.ISBN,
                Subtitle = b.Subtitle,
                Title = b.Title,
                Notes = b.Notes
            };

            // assign bookdto data

            SaleDto sresult = new();


            try
            {
                // get book sales
                SaleDto? data = _context.Sales
                    .Where(sale => sale.BookID == b.BookID && sale.VendorID == 5)
                    .OrderByDescending(sale => sale.SaleID)
                    .FirstOrDefault();

                if (data != null)
                {
                    summary.BooksSalesThisPeriod = data.SalesThisPeriod;
                    summary.BooksSalesToDate = data.SalesToDate;
                    summary.BooksSoldThisPeriod = data.UnitsSold;
                    summary.BooksSoldToDate = data.UnitsToDate;
                    summary.BookRoyalties = data.Royalty;
                }

                // get ebook sales
                SaleDto? data2 = _context.Sales
                    .Where(sale => sale.BookID == b.BookID && sale.VendorID == 6)
                    .OrderByDescending(sale => sale.SaleID)
                    .FirstOrDefault();

                if (data2 != null)
                {
                    summary.EBooksSalesThisPeriod = data2.SalesThisPeriod;
                    summary.EBooksSalesToDate = data2.SalesToDate;
                    summary.EBooksSoldThisPeriod = data2.UnitsSold;
                    summary.EBooksSoldToDate = data2.UnitsToDate;
                    summary.EBookRoyalties = data2.Royalty;
                }

                // get audio book sales
                SaleDto? data3 = _context.Sales
                    .Where(sale => sale.BookID == b.BookID && sale.VendorID == 7)
                    .OrderByDescending(sale => sale.SaleID)
                    .FirstOrDefault();

                if (data3 != null)
                {
                    summary.AudioBooksSalesThisPeriod = data3.SalesThisPeriod;
                    summary.AudioBooksSalesToDate = data3.SalesToDate;
                    summary.AudioBooksSoldThisPeriod = data3.UnitsSold;
                    summary.AudioBooksSoldToDate = data3.UnitsToDate;
                    summary.AudioBookRoyalties = data3.Royalty;
                }
                result.Add(summary);
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }

        }

        return result;
    }

    public async Task<IEnumerable<BookDto>> GetAll() => await _context.Book.ToListAsync();



    public async Task Add(BookDto Book)
    {
        int maxBook = _context.Book.Max(p => p.BookID);
        Book.BookID = maxBook + 1;

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
        var bookList = _context.Book.Where(a => a.AuthorID == id).ToList();

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
