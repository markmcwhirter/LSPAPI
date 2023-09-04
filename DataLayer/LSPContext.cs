using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LSPApi.DataLayer.Model;
using Org.BouncyCastle.Asn1.Mozilla;
using System.Configuration;

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

