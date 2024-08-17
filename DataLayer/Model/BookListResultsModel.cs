using System.ComponentModel.DataAnnotations;

namespace LSPApi.DataLayer.Model;


public class BookListResultsModel
{
    public int BookID { get; set; }
    public int AuthorID { get; set; }
    public string Title { get; set; }
    public string SubTitle { get; set; }
    public string ISBN { get; set; }
    public string Author { get; set; }
    public string Notes { get; set; }
    public string? InfoLink { get; set; }
    public string? EditLink { get; set; }
    public string? DeleteLink { get; set; }
}
