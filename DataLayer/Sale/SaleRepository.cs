using LSPApi.DataLayer.Model;
using Microsoft.EntityFrameworkCore;

namespace LSPApi.DataLayer;
public class SaleRepository : ISaleRepository
{
    private readonly LSPContext _context;

    public SaleRepository(LSPContext context)
    {
        _context = context;
    }

    public async Task<SaleDto> GetById(int id) => await _context.Sales.FirstOrDefaultAsync(a => a.SaleID == id);

    public async Task<IEnumerable<SaleDto>> GetAll() => await _context.Sales.ToListAsync();


    public async Task Add(SaleDto Sale)
    {
        _context.Sales.Add(Sale);
        await _context.SaveChangesAsync();
    }

    public async Task Update(SaleDto Sale)
    {
        _context.Sales.Update(Sale);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var Sale = await _context.Sales.FirstOrDefaultAsync(a => a.SaleID == id);
        if (Sale != null)
        {
            _context.Sales.Remove(Sale);
            await _context.SaveChangesAsync();
        }
    }
}
