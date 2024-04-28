using LSPApi.DataLayer.Model;

namespace LSPApi.DataLayer;

public interface IAuthorRepository
{
    Task<AuthorDto> GetById(int id);
    public bool CheckUsername(string username);
    Task<List<AuthorListResultsModel>> GetBySearchTerm(AuthorSearchModel authorsearch);

    Task<AuthorDto> GetByUsernamePassword(string username, string password);
    Task<AuthorDto> GetByUsername(string username);
    Task<IEnumerable<AuthorDto>> GetAll();
    Task Add(AuthorDto author);
    Task Update(AuthorDto author);
    Task Delete(int id);

    Task<bool> CheckForUsername(string username);
}

