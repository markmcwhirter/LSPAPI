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

    public async Task<AuthorDto> GetById(int id)
    {
        var result = await _context.Author.FirstOrDefaultAsync(a => a.AuthorID == id);
        return result != null ? result : new AuthorDto();
    }
    public async Task<AuthorDto> GetByUsername(string username, string password)
    {
        // decrypt password here
        string incoming = password.Replace('_', '/').Replace('-', '+');
        switch (password.Length % 4)
        {
            case 2: incoming += "=="; break;
            case 3: incoming += "="; break;
        }
        byte[] bytes = Convert.FromBase64String(incoming);
        //string originalText = Encoding.ASCII.GetString(bytes);

        string decrypted = await new EncryptionService().DecryptAsync(bytes);

        var result = await _context.Author.FirstOrDefaultAsync(a => a.Username == username && a.Password == decrypted);
        return result != null ? result : new AuthorDto();
    }

    public async Task<IEnumerable<AuthorDto>> GetAll() => await _context.Author.OrderBy(a => a.LastName).ThenBy(b => b.FirstName).ToListAsync();
    public async Task<bool> CheckForUsername(string username)
    {
        // make sure there are no duplicate usernames
        var dupcheck = _context.Author.Any(a => a.Username == username);
        return dupcheck ? true : false;
    }

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
