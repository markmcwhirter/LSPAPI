using LSPApi.DataLayer.Model;

namespace LSPApi.DataLayer;

public interface ISoldRepository
{
    Task<IEnumerable<SoldDto>> GetAll();
    Task Add(SoldDto Sold);
    Task Update(SoldDto Sold);
}

