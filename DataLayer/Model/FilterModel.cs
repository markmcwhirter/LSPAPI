namespace LSPApi.DataLayer.Model;

public class AuthorId
{
    public string filterType { get; set; }
    public string type { get; set; }
    public int filter { get; set; }
}

public class FirstName
{
    public string filterType { get; set; }
    public string type { get; set; }
    public string filter { get; set; }
}
public class LastName
{
    public string filterType { get; set; }
    public string type { get; set; }
    public string filter { get; set; }
}
public class EMailAddress
{
    public string filterType { get; set; }
    public string type { get; set; }
    public string filter { get; set; }
}


public class FilterModel
{
    public AuthorId authorID { get; set; }
    public FirstName firstName { get; set; }
    public LastName lastName { get; set; }
    public EMailAddress eMail { get; set; }
}
