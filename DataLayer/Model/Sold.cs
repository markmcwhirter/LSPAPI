using Microsoft.EntityFrameworkCore;

namespace LSPApi.DataLayer.Model;

[Keyless]
public class SoldDto
{
    public int LinkID { get; set; }
	public int BookID { get; set; }
	public string? LinkDescription { get; set; }
}
