using Microsoft.AspNetCore.Mvc;
using LSPApi.DataLayer;
using model = LSPApi.DataLayer.Model;
using LSPApi.DataLayer.Model;
using System.Collections.Generic;

namespace LSPApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SaleController : ControllerBase
    {
        private readonly ILogger<SaleController> _logger;
        private readonly ISaleRepository _Sale;

        public SaleController(ILogger<SaleController> logger, ISaleRepository Sale)
        {
            _logger = logger;
            _Sale = Sale;
        }


        [HttpGet]
        public async Task<IEnumerable<model.SaleDto>> GetAll() => await _Sale.GetAll();

        [HttpPost]
        public async Task Insert([FromBody] model.SaleDto Sale) => await _Sale.Add(Sale);

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

    }
}
