using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;

namespace LSPApi.DataLayer;
public class SoldRepository : ISoldRepository
{
    private readonly LSPContext _context;

    public SoldRepository(LSPContext context)
    {
        _context = context;
    }


    public async Task<IEnumerable<SoldDto>> GetAll() => await _context.Sold.ToListAsync();

    public async Task Add(SoldDto Sold)
    {
        _context.Sold.Add(Sold);
        await _context.SaveChangesAsync();
    }

    public async Task Update(SoldDto Sold)
    {
        _context.Sold.Update(Sold);
        await _context.SaveChangesAsync();
    }

}
