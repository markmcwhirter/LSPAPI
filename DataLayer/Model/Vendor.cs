using System.ComponentModel.DataAnnotations;

namespace LSPApi.DataLayer.Model;
public partial class VendorDto
{
    [Key]
    public int VendorID { get; set; }
	public string? VendorName { get; set; }
	public DateTime? DateCreated { get; set; }
	public DateTime? DateUpdated { get; set; }
}
