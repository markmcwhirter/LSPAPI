//using LSPApi.DataLayer.Model;

//namespace LSPApi.DataLayer;
//public class VendorService : IVendorService
//{
//    private readonly IRepository<VendorDto> _VendorRepository;

//    public VendorService(IRepository<VendorDto> VendorRepository)
//    {
//        _VendorRepository = VendorRepository;
//    }

//    public async Task CreateVendor(VendorDto a)
//    {
//        var newVendor = new VendorDto
//        {
//            VendorID = a.VendorID,
//            VendorName = a.VendorName,
//            DateCreated = DateTime.Now,
//            DateUpdated = DateTime.Now
//        };

//        await _VendorRepository.AddAsync(newVendor);
//    }

//    public async Task UpdateVendor(VendorDto a)
//    {
//        var existingVendor = await _VendorRepository.GetByIdAsync(a.VendorID);

//        if (existingVendor == null)
//        {
//            throw new ArgumentException("Vendor not found");
//        }

//        existingVendor.VendorID = a.VendorID;
//        existingVendor.VendorName = a.VendorName;
//        existingVendor.DateCreated = DateTime.Now;
//        existingVendor.DateUpdated = DateTime.Now;


//        await _VendorRepository.UpdateAsync(existingVendor);
//    }

//    public async Task<VendorDto> GetVendorById(int id)
//    {
//        return await _VendorRepository.GetByIdAsync(id);
//    }

//    public async Task<IEnumerable<VendorDto>> GetAllVendors()
//    {
//        return await _VendorRepository.GetAllAsync();
//    }

//    public async Task DeleteVendor(int id)
//    {
//        var existingVendor = await _VendorRepository.GetByIdAsync(id);

//        if (existingVendor == null)
//        {
//            throw new ArgumentException("Vendor not found");
//        }

//        await _VendorRepository.DeleteAsync(existingVendor);
//    }
//}
