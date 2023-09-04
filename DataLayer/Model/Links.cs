using System.ComponentModel.DataAnnotations;

namespace LSPApi.DataLayer.Model;
public partial class LinkDto
{
    [Key]
    public int LinkID { get; set; }
	public int? BookID { get; set; }
	public string? LinkDescription { get; set; }
	public string? Link { get; set; }
	public string? DateCreated { get; set; }
	public string? DateUpdated { get; set; }
}
