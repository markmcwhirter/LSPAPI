using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;

namespace LSPApi.DataLayer;

public partial class LSPContext : DbContext
{
    public LSPContext(DbContextOptions<LSPContext> options)
            : base(options)
    {
    }


    public DbSet<AuthorDto> Author { get; set; }
    public DbSet<BookDto> Book { get; set; }
    public DbSet<SaleDto> Sales { get; set; }
    public DbSet<VendorDto> Vendor { get; set; }


    #region Author Definition
    /*
CREATE TABLE "Author" (
  x "AuthorID" int NOT NULL,
  x Prefix" varchar(20) DEFAULT NULL,
  x FirstName" varchar(50) DEFAULT NULL,
  x"MiddleName" varchar(50) DEFAULT NULL,
  x "LastName" varchar(50) DEFAULT NULL,
  x Suffix" varchar(20) DEFAULT NULL,
  x "Address1" varchar(255) DEFAULT NULL,
  x "Address2" varchar(255) DEFAULT NULL,
  x "City" varchar(50) DEFAULT NULL,
  x "State" varchar(25) DEFAULT NULL,
 x  "ZIP" varchar(12) DEFAULT NULL,
 x  "Country" varchar(25) DEFAULT NULL,
x "BusinessPhone" varchar(20) DEFAULT NULL,
 x  "HomePhone" varchar(20) DEFAULT NULL,
 x  "CellPhone" varchar(20) DEFAULT NULL,
 x  "Email" varchar(50) DEFAULT NULL,
x "Password" varchar(50) DEFAULT NULL,
x   "DateCreated" varchar(50) DEFAULT NULL,
 x  "DateUpdated" varchar(50) DEFAULT NULL,
x   "Username" varchar(50) DEFAULT NULL,
x "Admin" char(1) DEFAULT NULL,
x   "Bio" longtext,
x   "Notes" longtext,
  PRIMARY KEY ("AuthorID"),
  KEY "SK_LAST" ("LastName"),
  KEY "SK_FIRST" ("FirstName"),
  KEY "SK_USERNAME" ("Username")
);     */

    #endregion
    //protected override void OnModelCreating(ModelBuilder modelBuilder)
    //{

    //    modelBuilder.Entity<AuthorDto>(entity =>
    //    {
    //        entity.ToTable("lsp.Author");
    //        entity.HasKey(e => e.AuthorID);

    //        // entity.HasIndex(e => e.AuthorID, "PRIMARY");
    //        entity.HasIndex(e => e.LastName, "SK_LAST");
    //        entity.HasIndex(e => e.FirstName, "SK_FIRST");
    //        entity.HasIndex(e => e.Username, "SK_USERNAME");

    //        entity.Property(e => e.AuthorID).HasColumnName("AuthorID");
    //        entity.Property(e => e.Address1)
    //            .HasMaxLength(255)
    //            .IsUnicode(false)
    //            .HasColumnName("Address1");
    //        entity.Property(e => e.Address2)
    //            .HasMaxLength(255)
    //            .IsUnicode(false);
    //        entity.Property(e => e.Admin)
    //            .HasMaxLength(1)
    //            .IsUnicode(false)
    //            .IsFixedLength();
    //        entity.Property(e => e.Bio).HasColumnType("ntext");
    //        entity.Property(e => e.BusinessPhone)
    //            .HasMaxLength(20)
    //            .IsUnicode(false);
    //        entity.Property(e => e.CellPhone)
    //            .HasMaxLength(20)
    //            .IsUnicode(false);
    //        entity.Property(e => e.City)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.Country)
    //            .HasMaxLength(25)
    //            .IsUnicode(false);
    //        entity.Property(e => e.DateCreated)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.DateUpdated)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.Email)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.FirstName)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.HomePhone)
    //            .HasMaxLength(20)
    //            .IsUnicode(false);
    //        entity.Property(e => e.LastName)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.MiddleName)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.Notes).HasColumnType("ntext");
    //        entity.Property(e => e.Password)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.Prefix)
    //            .HasMaxLength(20)
    //            .IsUnicode(false);
    //        entity.Property(e => e.State)
    //            .HasMaxLength(25)
    //            .IsUnicode(false);
    //        entity.Property(e => e.Suffix)
    //            .HasMaxLength(20)
    //            .IsUnicode(false);
    //        entity.Property(e => e.Username)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.ZIP)
    //            .HasMaxLength(12)
    //            .IsUnicode(false)
    //            .HasColumnName("ZIP");

    //        entity.HasMany(e => e.Books)
    //               .WithOne(i => i.Author)
    //               .HasForeignKey(i => i.BookID)
    //               .OnDelete(DeleteBehavior.Cascade);
    //    });

    //    modelBuilder.Entity<BookDto>(entity =>
    //    {
    //        entity.ToTable("Book");
    //        entity.HasKey(e => e.BookID);
    //        entity.HasIndex(e => e.Title, "IX_Book");

    //        entity.Property(e => e.BookID).HasColumnName("BookID");
    //        entity.Property(e => e.AuthorBio).IsUnicode(false);
    //        entity.Property(e => e.AuthorID).HasColumnName("AuthorID");
    //        entity.Property(e => e.AuthorPhoto).IsUnicode(false);
    //        entity.Property(e => e.Cover).IsUnicode(false);
    //        entity.Property(e => e.CoverIdea).IsUnicode(false);
    //        entity.Property(e => e.DateCreated)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.DateUpdated)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.Description).HasColumnType("ntext");
    //        entity.Property(e => e.Interior).IsUnicode(false);
    //        entity.Property(e => e.ISBN)
    //            .HasMaxLength(25)
    //            .IsUnicode(false)
    //            .HasColumnName("ISBN");
    //        entity.Property(e => e.Subtitle)
    //            .HasMaxLength(255)
    //            .IsUnicode(false);
    //        entity.Property(e => e.Title)
    //            .HasMaxLength(255)
    //            .IsUnicode(false);

    //        entity.HasOne(i => i.Author)
    //                .WithMany(p => p.Books)
    //                .HasForeignKey(i => i.AuthorID);
    //    });

    //    modelBuilder.Entity<SaleDto>(entity =>
    //    {
    //        entity.HasKey(e => e.SaleID);
    //        entity.Property(e => e.SaleID).HasColumnName("SaleID");
    //        entity.Property(e => e.BookID).HasColumnName("BookID");
    //        entity.Property(e => e.DateCreated)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.DateUpdated)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //        entity.Property(e => e.Royalty).HasColumnType("money");
    //        entity.Property(e => e.SalesDate).HasColumnType("datetime");
    //        entity.Property(e => e.SalesThisPeriod).HasColumnType("money");
    //        entity.Property(e => e.SalesToDate).HasColumnType("money");
    //        entity.Property(e => e.VendorID).HasColumnName("VendorID");
    //    });

    //    modelBuilder.Entity<VendorDto>(entity =>
    //    {
    //        entity.ToTable("Vendor");
    //        entity.HasKey(e => e.VendorID);
    //        entity.Property(e => e.VendorID).HasColumnName("VendorID");
    //        entity.Property(e => e.DateCreated).HasColumnType("datetime");
    //        entity.Property(e => e.DateUpdated).HasColumnType("datetime");
    //        entity.Property(e => e.VendorName)
    //            .HasMaxLength(50)
    //            .IsUnicode(false);
    //    });

        // OnModelCreatingPartial(modelBuilder);


    //}
    //partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

