﻿using DataLayer;

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
        IQueryable<AuthorDto> queryboth;
        IQueryable<AuthorDto> queryfirst;
        IQueryable<AuthorDto> querylast;
        IQueryable<AuthorDto> query;

        queryboth = _context.Author.AsNoTracking()
            .Where(a => a.LastName.StartsWith(authorsearch.LastName) && a.FirstName.StartsWith(authorsearch.FirstName));

        queryfirst = _context.Author.AsNoTracking()
            .Where(a => a.FirstName.StartsWith(authorsearch.FirstName));

        querylast = _context.Author.AsNoTracking()
            .Where(a => a.LastName.StartsWith(authorsearch.LastName));


        if (authorsearch.LastName.Trim() != "" && authorsearch.FirstName.Trim() != "")
            query = queryboth;
        else if (authorsearch.FirstName.Trim() == "")
            query = querylast;
        else
            query = queryfirst;


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

        return result != null ? result : new List<AuthorListResultsModel>();
    }

    public async Task<AuthorDto> GetById(int id)
    {
        var result = await _context.Author.AsNoTracking().FirstOrDefaultAsync(a => a.AuthorID == id);
        return result != null ? result : new AuthorDto();
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
        var result = _context.Author.AsNoTracking().FirstOrDefault(a => a.Username == username);
        result.Password = "";

        if (result != null) result.Admin = "1";
        return result != null ? result : new AuthorDto();
    }
    public async Task<AuthorDto> GetByUsername(string username)
    {
        var result = await _context.Author.AsNoTracking().FirstOrDefaultAsync(a => a.Username == username);
        return result != null ? result : new AuthorDto();
    }

    public async Task<IEnumerable<AuthorDto>> GetAll() => await _context.Author.AsNoTracking().OrderBy(a => a.LastName).ThenBy(b => b.FirstName).ToListAsync();
    public async Task<bool> CheckForUsername(string username)
    {
        // make sure there are no duplicate usernames
        var dupcheck = _context.Author.AsNoTracking().Any(a => a.Username == username);
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
