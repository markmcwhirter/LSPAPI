using LSPApi.DataLayer.Model;

using System.Globalization;

namespace LSPApi.DataLayer;

public interface IAuthorRepository
{
    Task<List<AuthorListResultsModel>> GetAuthors(int startRow, int endRow, string sortColumn, string sortDirection, string filter);

   
    Task<AuthorDto> GetById(int id);
    public bool CheckUsername(string username);
    public string GetUsername(string email);
    string GetEmail(string username);

    Task<List<AuthorListResultsModel>> GetBySearchTerm(AuthorSearchModel authorsearch);

    Task<AuthorDto> GetByUsernamePassword(string username, string password);
    Task<AuthorDto> GetByUsername(string username);

    Task<List<AuthorListResultsModel>> GetAll();

    Task Add(AuthorDto author);
    Task Update(AuthorDto author);
    Task Delete(int id);

    Task<bool> CheckForUsername(string username);
}

