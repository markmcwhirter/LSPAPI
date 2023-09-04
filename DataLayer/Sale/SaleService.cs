//using LSPApi.DataLayer.Model;

//namespace LSPApi.DataLayer;
//public class SaleService : ISaleService
//{
//    private readonly IRepository<SaleDto> _SaleRepository;

//    public SaleService(IRepository<SaleDto> SaleRepository)
//    {
//        _SaleRepository = SaleRepository;
//    }

//    public async Task CreateSale(SaleDto a)
//    {
//        var newSale = new SaleDto
//        {
//            BookID = a.BookID,
//            DateCreated = a.DateCreated,
//            DateUpdated = a.DateUpdated,
//            Royalty = a.Royalty,
//            SalesDate = a.SalesDate,
//            SalesThisPeriod = a.SalesThisPeriod,
//            SalesToDate = a.SalesToDate,
//            UnitsSold = a.UnitsSold,
//            UnitsToDate = a.UnitsToDate,
//            VendorID = a.VendorID,
//            SaleID = a.SaleID
//        };

//        await _SaleRepository.AddAsync(newSale);
//    }

//    //public async Task UpdateSale(SaleDto a)
//    //{
//    //    var existingSale = await _SaleRepository.GetByIdAsync(a.SaleID);

//    //    if (existingSale == null)
//    //    {
//    //        throw new ArgumentException("Sale not found");
//    //    }

//    //    existingSale.BookID = a.BookID;
//    //    existingSale.DateCreated = a.DateCreated;
//    //    existingSale.DateUpdated = a.DateUpdated;
//    //    existingSale.Royalty = a.Royalty;
//    //    existingSale.SaleID = a.SaleID;
//    //    existingSale.SalesDate = a.SalesDate;
//    //    existingSale.SalesThisPeriod = a.SalesThisPeriod;
//    //    existingSale.SalesToDate = a.SalesToDate;
//    //    existingSale.UnitsSold = a.UnitsSold;
//    //    existingSale.UnitsToDate = a.UnitsToDate;
//    //    existingSale.VendorID = a.VendorID;

//    //    await _SaleRepository.UpdateAsync(existingSale);
//    //}

//    //public async Task<SaleDto> GetSaleById(int id)
//    //{
//    //    return await _SaleRepository.GetByIdAsync(id);
//    //}

//    public async Task<IEnumerable<SaleDto>> GetAllSales() => await _SaleRepository.GetAllAsync();    

//    //public async Task DeleteSale(int id)
//    //{
//    //    var existingSale = await _SaleRepository.GetByIdAsync(id);

//    //    if (existingSale == null)
//    //    {
//    //        throw new ArgumentException("Sale not found");
//    //    }

//    //    await _SaleRepository.DeleteAsync(existingSale);
//    //}

//    //public async Task<SaleDto> LastSales(int bookId)
//    //{
//    //    SaleDto result = new SaleDto();

//    //    var data = await _SaleRepository.GetAllAsync();

//    //    // first find the maximum date

//    //    DateTime? maxdate = data.MaxBy(x => x.SalesDate).SalesDate;

//    //    //DateTime? maxdate = DateTime.MinValue;

//    //    //foreach (var item in data)
//    //    //{
//    //    //    if (item.SalesDate > maxdate)
//    //    //        maxdate = item.SalesDate;
//    //    //}

//    //    for (int v = 0; v < 7; v++)
//    //    { 
//    //        var tmpresult = data.Where(s => s.VendorID == v && s.BookID == bookId && s.SalesDate == maxdate);
//    //    }   

//    //    return result;
//    //}
    
//}
