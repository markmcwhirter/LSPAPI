using DataLayer.Model;

using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;

using System.Numerics;
using System.Text.Json;


namespace LSPApi.DataLayer;
public class SaleRepository : ISaleRepository
{
    private readonly LSPContext _context;

    public SaleRepository(LSPContext context) => _context = context;

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

    public async Task<int> GetLastSaleId() => await _context.Sales.MaxAsync(e => e.SaleID);

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



    // ORIGINAL CODE
    public async Task<List<BookSaleDto>> GetSales(int startRow, int endRow, string sortColumn, string sortDirection)
    {
        List<BookSaleDto> result = [];

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
                       BookID = b1.Book.BookID,
                       SaleID = b1.Sales.SaleID,
                       DateCreated = b1.Sales.DateCreated,
                       DateUpdated = b1.Sales.DateUpdated,
                       Author = a.LastName + ", " + a.FirstName,
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

            result = await query.Skip(startRow).Take(endRow - startRow).ToListAsync();
            foreach (var item in result)
            {
                item.SalesDate = item.SalesDate[..10];
                item.Title = item.Title.Replace("&amp;", "&");
                item.Title = item.Title.Replace("&#39;", "'");

                item.VendorName = item.VendorID switch
                {
                    1 => "Amazon",
                    2 => "Kindle",
                    3 => "Barnes and Noble",
                    4 => "Nook",
                    5 => "Paperbook",
                    6 => "EBook",
                    _ => ""
                };
            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }


        return result;

    }


    // NEW CODE
    public async Task<List<SaleSummaryGridModel>> GetSales2(int startRow, int endRow, string sortColumn, string sortDirection, string filter)
    {
        List<SaleSummaryGridModel> result = [];

        try
        {
            sortColumn = sortColumn == "null" ? "bookID" : sortColumn;
            sortDirection = sortDirection == "null" ? "ASC" : sortDirection;

            var query = from sale in _context.Sales
                        join vendor in _context.Vendor on sale.VendorID equals vendor.VendorID into vendorGroup
                        from vendor in vendorGroup.DefaultIfEmpty()
                        join book in _context.Book on sale.BookID equals book.BookID into bookGroup
                        from book in bookGroup.DefaultIfEmpty()
                        select new SaleSummaryGridModel
                        {
                            BookID = sale.BookID,
                            Title = book.Title,
                            ISBN = book.ISBN,
                            VendorName = vendor.VendorName,
                            SalesDate = sale.SalesDate,
                            UnitsSold = sale.UnitsSold,
                            UnitsToDate = sale.UnitsToDate,
                            SalesThisPeriod = sale.SalesThisPeriod,
                            SalesToDate = sale.SalesToDate,
                            Royalty = sale.Royalty
                        };

            sortColumn = sortColumn == "null" ? "BookID" : sortColumn.ToUpper();
            sortDirection = sortDirection == "null" ? "ASC" : sortDirection.ToUpper();

            FilterSalesModel? filterList = new();

            // parse filter and add to a where clause
            if (!string.IsNullOrEmpty(filter) && filter != "{}")
                filterList = JsonSerializer.Deserialize<FilterSalesModel>(filter);



            if (filterList != null)
            {
                if (filterList.bookId != null)
                {
                    // TODO: implement {"authorID":{"filterType":"number","type":"inRange","filter":500,"filterTo":600}}

                    string? filtertype = filterList.bookId.type.ToLower();
                    int? filterValue = filterList.bookId.filter;

                    if (filterList.bookId.type.ToLower().Equals("inrange"))
                        query = query.Where(a => a.BookID >= filterList.bookId.filter && a.BookID <= filterList.bookId.filterTo);
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
                if (filterList.title != null)
                {
                    string? filtertype = filterList.title.type.ToLower();
                    string? filterValue = filterList.title.filter;


                    if (filtertype.Equals("startswith"))
                        query = query.Where(a => EF.Functions.Like(a.Title, $"{filterValue}%"));
                    else if (filtertype.Equals("endswith"))
                        query = query.Where(a => EF.Functions.Like(a.Title, $"%{filterValue}"));
                    else if (filtertype.Equals("equals"))
                        query = query.Where(a => a.Title == filterValue);
                    else if (filtertype.Equals("does not equal"))
                        query = query.Where(a => a.Title != filterValue);
                    else if (filtertype.Equals("blank"))
                        query = query.Where(a => a.Title == null);
                    else if (filtertype.Equals("contains"))
                        query = query.Where(a => EF.Functions.Like(a.Title, $"%{filterValue}%"));
                    else if (filtertype.Equals("notcontains"))
                        query = query.Where(a => !EF.Functions.Like(a.Title, $"%{filterValue}%"));
                }
                if (filterList.isbn != null)
                {
                    string? filtertype = filterList.isbn.type.ToLower();
                    string? filterValue = filterList.isbn.filter;


                    if (filtertype.Equals("startswith"))
                        query = query.Where(a => EF.Functions.Like(a.ISBN, $"{filterValue}%"));
                    else if (filtertype.Equals("endswith"))
                        query = query.Where(a => EF.Functions.Like(a.ISBN, $"%{filterValue}"));
                    else if (filtertype.Equals("equals"))
                        query = query.Where(a => a.ISBN == filterValue);
                    else if (filtertype.Equals("does not equal"))
                        query = query.Where(a => a.ISBN != filterValue);
                    else if (filtertype.Equals("blank"))
                        query = query.Where(a => a.ISBN == null);
                    else if (filtertype.Equals("contains"))
                        query = query.Where(a => EF.Functions.Like(a.ISBN, $"%{filterValue}%"));
                    else if (filtertype.Equals("notcontains"))
                        query = query.Where(a => !EF.Functions.Like(a.ISBN, $"%{filterValue}%"));

                }
                if (filterList.vendorName != null)
                {
                    string? filtertype = filterList.vendorName.type.ToLower();
                    string? filterValue = filterList.vendorName.filter;


                    if (filtertype.Equals("startswith"))
                        query = query.Where(a => EF.Functions.Like(a.VendorName, $"{filterValue}%"));
                    else if (filtertype.Equals("endswith"))
                        query = query.Where(a => EF.Functions.Like(a.VendorName, $"%{filterValue}"));
                    else if (filtertype.Equals("equals"))
                        query = query.Where(a => a.VendorName == filterValue);
                    else if (filtertype.Equals("does not equal"))
                        query = query.Where(a => a.VendorName != filterValue);
                    else if (filtertype.Equals("blank"))
                        query = query.Where(a => a.VendorName == null);
                    else if (filtertype.Equals("contains"))
                        query = query.Where(a => EF.Functions.Like(a.VendorName, $"%{filterValue}%"));
                    else if (filtertype.Equals("notcontains"))
                        query = query.Where(a => !EF.Functions.Like(a.VendorName, $"%{filterValue}%"));
                }
                if (filterList.salesDate != null)
                {
                    string? filtertype = filterList.salesDate.type.ToLower();
                    string? filterValue = filterList.salesDate.filter;


                    if (filtertype.Equals("startswith"))
                        query = query.Where(a => EF.Functions.Like(a.SalesDate, $"{filterValue}%"));
                    else if (filtertype.Equals("endswith"))
                        query = query.Where(a => EF.Functions.Like(a.SalesDate, $"%{filterValue}"));
                    else if (filtertype.Equals("equals"))
                        query = query.Where(a => a.SalesDate == filterValue);
                    else if (filtertype.Equals("does not equal"))
                        query = query.Where(a => a.SalesDate != filterValue);
                    else if (filtertype.Equals("blank"))
                        query = query.Where(a => a.SalesDate == null);
                    else if (filtertype.Equals("contains"))
                        query = query.Where(a => EF.Functions.Like(a.SalesDate, $"%{filterValue}%"));
                    else if (filtertype.Equals("notcontains"))
                        query = query.Where(a => !EF.Functions.Like(a.SalesDate, $"%{filterValue}%"));
                }
                if (filterList.unitsSold != null)
                {
                    // TODO: implement {"authorID":{"filterType":"number","type":"inRange","filter":500,"filterTo":600}}

                    string? filtertype = filterList.unitsSold.type.ToLower();
                    int? filterValue = filterList.unitsSold.filter;

                    if (filterList.unitsSold.type.ToLower().Equals("inrange"))
                        query = query.Where(a => a.UnitsSold >= filterList.unitsSold.filter && a.UnitsSold <= filterList.unitsSold.filterTo);
                    else
                    {

                        if (filtertype.Equals("equals"))
                            query = query.Where(a => a.UnitsSold == filterValue);
                        else if (filtertype.Equals("doesnotequal"))
                            query = query.Where(a => a.UnitsSold != filterValue);
                        else if (filtertype.Equals("greaterthan"))
                            query = query.Where(a => a.UnitsSold > filterValue);
                        else if (filtertype.Equals("greaterthanorequal"))
                            query = query.Where(a => a.UnitsSold >= filterValue);
                        else if (filtertype.Equals("lessthan"))
                            query = query.Where(a => a.UnitsSold < filterValue);
                        else if (filtertype.Equals("lessthanorequal"))
                            query = query.Where(a => a.UnitsSold <= filterValue);
                        else if (filtertype.Equals("between"))
                            query = query.Where(a => a.UnitsSold >= filterValue);
                        else if (filtertype.Equals("blank"))
                            query = query.Where(a => a.UnitsSold == 0);
                        else if (filtertype.Equals("notblank"))
                            query = query.Where(a => a.UnitsSold != 0);
                    }
                }
            }

            if (sortColumn == "TITLE")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.Title) :
                    query.OrderByDescending(a => a.Title);
            else if (sortColumn == "BOOKID")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.BookID) : query.OrderByDescending(a => a.BookID);
            else if (sortColumn == "ISBN")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.ISBN) : query.OrderByDescending(a => a.ISBN);
            else if (sortColumn == "VENDORNAME")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.VendorName) : query.OrderByDescending(a => a.VendorName);
            else if (sortColumn == "SALESDATE")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.SalesDate) : query.OrderByDescending(a => a.SalesDate);
            else if (sortColumn == "UNITSSOLD")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.UnitsSold) : query.OrderByDescending(a => a.UnitsSold);
            else if (sortColumn == "UNITSTODATE")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.UnitsToDate) : query.OrderByDescending(a => a.UnitsToDate);
            else if (sortColumn == "SALESTHISPERIOD")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.SalesThisPeriod) : query.OrderByDescending(a => a.SalesThisPeriod);
            else if (sortColumn == "SALESTODATE")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.SalesToDate) : query.OrderByDescending(a => a.SalesToDate);
            else if (sortColumn == "ROYALTY")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.Royalty) : query.OrderByDescending(a => a.Royalty);



            result = await query.Skip(startRow).Take(endRow - startRow)
                    .Select(p => new SaleSummaryGridModel
                    {
                        BookID = p.BookID,
                        Title = p.Title,
                        ISBN = p.ISBN,
                        VendorName = p.VendorName,
                        SalesDate = p.SalesDate,
                        UnitsSold = p.UnitsSold,
                        UnitsToDate = p.UnitsToDate,
                        SalesThisPeriod = p.SalesThisPeriod,
                        SalesToDate = p.SalesToDate,
                        Royalty = p.Royalty
                    })
                    .ToListAsync();

            foreach (var item in result)
            {
                item.SalesDate = string.IsNullOrEmpty(item.SalesDate) ? "" : item.SalesDate[..10];                
                item.Title = string.IsNullOrEmpty(item.Title) ? "" : item.Title.Replace("&amp;", "&");
                item.Title = string.IsNullOrEmpty(item.Title) ? "" : item.Title.Replace("&#39;", "'");

            }
        }
        catch (Exception ex)
        {
            _ = ex.Message;
        }

        return result;

    }
}
