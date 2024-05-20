using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;

namespace LSPApi.DataLayer;
public class AuthorRepository : IAuthorRepository
{
    private readonly LSPContext _context;

    public AuthorRepository(LSPContext context)
    {
        _context = context;
    }

    public async Task<List<AuthorListResultsModel>> GetAuthors(int startRow, int endRow, string sortColumn, string sortDirection)
    {
        List<AuthorListResultsModel> result = new();

        try
        {
            var query = _context.Author.AsQueryable(); // Start with IQueryable

            sortColumn = sortColumn == "null" ? "AuthorID" : sortColumn;
            sortColumn = sortColumn.ToUpper();

            sortDirection = sortDirection == "null" ? "ASC" : sortDirection.ToUpper();


            if (sortColumn == "LASTNAME")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.LastName) :
                    query.OrderByDescending(a => a.LastName);
            else if (sortColumn == "FIRSTNAME")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.FirstName) : query.OrderByDescending(a => a.FirstName);
            else if (sortColumn == "AUTHORID")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.AuthorID) : query.OrderByDescending(a => a.AuthorID);
            else if (sortColumn == "EMAIL")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.Email) : query.OrderByDescending(a => a.Email);


            // List<AuthorDto> result = query.Skip(startRow).Take(endRow - startRow).ToList();
            result = await query.Skip(startRow).Take(endRow - startRow)
                    .Select(p => new AuthorListResultsModel
                    {
                        AuthorID = p.AuthorID,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        MiddleName = p.MiddleName,
                        Prefix = p.Prefix,
                        Suffix = p.Suffix,
                        EMail = p.Email,
                        EditLink = p.AuthorID.ToString(),
                        DeleteLink = p.AuthorID.ToString()
                    })
                    .ToListAsync();

        }
        catch (Exception ex) 
        {
            _ = ex.Message;
        }


        return result;

    }

    public async Task<List<AuthorListResultsModel>> GetBySearchTerm(AuthorSearchModel authorsearch)
    {
        IQueryable<AuthorDto>? query = null;

        List<AuthorListResultsModel> result = new();

        try
        {
            authorsearch.FirstName = authorsearch.FirstName.Trim();
            authorsearch.LastName = authorsearch.LastName.Trim();
            bool firstNameEmpty = string.IsNullOrEmpty(authorsearch.FirstName);
            bool lastNameEmpty = string.IsNullOrEmpty(authorsearch.LastName);


#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (lastNameEmpty && firstNameEmpty)
                query = _context.Author;

            else if (!lastNameEmpty && !firstNameEmpty)
                //query = _context.Author.Where(a => a.LastName.StartsWith(authorsearch.LastName) && a.FirstName.StartsWith(authorsearch.FirstName));
                query = _context.Author.Where(item => EF.Functions.Like(item.LastName.ToLower(), $"%{authorsearch.LastName.ToLower()}%") &&
                        EF.Functions.Like(item.FirstName.ToLower(), $"%{authorsearch.FirstName.ToLower()}%"));

            else if (!firstNameEmpty)
                query = _context.Author.Where(item => EF.Functions.Like(item.FirstName.ToLower(), $"%{authorsearch.FirstName.ToLower()}%"));

            else if (!lastNameEmpty)
                query = _context.Author.Where(item => EF.Functions.Like(item.LastName.ToLower(), $"%{authorsearch.LastName.ToLower()}%"));

            else
                query = _context.Author.Where(a => a.LastName != null);


#pragma warning restore CS8602 // Dereference of a possibly null reference.

#pragma warning disable CS8604 // Possible null reference argument.

            if (authorsearch.SortOrder.ToUpper() == "LASTNAME")
                query = authorsearch.Direction.ToUpper() == "ASC" ? query.OrderBy(a => a.LastName) :
                    query.OrderByDescending(a => a.LastName);
            else if (authorsearch.SortOrder.ToUpper() == "FIRSTNAME")
                query = authorsearch.Direction.ToUpper() == "ASC" ? query.OrderBy(a => a.FirstName) : query.OrderByDescending(a => a.FirstName);
            else if (authorsearch.SortOrder.ToUpper() == "AUTHORID")
                query = authorsearch.Direction.ToUpper() == "ASC" ? query.OrderBy(a => a.AuthorID) : query.OrderByDescending(a => a.AuthorID);
            else if (authorsearch.SortOrder.ToUpper() == "EMAIL")
                query = authorsearch.Direction.ToUpper() == "ASC" ? query.OrderBy(a => a.Email) : query.OrderByDescending(a => a.Email);

            result = await query
                    .Select(p => new AuthorListResultsModel
                    {
                        AuthorID = p.AuthorID,
                        FirstName = p.FirstName,
                        LastName = p.LastName,
                        MiddleName = p.MiddleName,
                        Prefix = p.Prefix,
                        Suffix = p.Suffix,
                        EMail = p.Email,
                        EditLink = $"<a href='ProfileModify?id={p.AuthorID}'>Edit</a>",
                        DeleteLink = $"<a href='DeleteAuthor?id={p.AuthorID}'>Delete</a>"
                    })
                    .ToListAsync();
        }
        catch(Exception ex)
        {
            _ = ex.Message;
        }
        return result ?? new List<AuthorListResultsModel>();

#pragma warning restore CS8604 // Possible null reference argument.
    }

    public async Task<AuthorDto> GetById(int id)
    {
        var result = await _context.Author.FirstOrDefaultAsync(a => a.AuthorID == id);
        return result ?? new AuthorDto();
    }

    public bool CheckUsername(string username) =>
        _context.Author.Where(a => a.Username == username).Any();

    public string GetUsername(string email) =>
     _context.Author.Where(a => a.Email == email).FirstOrDefault().Username;

    public string GetEmail(string username) =>
        _context.Author.Where(a => a.Username == username).FirstOrDefault().Email;

    public async Task<AuthorDto> GetByUsernamePassword(string username, string password)
    {
        // decrypt password here
        string incoming = password.Replace('_', '/').Replace('-', '+');
        switch (password.Length % 4)
        {
            case 2: incoming += "=="; break;
            case 3: incoming += "="; break;
        }
        byte[] bytes = Convert.FromBase64String(incoming);


        string decrypted = await new EncryptionService().DecryptAsync(bytes);

        var result = await _context.Author.FirstOrDefaultAsync(a => a.Username == username && a.Password == decrypted);
        //var result = await _context.Author.FirstOrDefaultAsync(a => a.Username == username);

        return result ?? new AuthorDto();
    }
    public async Task<AuthorDto> GetByUsername(string username)
    {
        var result = await _context.Author.FirstOrDefaultAsync(a => a.Username == username);
        return result ?? new AuthorDto();
    }

    public async Task<IEnumerable<AuthorDto>> GetAll() => await _context.Author.OrderBy(a => a.LastName).ThenBy(b => b.FirstName).ToListAsync();
    public async Task<bool> CheckForUsername(string username) =>
        await _context.Author.AnyAsync(a => a.Username == username);

    public async Task Add(AuthorDto author)
    {
        int maxAge = _context.Author.Max(p => p.AuthorID);
        author.AuthorID = maxAge + 1;
        author.DateCreated = DateTime.Now.ToString();


        _context.Author.Add(author);
        await _context.SaveChangesAsync();
    }

    public async Task Update(AuthorDto author)
    {
        _context.Set<AuthorDto>().Attach(author);
        _context.Entry(author).State = EntityState.Modified;

        _context.Author.Update(author);
        await _context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var author = await _context.Author.FirstOrDefaultAsync(a => a.AuthorID == id);
        if (author != null)
        {
            _context.Author.Remove(author);
            await _context.SaveChangesAsync();
        }
    }
}
