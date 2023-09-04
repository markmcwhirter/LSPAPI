using System.ComponentModel.DataAnnotations;

namespace LSPApi.DataLayer.Model;
public partial class BookDto
{
    [Key]
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
}
