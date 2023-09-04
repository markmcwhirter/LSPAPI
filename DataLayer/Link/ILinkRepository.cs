using LSPApi.DataLayer.Model;

namespace LSPApi.DataLayer;

public interface ILinkRepository
{
    Task<LinkDto> GetById(int id);
    Task<IEnumerable<LinkDto>> GetAll();
    Task Add(LinkDto Link);
    Task Update(LinkDto Link);
    Task Delete(int id);
}

