using DataLayer;

using LSPApi.DataLayer.Model;

using Microsoft.EntityFrameworkCore;

using System.Text;

namespace LSPApi.DataLayer;
public class AuthorRepository : IAuthorRepository
{
    private readonly LSPContext _context;

    public AuthorRepository(LSPContext context)
    {
        _context = context;
    }

    public async Task<List<AuthorListResultsModel>> GetBySearchTerm(AuthorSearchModel authorsearch)
    {
        IQueryable<AuthorDto>? queryboth = null;
        IQueryable<AuthorDto>? queryfirst = null;
        IQueryable<AuthorDto>? querylast = null;
        IQueryable<AuthorDto>? querynone = _context.Author;

        IQueryable<AuthorDto>? query = null;

        List<AuthorListResultsModel> result = new();

        try
        {
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            if (string.IsNullOrEmpty(authorsearch.LastName.Trim()) && string.IsNullOrEmpty(authorsearch.FirstName.Trim()))

                queryboth = _context.Author;

            else if (!string.IsNullOrEmpty(authorsearch.LastName.Trim()) && !string.IsNullOrEmpty(authorsearch.FirstName.Trim()))
                queryboth = _context.Author
                    .Where(a => a.LastName.StartsWith(authorsearch.LastName) && a.FirstName.StartsWith(authorsearch.FirstName));

            else if (authorsearch.FirstName != null)
                queryfirst = _context.Author
                .Where(a => a.FirstName.StartsWith(authorsearch.FirstName));

            else if (authorsearch.LastName != null)
                querylast = _context.Author
                .Where(a => a.LastName.StartsWith(authorsearch.LastName));

            else
                querynone = _context.Author.Where(a => a.LastName != null);

            if (string.IsNullOrEmpty(authorsearch.LastName.Trim()) && string.IsNullOrEmpty(authorsearch.FirstName.Trim()))
                query = querynone;
            else if (authorsearch.LastName.Trim() != "" && authorsearch.FirstName.Trim() != "")
                query = queryboth;
            else if (authorsearch.LastName.Trim() != "")
                query = querylast;
            else if (authorsearch.FirstName.Trim() != "")
                query = queryfirst;
            else
                query = querynone;

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
                        EMail = p.Email
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


    public async Task<AuthorDto> GetByUsernamePassword(string username, string password)
    {
        // decrypt password here
        //string incoming = password.Replace('_', '/').Replace('-', '+');
        //switch (password.Length % 4)
        //{
        //    case 2: incoming += "=="; break;
        //    case 3: incoming += "="; break;
        //}
        //byte[] bytes = Convert.FromBase64String(incoming);


        //string decrypted = await new EncryptionService().DecryptAsync(bytes);

        // var result = await _context.Author.FirstOrDefaultAsync(a => a.Username == username && a.Password == decrypted);
        var result = await _context.Author.FirstOrDefaultAsync(a => a.Username == username);

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
