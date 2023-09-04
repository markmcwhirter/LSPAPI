using LSPApi.DataLayer.Model;

namespace LSPApi.DataLayer;

public interface IAuthorRepository
{
    Task<AuthorDto> GetById(int id);

    Task<AuthorDto> GetByUsername(string username, string password);
    Task<IEnumerable<AuthorDto>> GetAll();
    Task Add(AuthorDto author);
    Task Update(AuthorDto author);
    Task Delete(int id);

    Task<bool> CheckForUsername(string username);
}

