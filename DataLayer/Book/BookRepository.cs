﻿using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;

using System.Net;

namespace LSPApi.DataLayer;
public class BookRepository : IBookRepository
{
    private readonly LSPContext _context;

    public BookRepository(LSPContext context) => _context = context;

#pragma warning disable CS8603 // Possible null reference return.
    public async Task<BookDto> GetById(int id) => await _context.Book.FirstOrDefaultAsync(a => a.BookID == id);
#pragma warning restore CS8603 // Possible null reference return.

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
                Title = b.Title
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
                    summary.BooksSoldToDate  =data.UnitsToDate;
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
