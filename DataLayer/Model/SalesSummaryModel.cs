namespace DataLayer.Model;

public class SalesSummaryModel
{
    public int? BooksSoldToDate { get; set; }
    public int? BooksSoldThisPeriod { get; set; }
    public decimal? BooksSalesThisPeriod { get; set; }
    public decimal? BooksSalesToDate { get; set; }

    public int? EBooksSoldToDate { get; set; }
    public int? EBooksSoldThisPeriod { get; set; }
    public decimal? EBooksSalesThisPeriod { get; set; }
    public decimal? EBooksSalesToDate { get; set; }

    public decimal? Royalties { get; set; }


}
