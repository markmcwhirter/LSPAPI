namespace LSPApi.DataLayer.Model;


/*
select c.Title, c.ISBN, b.VendorName, a.SalesDate, a.UnitsSold, a.UnitsToDate, a.SalesThisPeriod, a.SalesToDate, a.Royalty
from Sales a
left join Vendor b on a.VendorID= b.VendorID
left join Book c on c.BookID= a.BookID
*/

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
