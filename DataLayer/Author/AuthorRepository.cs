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
        IQueryable<AuthorDto>? querynone = null;

        IQueryable<AuthorDto>? query = null;

        if (authorsearch.LastName != null && authorsearch.FirstName != null)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            queryboth = _context.Author
                .Where(a => a.LastName.StartsWith(authorsearch.LastName) && a.FirstName.StartsWith(authorsearch.FirstName));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        if (authorsearch.FirstName != null)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            queryfirst = _context.Author
            .Where(a => a.FirstName.StartsWith(authorsearch.FirstName));
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        if (authorsearch.LastName != null)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            querylast = _context.Author
            .Where(a => a.LastName.StartsWith(authorsearch.LastName));


        querynone = _context.Author;

        if (authorsearch.LastName.Trim() == "" && authorsearch.FirstName.Trim() == "")
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

        string[]? sortargs = null;

        if(authorsearch.SortOrder  != null)
            sortargs =  authorsearch.SortOrder.Split(' ');

        string? sortfield = null;

        if(sortargs != null )
            sortfield = sortargs[0].ToUpper().Trim();

        string? sortdirection = null;
        if( sortargs != null)
            sortdirection = sortargs.Length > 1 ? sortargs[1].ToUpper().Trim() : "ASC";

        if (sortfield == "LASTNAME")
        {
            if (sortdirection == "ASC")
#pragma warning disable CS8604 // Possible null reference argument.
                query = query.OrderBy(a => a.LastName);

            else
                query = query.OrderByDescending(a => a.LastName);
        }
        else if (sortfield == "FIRSTNAME")
        {
            if (sortdirection == "ASC")
                query = query.OrderBy(a => a.FirstName);
            else
                query = query.OrderByDescending(a => a.FirstName);
        }


        var result = await query
                .Select(p => new AuthorListResultsModel
                {
                    AuthorID = p.AuthorID,
                    FirstName = p.FirstName,
                    LastName = p.LastName,
                    MiddleName = p.MiddleName,
                    Prefix = p.Prefix,
                    Suffix = p.Suffix
                })
                .ToListAsync();
#pragma warning restore CS8604 // Possible null reference argument.
        return result ?? new List<AuthorListResultsModel>();
    }

    public async Task<AuthorDto> GetById(int id)
    {
        var result = await _context.Author.FirstOrDefaultAsync(a => a.AuthorID == id);
        return result ?? new AuthorDto();
    }
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
