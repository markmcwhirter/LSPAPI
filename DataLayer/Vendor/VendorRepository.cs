using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;

namespace LSPApi.DataLayer;
public class VendorRepository : IVendorRepository
{
    private readonly LSPContext _context;

    public VendorRepository(LSPContext context)
    {
        _context = context;
    }

    public async Task<VendorDto> GetById(int id)
    {
        var result = await _context.Vendor.FirstOrDefaultAsync(a => a.VendorID == id);
        return result != null ? result : new VendorDto();
    }

    public async Task<IEnumerable<VendorDto>> GetAll() => await _context.Vendor.ToListAsync();

    public async Task Add(VendorDto Vendor)
    {
        _context.Vendor.Add(Vendor);
        await _context.SaveChangesAsync();
    }

    public async Task Update(VendorDto Vendor)
    {
        _context.Vendor.Update(Vendor);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var Vendor = await _context.Vendor.FirstOrDefaultAsync(a => a.VendorID == id);
        if (Vendor != null)
        {
            _context.Vendor.Remove(Vendor);
            await _context.SaveChangesAsync();
        }
    }
}
