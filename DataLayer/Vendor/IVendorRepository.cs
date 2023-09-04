using LSPApi.DataLayer.Model;

namespace LSPApi.DataLayer;

public interface IVendorRepository
{
    Task<VendorDto> GetById(int id);
    Task<IEnumerable<VendorDto>> GetAll();
    Task Add(VendorDto Vendor);
    Task Update(VendorDto Vendor);
    Task Delete(int id);
}

