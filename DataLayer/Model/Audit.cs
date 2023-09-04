namespace LSPApi.DataLayer.Model;
public partial class Audit
{
    public int ID { get; set; }
	public string? DateTime { get; set; }
	public string? AuditMessage { get; set; }
}
