﻿using LSPApi.DataLayer.Model;

namespace LSPApi.DataLayer;

public interface ISaleRepository
{
    Task<List<BookSaleDto>> GetSales(int startRow, int endRow, string sortColumn, string sortDirection);

    Task<SaleDto> GetById(int id);
    Task<IEnumerable<SaleDto>> GetAll();
    Task Add(SaleDto Sale);
    Task Update(SaleDto Sale);
    Task Delete(int id);

    Task<List<BookSaleDto>> GetSales();

    Task<int> GetLastSaleId();
}

