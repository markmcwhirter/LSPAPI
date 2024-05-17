using LSPApi.DataLayer;
using LSPApi.DataLayer.Model;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using System.Globalization;

namespace LSPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly ISaleRepository _Sale;
        private readonly IMemoryCache _cache;

        public SaleController(ILogger<SaleController> logger, ISaleRepository Sale,IMemoryCache cache)
        {
            _logger = logger;
            _Sale = Sale;
            _cache = cache;
        }


        [HttpGet]
        public async Task<IEnumerable<SaleDto>> GetAll() => await _Sale.GetAll();

        [HttpPost]
        public async Task Insert([FromBody] List<SalePostModel> dataList) 
        {

            try
            {
                if( dataList == null || dataList.Count == 0) return;


                int maxid = await _Sale.GetLastSaleId();
                maxid++;

                foreach (var sale in dataList)
                {
                    DateTime dateTime = DateTime.ParseExact(sale.InputDate, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture);

                    SaleDto tmpsale = new SaleDto
                    {
                        SaleID = maxid,
                        BookID = sale.BookId,
                        DateCreated = dateTime.ToString("MMM dd yyyy hh:mm tt"),
                        DateUpdated = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                        Royalty = sale.Royalty,
                        SalesDate = sale.InputDate,
                        SalesThisPeriod = sale.SalesThisPeriod,
                        SalesToDate = sale.SalesToDate,
                        UnitsSold = sale.Units,
                        UnitsToDate = sale.UnitsToDate,
                        VendorID = sale.BookType
                    };

                    await _Sale.Add(tmpsale);

                    maxid++;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in Bulk Sales Entry");
            }
        }


        [HttpGet, Route("GetSales/{bookid:int}")]
        public async Task<SaleDto> LastSales(int bookId)
        {
            SaleDto result = new();


            try
            {
                // DateTime? maxdate = DateTime.MinValue;
                var data = await _Sale.GetAll();

                if (data != null)
                {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                    var maxdate = data.MaxBy(x => x.SalesDate).SalesDate;
#pragma warning restore CS8602 // Dereference of a possibly null reference.

                    for (int v = 1; v < 7; v++)
                    {
                        var tmpresult = data.Where(s => s.VendorID == v && s.BookID == bookId && s.SalesDate == maxdate);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in LastSales");
            }

            return result;

        }

        [HttpGet, Route("GetSales")]
        public async Task<List<BookSaleDto>> GetSales() => await _Sale.GetSales();


        [HttpGet("gridsearch")]
        public async Task<List<BookSaleDto>> GetSales(int startRow, int endRow, string sortColumn, string sortDirection)
        {
            List<BookSaleDto> result = new();

            try
            {
                string key = $"{sortColumn.PadLeft(20)}{sortDirection.PadLeft(20)}{startRow.ToString().PadLeft(20)}{endRow.ToString().PadLeft(5)}";


                if (!_cache.TryGetValue(key, out result))
                {
                    result = await _Sale.GetSales(startRow, endRow, sortColumn, sortDirection);

                    MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromHours(1));

                    _cache.Set(key, result, cacheEntryOptions);
                }
            }
            catch (Exception ex)
            {
                _ = ex.Message;
            }


            return result;
        }
    }
}
