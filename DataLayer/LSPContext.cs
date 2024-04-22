using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;

namespace LSPApi.DataLayer;

public class LSPContext : DbContext
{
    public LSPContext(DbContextOptions<LSPContext> options)
            : base(options)
    {
    }


    public DbSet<AuthorDto> Author { get; set; }
    public DbSet<BookDto> Book { get; set; }
    public DbSet<LinkDto> Links{ get; set; }
    public DbSet<LogDto> Log { get; set; }
    public DbSet<SaleDto> Sales { get; set; }
    public DbSet<SoldDto> Sold { get; set; }
    public DbSet<VendorDto> Vendor { get; set; }



}

