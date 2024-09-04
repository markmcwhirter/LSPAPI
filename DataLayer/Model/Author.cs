using System.ComponentModel.DataAnnotations;

namespace LSPApi.DataLayer.Model;
public partial class AuthorDto
{
	[Key]
    public int AuthorID { get; set; }
	public string? Prefix { get; set; }
	public string? FirstName { get; set; }
	public string? MiddleName { get; set; }
	public string? LastName { get; set; }
	public string? Suffix { get; set; }
	public string? Address1 { get; set; }
	public string? Address2 { get; set; }
	public string? City { get; set; }
	public string? State { get; set; }
	public string? ZIP { get; set; }
	public string? Country { get; set; }
	public string? BusinessPhone { get; set; }
	public string? HomePhone { get; set; }
	public string? CellPhone { get; set; }
	public string? Email { get; set; }
	public string? Password { get; set; }
	public string? DateCreated { get; set; }
	public string? DateUpdated { get; set; }
	public string? Username { get; set; }
	public string? Admin { get; set; }
	public string? Bio { get; set; }
    public string? Notes { get; set; }
	public ICollection<BookDto> Books { get; set; }

}
