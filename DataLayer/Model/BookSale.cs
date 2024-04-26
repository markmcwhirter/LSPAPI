namespace LSPApi.DataLayer.Model;

public class BookSaleDto
{
    public int AuthorID { get; set; }
    public string? Author { get; set; }
    public string? Title { get; set; }
    public int SaleID { get; set; }
    public int BookID { get; set; }
    public int VendorID { get; set; }
    public string? VendorName { get; set; }
    public string? SalesDate { get; set; }
    public decimal AuthorComp { get; set; }
    public decimal UnitsSold { get; set; }
    public decimal Royalty { get; set; }
    public decimal SalesToDate { get; set; }
    public decimal UnitsToDate { get; set; }
    public decimal SalesThisPeriod { get; set; }
    public string? DateCreated { get; set; }
    public string? DateUpdated { get; set; }
    public int TotalCount { get; set; }
}
