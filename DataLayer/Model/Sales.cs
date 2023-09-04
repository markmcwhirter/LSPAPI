using System.ComponentModel.DataAnnotations;

namespace LSPApi.DataLayer.Model;
public partial class SaleDto
{
    [Key]
    public int SaleID { get; set; }
	public int BookID { get; set; }
	public int? VendorID { get; set; }
	public DateTime? SalesDate { get; set; }
	public int? UnitsSold { get; set; }
	public decimal? Royalty { get; set; }
	public string? DateCreated { get; set; }
	public string? DateUpdated { get; set; }
	public decimal? SalesToDate { get; set; }
	public int? UnitsToDate { get; set; }
	public decimal? SalesThisPeriod { get; set; }
}
