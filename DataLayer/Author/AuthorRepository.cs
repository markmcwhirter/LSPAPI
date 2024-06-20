using LSPApi.DataLayer.Model;
using DataLayer.Model;

using Microsoft.EntityFrameworkCore;

using System.Text.Json;
using LinqKit;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.Extensions.Logging;

namespace LSPApi.DataLayer;
public class AuthorRepository : IAuthorRepository
{
    private readonly LSPContext _context;
    private readonly ILogger<AuthorRepository> _logger;

    public AuthorRepository(LSPContext context, ILogger<AuthorRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<AuthorListResultsModel>> GetAuthors(int startRow, int endRow, string sortColumn, string sortDirection, string filter)
    {
        List<AuthorListResultsModel> result = [];

        try
        {
            var query = _context.Author.AsQueryable(); // Start with IQueryable

            sortColumn = sortColumn == "null" ? "AUTHORID" : sortColumn.ToUpper();
            sortDirection = sortDirection == "null" ? "ASC" : sortDirection.ToUpper();

            FilterModel? filterList = new();

            // parse filter and add to a where clause
            if (!string.IsNullOrEmpty(filter) && filter != "{}")
                filterList = JsonSerializer.Deserialize<FilterModel>(filter);

            if (filterList != null)
            {
                if (filterList.authorID != null)
                {
                    // TODO: implement {"authorID":{"filterType":"number","type":"inRange","filter":500,"filterTo":600}}

                    string? filtertype = filterList.authorID.type.ToLower();
                    int filterValue = filterList.authorID.filter;

                    if (filterList.authorID.type.ToLower().Equals("inrange"))
                        query = query.Where(a => a.AuthorID >= filterList.authorID.filter && a.AuthorID <= filterList.authorID.filterTo);
                    else
                    {

                        if (filtertype.Equals("equals"))
                            query = query.Where(a => a.AuthorID == filterValue);
                        else if (filtertype.Equals("doesnotequal"))
                            query = query.Where(a => a.AuthorID != filterValue);
                        else if (filtertype.Equals("greaterthan"))
                            query = query.Where(a => a.AuthorID > filterValue);
                        else if (filtertype.Equals("greaterthanorequal"))
                            query = query.Where(a => a.AuthorID >= filterValue);
                        else if (filtertype.Equals("lessthan"))
                            query = query.Where(a => a.AuthorID < filterValue);
                        else if (filtertype.Equals("lessthanorequal"))
                            query = query.Where(a => a.AuthorID <= filterValue);
                        else if (filtertype.Equals("between"))
                            query = query.Where(a => a.AuthorID >= filterValue);
                        else if (filtertype.Equals("blank"))
                            query = query.Where(a => a.AuthorID == 0);
                        else if (filtertype.Equals("notblank"))
                            query = query.Where(a => a.AuthorID != 0);
                    }
                }
               
                if (filterList.lastName != null)
                    query = query.BuildStringQuery("LastName", filterList.lastName.type.ToLower(), filterList.lastName.filter);
                if (filterList.firstName != null)
                    query = query.BuildStringQuery("FirstName", filterList.firstName.type.ToLower(), filterList.firstName.filter);
                if (filterList.eMail != null)
                    query = query.BuildStringQuery("Email", filterList.eMail.type.ToLower(), filterList.eMail.filter);
            }

            if (sortColumn == "LASTNAME")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.LastName) :
                    query.OrderByDescending(a => a.LastName);
            else if (sortColumn == "FIRSTNAME")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.FirstName) : query.OrderByDescending(a => a.FirstName);
            else if (sortColumn == "AUTHORID")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.AuthorID) : query.OrderByDescending(a => a.AuthorID);
            else if (sortColumn == "EMAIL")
                query = sortDirection == "ASC" ? query.OrderBy(a => a.Email) : query.OrderByDescending(a => a.Email);


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
                        Notes = p.Notes,
                        InfoLink = p.AuthorID.ToString(),
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

    public async Task<List<AuthorListResultsModel>> GetBySearchTerm(AuthorSearchModel? authorsearch)
    {
        IQueryable<AuthorDto>? query = null;

        List<AuthorListResultsModel> result = [];

        try
        {
            if (authorsearch == null)
            {
                authorsearch = new AuthorSearchModel();
                return result;
            }

#pragma warning disable CS8602 // Dereference of a possibly null reference.

            authorsearch.FirstName = string.IsNullOrEmpty(authorsearch?.FirstName) ? " " : authorsearch?.FirstName.Trim();
            authorsearch.LastName = string.IsNullOrEmpty(authorsearch?.LastName) ? " " : authorsearch?.LastName.Trim();
            authorsearch.Direction = string.IsNullOrEmpty(authorsearch?.Direction) ? "ASC" : authorsearch?.Direction.Trim();
            authorsearch.SortOrder = string.IsNullOrEmpty(authorsearch?.SortOrder) ? "LASTNAME" : authorsearch?.SortOrder.Trim();


            bool firstNameEmpty = string.IsNullOrEmpty(authorsearch?.FirstName);
            bool lastNameEmpty = string.IsNullOrEmpty(authorsearch?.LastName);



            if (lastNameEmpty && firstNameEmpty)
                query = _context.Author;

            else if (!lastNameEmpty && !firstNameEmpty)
                query = _context.Author.Where(item => EF.Functions.Like(item.LastName.ToLower(), $"%{authorsearch.LastName.ToLower()}%") &&
                        EF.Functions.Like(item.FirstName.ToLower(), $"%{authorsearch.FirstName.ToLower()}%"));

            else if (!string.IsNullOrEmpty(authorsearch?.FirstName))
                query = _context.Author.Where(item => EF.Functions.Like(item.FirstName.ToLower(), $"%{authorsearch.FirstName.ToLower()}%"));

            else if (!string.IsNullOrEmpty(authorsearch?.LastName))
                query = _context.Author.Where(item => EF.Functions.Like(item.LastName.ToLower(), $"%{authorsearch.LastName.ToLower()}%"));

            else
                query = _context.Author.Where(a => a.LastName != null);


            if (authorsearch.SortOrder.Equals("LASTNAME", StringComparison.CurrentCultureIgnoreCase))
                query = authorsearch.Direction.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderBy(a => a.LastName) :
                    query.OrderByDescending(a => a.LastName);
            else if (authorsearch.SortOrder.Equals("FIRSTNAME", StringComparison.CurrentCultureIgnoreCase))
                query = authorsearch.Direction.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderBy(a => a.FirstName) : query.OrderByDescending(a => a.FirstName);
            else if (authorsearch.SortOrder.Equals("AUTHORID", StringComparison.CurrentCultureIgnoreCase))
                query = authorsearch.Direction.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderBy(a => a.AuthorID) : query.OrderByDescending(a => a.AuthorID);
            else if (authorsearch.SortOrder.Equals("EMAIL", StringComparison.CurrentCultureIgnoreCase))
                query = authorsearch.Direction.Equals("ASC", StringComparison.CurrentCultureIgnoreCase) ? query.OrderBy(a => a.Email) : query.OrderByDescending(a => a.Email);



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
        catch (Exception ex)
        {
            _ = ex.Message;
        }
        return result ?? [];

    }

    public async Task<AuthorDto> GetById(int id)
    {
        var result = await _context.Author.FirstOrDefaultAsync(a => a.AuthorID == id);
        return result ?? new AuthorDto();
    }

    public bool CheckUsername(string username) =>
        _context.Author.Where(a => a.Username == username).Any();

    public string GetUsername(string email) =>
        string.IsNullOrEmpty(email) ? "" : _context.Author.Where(a => a.Email == email).FirstOrDefault().Username;

    public string GetEmail(string username) =>
        string.IsNullOrEmpty(username) ? "" : _context.Author.Where(a => a.Username == username).FirstOrDefault().Email;

#pragma warning restore CS8602 // Dereference of a possibly null reference

    public async Task<AuthorDto> GetByUsernamePassword(string username, string password)
    {
        _logger.LogInformation($"GetByUsernamePassword: {username}");        // decrypt password here
        string incoming = password.Replace('_', '/').Replace('-', '+');
        switch (password.Length % 4)
        {
            case 2: incoming += "=="; break;
            case 3: incoming += "="; break;
        }
        byte[] bytes = Convert.FromBase64String(incoming);


        string decrypted = await new EncryptionService().DecryptAsync(bytes);

        var result = await _context.Author.FirstOrDefaultAsync(a => a.Username == username && a.Password == decrypted);


        return result ?? new AuthorDto();
    }
    public async Task<AuthorDto> GetByUsername(string username)
    {
        var result = await _context.Author.FirstOrDefaultAsync(a => a.Username == username);
        return result ?? new AuthorDto();
    }

    public async Task<List<AuthorListResultsModel>> GetAll() =>

        await _context.Author.OrderBy(a => a.AuthorID)
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



    public async Task<bool> CheckForUsername(string username) =>
        await _context.Author.AnyAsync(a => a.Username == username);

    public async Task Add(AuthorDto author)
    {
        int maxAge = _context.Author.Max(p => p.AuthorID);
        author.AuthorID = maxAge + 1;
        author.DateCreated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");


        _context.Author.Add(author);
        await _context.SaveChangesAsync();
    }

    public async Task Update(AuthorDto author)
    {
        var current = _context.Author.Find(author.AuthorID);

        author.DateUpdated = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss");
        author.Username = current.Username;

        if (author.Admin == null || author.Password == null)
        {
            author.Admin ??= current.Admin;
            author.Password ??= current.Password;
        }

        if (current != null)
        {
            _context.Entry(current).CurrentValues.SetValues(author);
        }
        else
        {
            _context.Author.Add(author);
        }

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
