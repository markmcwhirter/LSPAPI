using LSPApi.DataLayer.Model;

namespace LSPApi.DataLayer;

public interface ISaleRepository
{
    Task<SaleDto> GetById(int id);
    Task<IEnumerable<SaleDto>> GetAll();
    Task Add(SaleDto Sale);
    Task Update(SaleDto Sale);
    Task Delete(int id);

    Task<List<BookSaleDto>> GetSales();

    Task<int> GetLastSaleId();
}

