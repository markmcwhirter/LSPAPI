using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;


namespace LSPApi.DataLayer;
public class SaleRepository : ISaleRepository
{
    private readonly LSPContext _context;

    public SaleRepository(LSPContext context)
    {
        _context = context;
    }

#pragma warning disable CS8603 // Possible null reference return.
    public async Task<SaleDto> GetById(int id) => await _context.Sales.FirstOrDefaultAsync(a => a.SaleID == id);
#pragma warning restore CS8603 // Possible null reference return.

    public async Task<IEnumerable<SaleDto>> GetAll() => await _context.Sales.ToListAsync();


    public async Task Add(SaleDto Sale)
    {
        _context.Sales.Add(Sale);
        await _context.SaveChangesAsync();
    }

    public async Task Update(SaleDto Sale)
    {
        _context.Sales.Update(Sale);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var Sale = await _context.Sales.FirstOrDefaultAsync(a => a.SaleID == id);
        if (Sale != null)
        {
            _context.Sales.Remove(Sale);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<List<BookSaleDto>> GetSales()
    {


#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var tmpdata = await _context.Sales
    .Join(_context.Book,
        a => a.BookID,
        b => b.BookID,
        (a, b) => new { Sales = a, Book = b })
    .Join(_context.Author,
        b1 => b1.Book.AuthorID,
        a => a.AuthorID,
        (b1, a) => new BookSaleDto
        {
            SaleID = b1.Sales.SaleID,
            Author = a.LastName.Trim() + ", " + a.FirstName.Trim(),
            Title = b1.Book.Title ?? "",
            VendorName = "",
            SalesDate = b1.Sales.SalesDate,
            UnitsSold = b1.Sales.UnitsSold ?? 0M,
            UnitsToDate = b1.Sales.UnitsToDate ?? 0M,
            SalesThisPeriod = b1.Sales.SalesThisPeriod ?? 0M,
            SalesToDate = b1.Sales.SalesToDate ?? 0M,
            Royalty = b1.Sales.Royalty ?? 0M,
            //VendorID = b1.Sales.VendorID ?? 0
        })
        .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        return tmpdata;
    }

    public async Task<int> GetLastSaleId() => await _context.Sales.MaxAsync(e => e.SaleID);

    public async Task<List<BookSaleDto>> GetSales(int startRow, int endRow, string sortColumn, string sortDirection)
    {
        List<BookSaleDto> result = new();

        try
        {
            var query = _context.Sales
               .Join(_context.Book,
                   a => a.BookID,
                   b => b.BookID,
                   (a, b) => new { Sales = a, Book = b })
               .Join(_context.Author,
                   b1 => b1.Book.AuthorID,
                   a => a.AuthorID,
                   (b1, a) => new BookSaleDto
                   {
                       AuthorID = a.AuthorID,
                       BookID  = b1.Book.BookID,
                       SaleID = b1.Sales.SaleID,
                       DateCreated = b1.Sales.DateCreated,
                       DateUpdated  = b1.Sales.DateUpdated,
                       Author = a.LastName.Trim() + ", " + a.FirstName.Trim(),
                       Title = b1.Book.Title ?? "",
                       VendorName = "",
                       SalesDate = b1.Sales.SalesDate,
                       UnitsSold = b1.Sales.UnitsSold ?? 0M,
                       UnitsToDate = b1.Sales.UnitsToDate ?? 0M,
                       SalesThisPeriod = b1.Sales.SalesThisPeriod ?? 0M,
                       SalesToDate = b1.Sales.SalesToDate ?? 0M,
                       Royalty = b1.Sales.Royalty ?? 0M,
                       VendorID = b1.Sales.VendorID ?? 0
                   }).AsQueryable();


            sortColumn = sortColumn == "null" ? "SaleID" : sortColumn;
            sortColumn = sortColumn.ToUpper();

            sortDirection = sortDirection == "null" ? "ASC" : sortDirection.ToUpper();


            if (sortColumn == "SALEID")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.SaleID) :
                    query.OrderByDescending(a => a.SaleID);
            else if (sortColumn == "BOOKID")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.BookID) : query.OrderByDescending(a => a.BookID);
            else if (sortColumn == "VENDORID")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.VendorID) : query.OrderByDescending(a => a.VendorID);
            else if (sortColumn == "SALESDATE")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.SalesDate) : query.OrderByDescending(a => a.SalesDate);
            else
                query = query.OrderBy(a => a.SaleID);


            // List<AuthorDto> result = query.Skip(startRow).Take(endRow - startRow).ToList();
            result = await query.Skip(startRow).Take(endRow - startRow).ToListAsync();
            foreach( var item in result)
            {
                item.SalesDate = item.SalesDate.Substring(0, 10);
                item.Title = item.Title.Replace("&amp;", "&");
                item.Title = item.Title.Replace("&#39;", "'");

                switch (item.VendorID)
                {
                    case 1:
                        item.VendorName = "Amazon";
                        break;
                    case 2:
                        item.VendorName = "Kindle";
                        break;
                    case 3:
                        item.VendorName = "Barnes and Noble";
                        break;
                    case 4:
                        item.VendorName = "Nook";
                        break;
                    case 5:
                        item.VendorName = "Paperbook";
                        break;
                    case 6:
                        item.VendorName = "EBook";
                        break;
                    default:
                        item.VendorName = "";
                        break;
                }
            }


        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }


        return result;

    }
}
