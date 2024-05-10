using Microsoft.EntityFrameworkCore;

namespace LSPApi.DataLayer;
public class Repository<T> : IRepository<T> where T : class
{
    public DbContext DbContext { get; set; }

    public Repository(DbContext dbContext)
    {
        DbContext = dbContext;
    }

#pragma warning disable CS8603 // Possible null reference return.
    public async Task<T> GetByIdAsync(int id) =>
        await DbContext.Set<T>().FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.


    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbContext.Set<T>().ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await DbContext.Set<T>().AddAsync(entity);
        await DbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        DbContext.Entry(entity).State = EntityState.Modified;
        await DbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(T entity)
    {
        DbContext.Set<T>().Remove(entity);
        await DbContext.SaveChangesAsync();
    }

}
