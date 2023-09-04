using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;

namespace LSPApi.DataLayer;
public class LinkRepository : ILinkRepository
{
    private readonly LSPContext _context;

    public LinkRepository(LSPContext context)
    {
        _context = context;
    }

    public async Task<LinkDto> GetById(int id)
    {
        var result = await _context.Links.FirstOrDefaultAsync(a => a.BookID == id);
        return result != null ? result : new LinkDto();
    }


    public async Task<IEnumerable<LinkDto>> GetAll() => await _context.Links.ToListAsync();

    public async Task Add(LinkDto Link)
    {
        _context.Links.Add(Link);
        await _context.SaveChangesAsync();
    }

    public async Task Update(LinkDto Link)
    {
        _context.Links.Update(Link);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var Link = await _context.Links.FirstOrDefaultAsync(a => a.LinkID == id);
        if (Link != null)
        {
            _context.Links.Remove(Link);
            await _context.SaveChangesAsync();
        }
    }
}
