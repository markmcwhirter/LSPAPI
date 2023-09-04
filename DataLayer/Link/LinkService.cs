//using LSPApi.DataLayer.Model;

//namespace LSPApi.DataLayer;
//public class LinkService : ILinkService
//{
//    private readonly IRepository<LinkDto> _LinkRepository;

//    public LinkService(IRepository<LinkDto> LinkRepository)
//    {
//        _LinkRepository = LinkRepository;
//    }

//    public async Task CreateLink(LinkDto a)
//    {
//        var newLink = new LinkDto
//        {
//            BookID = a.BookID,
//            DateCreated = a.DateCreated,
//            DateUpdated = a.DateUpdated,
//            Link = a.Link,
//            LinkDescription = a.LinkDescription,
//            LinkID = a.LinkID
//        };

//        await _LinkRepository.AddAsync(newLink);
//    }

//    public async Task UpdateLink(LinkDto a)
//    {
//        var existingLink = await _LinkRepository.GetByIdAsync(a.LinkID);

//        if (existingLink == null)
//        {
//            throw new ArgumentException("Link not found");
//        }

//        existingLink.BookID = a.BookID;
//        existingLink.DateCreated = a.DateCreated;
//        existingLink.DateUpdated = a.DateUpdated;
//        existingLink.Link = a.Link;
//        existingLink.LinkDescription = a.LinkDescription;
//        existingLink.LinkID = a.LinkID;


//        await _LinkRepository.UpdateAsync(existingLink);
//    }

//    public async Task<LinkDto> GetLinkById(int id)
//    {
//        return await _LinkRepository.GetByIdAsync(id);
//    }

//    public async Task<IEnumerable<LinkDto>> GetAllLinks()
//    {
//        return await _LinkRepository.GetAllAsync();
//    }

//    public async Task DeleteLink(int id)
//    {
//        var existingLink = await _LinkRepository.GetByIdAsync(id);

//        if (existingLink == null)
//        {
//            throw new ArgumentException("Link not found");
//        }

//        await _LinkRepository.DeleteAsync(existingLink);
//    }
//}
