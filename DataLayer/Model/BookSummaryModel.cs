namespace LSPApi.DataLayer.Model;

public class BookSummaryModel
{
    public int BookID { get; set; }
    public int AuthorID { get; set; }
    public string? Title { get; set; }
    public string? Subtitle { get; set; }
    public string? ISBN { get; set; }
    public string? Description { get; set; }
    public string? DateCreated { get; set; }
    public string? DateUpdated { get; set; }
    public string? Cover { get; set; }
    public string? Interior { get; set; }
    public string? AuthorPhoto { get; set; }
    public string? AuthorBio { get; set; }
    public string? CoverIdea { get; set; }
    public string? Notes { get; set; }

    public int? BooksSoldToDate { get; set; }
    public int? BooksSoldThisPeriod { get; set; }
    public decimal? BooksSalesThisPeriod { get; set; }
    public decimal? BooksSalesToDate { get; set; }
    public decimal? BookRoyalties { get; set; }
    public int? EBooksSoldToDate { get; set; }
    public int? EBooksSoldThisPeriod { get; set; }
    public decimal? EBooksSalesThisPeriod { get; set; }
    public decimal? EBooksSalesToDate { get; set; }    
    public decimal? EBookRoyalties { get; set; }
    public int? AudioBooksSoldToDate { get; set; }
    public int? AudioBooksSoldThisPeriod { get; set; }
    public decimal? AudioBooksSalesThisPeriod { get; set; }
    public decimal? AudioBooksSalesToDate { get; set; }
    public decimal? AudioBookRoyalties { get; set; }
}
