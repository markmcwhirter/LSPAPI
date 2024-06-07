namespace LSPApi.DataLayer.Model;

public class FilterModel        // for Author grid
{
    public AuthorId authorID { get; set; }
    public FirstName firstName { get; set; }
    public LastName lastName { get; set; }
    public EMailAddress eMail { get; set; }
}


public class FilterSalesModel
{
    public BookId bookId { get; set; }
    public Title title { get; set; }
    public ISBN isbn { get; set; }
    public VendorName vendorName { get; set; }
    public SalesDate salesDate { get; set; }
    public UnitsSold unitsSold { get; set; }
    public SalesThisPeriod salesThisPeriod { get; set; }
    public SalesToDate salesToDate { get; set; }
    public Royalty royalty { get; set; }
}


public class FilterBookModel
{
    public Author author { get; set; }
    public Title title { get; set; }
    public SubTitle subtitle { get; set; }
    public ISBN isbn { get; set; }
    public Notes notes { get; set; }
}


public class BookId
{
    public string filterType { get; set; }
    public string type { get; set; }
    public int? filter { get; set; }
    public int? filterTo { get; set; }
}
public class Title
{
    public string filterType { get; set; }
    public string type { get; set; }
    public string filter { get; set; }
}
public class ISBN
{
    public string filterType { get; set; }
    public string type { get; set; }
    public string filter { get; set; }
}
public class VendorName
{
    public string filterType { get; set; }
    public string type { get; set; }
    public string filter { get; set; }
}
public class SalesDate
{
    public string filterType { get; set; }
    public string type { get; set; }
    public string filter { get; set; }
}

public class UnitsSold
{
    public string filterType { get; set; }
    public string type { get; set; }
    public int filter { get; set; }
    public int filterTo { get; set; }
}

public class UnitsToDate
{
    public string filterType { get; set; }
    public string type { get; set; }
    public int filter { get; set; }
    public int filterTo { get; set; }
}
public class SalesThisPeriod
{
    public string filterType { get; set; }
    public string type { get; set; }
    public decimal filter { get; set; }
    public decimal filterTo { get; set; }
}

public class SalesToDate
{
    public string filterType { get; set; }
    public string type { get; set; }
    public decimal filter { get; set; }
    public decimal filterTo { get; set; }
}

public class Royalty
{
    public string filterType { get; set; }
    public string type { get; set; }
    public decimal filter { get; set; }
    public decimal filterTo { get; set; }
}


public class Author
{
    public string filterType { get; set; }
    public string type { get; set; }
    public string filter { get; set; }
}
public class SubTitle
{
    public string filterType { get; set; }
    public string type { get; set; }
    public string filter { get; set; }
}

public class Notes
{
    public string filterType { get; set; }
    public string type { get; set; }
    public string filter { get; set; }
}

public class AuthorId
{
    public string filterType { get; set; }
    public string type { get; set; }
    public int filter { get; set; }
    public int filterTo { get; set; }
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



