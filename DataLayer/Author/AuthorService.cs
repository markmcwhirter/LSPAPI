//using LSPApi.DataLayer;
//using LSPApi.DataLayer.Model;

//namespace LSPApi.DataLayer;
//public class AuthorService : IAuthorService
//{
//    private readonly IRepository<AuthorDto> _AuthorRepository;

//    public AuthorService(IRepository<AuthorDto> AuthorRepository)
//    {
//        _AuthorRepository = AuthorRepository;
//    }

//    public async Task CreateAuthor(AuthorDto a)
//    {
//        var newAuthor = new AuthorDto
//        {
//            Address1 = a.Address1,
//            Address2 = a.Address2,
//            Admin = a.Admin,
//            AuthorID = a.AuthorID,
//            Bio = a.Bio,
//            BusinessPhone = a.BusinessPhone,
//            CellPhone = a.CellPhone,
//            City = a.City,
//            Country = a.Country,
//            DateCreated = a.DateCreated,
//            DateUpdated = a.DateUpdated,
//            Email = a.Email,
//            FirstName = a.FirstName,
//            HomePhone = a.HomePhone,
//            LastName = a.LastName,
//            MiddleName = a.MiddleName,
//            Password = a.Password,
//            Prefix = a.Prefix,
//            State = a.State,
//            Suffix = a.Suffix,
//            Username = a.Username,
//            ZIP = a.ZIP
//        };

//        await _AuthorRepository.AddAsync(newAuthor);
//    }

//    public async Task UpdateAuthor(AuthorDto a)
//    {
//        var existingAuthor = await _AuthorRepository.GetByIdAsync(a.AuthorID);

//        if (existingAuthor == null)
//        {
//            throw new ArgumentException("Author not found");
//        }

//        existingAuthor.Address1 = a.Address1;
//        existingAuthor.Address2 = a.Address2;
//        existingAuthor.Admin = a.Admin;
//        existingAuthor.AuthorID = a.AuthorID;
//        existingAuthor.Bio = a.Bio;
//        existingAuthor.BusinessPhone = a.BusinessPhone;
//        existingAuthor.CellPhone = a.CellPhone;
//        existingAuthor.City = a.City;
//        existingAuthor.Country = a.Country;
//        existingAuthor.DateCreated = a.DateCreated;
//        existingAuthor.DateUpdated = a.DateUpdated;
//        existingAuthor.Email = a.Email;
//        existingAuthor.FirstName = a.FirstName;
//        existingAuthor.HomePhone = a.HomePhone;
//        existingAuthor.LastName = a.LastName;
//        existingAuthor.MiddleName = a.MiddleName;
//        existingAuthor.Password = a.Password;
//        existingAuthor.Prefix = a.Prefix;
//        existingAuthor.State = a.State;
//        existingAuthor.Suffix = a.Suffix;
//        existingAuthor.Username = a.Username;
//        existingAuthor.ZIP = a.ZIP;


//        await _AuthorRepository.UpdateAsync(existingAuthor);
//    }

//    public async Task<AuthorDto> GetAuthorById(int id)
//    {
//        return await _AuthorRepository.GetByIdAsync(id);
//    }

//    public async Task<IEnumerable<AuthorDto>> GetAllAuthors()
//    {
//        return await _AuthorRepository.GetAllAsync();
//    }

//    public async Task DeleteAuthor(int id)
//    {
//        var existingAuthor = await _AuthorRepository.GetByIdAsync(id);

//        if (existingAuthor == null)
//        {
//            throw new ArgumentException("Author not found");
//        }

//        await _AuthorRepository.DeleteAsync(existingAuthor);
//    }
//}
