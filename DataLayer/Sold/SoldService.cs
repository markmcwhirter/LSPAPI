//using LSPApi.DataLayer.Model;

//using System.Net;

//namespace LSPApi.DataLayer;
//public class SoldService : ISoldService
//{
//    private readonly IRepository<SoldDto> _SoldRepository;

//    public SoldService(IRepository<SoldDto> SoldRepository)
//    {
//        _SoldRepository = SoldRepository;
//    }

//    public async Task CreateSold(SoldDto a)
//    {
//        var newLink = new SoldDto
//        {
//            BookID = a.BookID,
//            LinkDescription = a.LinkDescription,
//            LinkID = a.LinkID
//        };

//        await _SoldRepository.AddAsync(newLink);
//    }

//    public async Task UpdateSold(SoldDto a)
//    {
//        var existingLink = await _SoldRepository.GetByIdAsync(a.LinkID);

//        if (existingLink == null)
//        {
//            throw new ArgumentException("Link not found");
//        }

//        existingLink.BookID = a.BookID;
//        existingLink.LinkDescription = a.LinkDescription;
//        existingLink.LinkID = a.LinkID;

//        await _SoldRepository.UpdateAsync(existingLink);
//    }

//    public async Task<SoldDto> GetLinkById(int id)
//    {
//        return await _SoldRepository.GetByIdAsync(id);
//    }

//    public async Task<IEnumerable<SoldDto>> GetAllSold()
//    {
//        return await _SoldRepository.GetAllAsync();
//    }

//    public async Task DeleteLink(int id)
//    {
//        var existingLink = await _SoldRepository.GetByIdAsync(id);

//        if (existingLink == null)
//        {
//            throw new ArgumentException("Link not found");
//        }

//        await _SoldRepository.DeleteAsync(existingLink);
//    }
//}
