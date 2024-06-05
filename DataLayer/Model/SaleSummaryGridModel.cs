namespace DataLayer.Model;

public class SaleSummaryGridModel
{
    public int? BookID { get; set; }
    public string? Title { get; set; }
    public string? ISBN { get; set; }
    public string? VendorName { get; set; }
    public string? SalesDate { get; set; }
    public decimal? UnitsSold { get; set; }
    public decimal? UnitsToDate { get; set; }
    public decimal? SalesThisPeriod { get; set; }
    public decimal? SalesToDate { get; set; }
    public decimal? Royalty { get; set; }

}
