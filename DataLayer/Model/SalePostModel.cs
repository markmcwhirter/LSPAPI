namespace LSPApi.DataLayer.Model;

public class SalePostModel
{
    public int BookType { get; set; }
    public string? InputDate { get; set; }
    public int BookId { get; set; }
    public int Units { get; set; }
    public int UnitsToDate { get; set; }
    public decimal Royalty { get; set; }
    public decimal SalesToDate { get; set; }
    public decimal SalesThisPeriod { get; set; }

}