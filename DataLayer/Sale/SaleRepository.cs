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

        //await _context.Author
        //    .Join(_context.Book,
        //        a => a.AuthorID,
        //        b => b.AuthorID,
        //        (a, b) => new { Author = a, Book = b })
        //    .Join(_context.Sales,
        //        b1 => b1.Book.BookID,
        //        s1 => s1.SaleID,
        //        (b1, s1) => new { Book = b1, Sale = s1 })
        //    .Join(_context.Vendor,
        //        s2 => s2.Sale.VendorID,
        //        v1 => v1.VendorID,
        //        (s2, v1) => new BookSaleDto
        //        {
        //             SaleID = s2.Sale.SaleID,
        //             Author = s2.Book.Author.LastName.Trim() + ", " + s2.Book.Author.FirstName.Trim(),
        //             Title = s2.Book.Book.Title ?? "",
        //             VendorName = v1.VendorName ?? "",
        //             SalesDate = s2.Sale.SalesDate, // .HasValue ? s2.Sale.SalesDate.Value.ToString("yyyy-MM-dd") : "",
        //             UnitsSold = s2.Sale.UnitsSold ?? 0M,
        //             UnitsToDate = s2.Sale.UnitsToDate ?? 0M,
        //             SalesThisPeriod = s2.Sale.SalesThisPeriod ?? 0M,
        //             SalesToDate = s2.Sale.SalesToDate  ?? 0M,
        //             Royalty = s2.Sale.Royalty ?? 0M
        //        })
        //.ToListAsync();

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        var tmpdata = await _context.Sales
    .Join(_context.Book,
        a => a.BookID,
        b => b.BookID,
        (a, b) => new { Sales = a, Book = b })
    .Join(_context.Author,
        b1 => b1.Book.AuthorID,
        a => a.AuthorID,
        (b1, a) => new  BookSaleDto
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
            VendorID = b1.Sales.VendorID ?? 0
        })
        .ToListAsync();
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        return tmpdata;
    }

}
