using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects;

public class BaseDaoAsync<T> where T : class
{
    private static readonly Lazy<BaseDaoAsync<T>> _instance = new Lazy<BaseDaoAsync<T>>(() => new BaseDaoAsync<T>());

    private BaseDaoAsync() { }

    public static BaseDaoAsync<T> Instance => _instance.Value;

    private async Task ExecuteWithTransactionAsync(Func<BcbpContext, Task> operation)
    {
        await using var context = new BcbpContext();
        await using var transaction = await context.Database.BeginTransactionAsync();

        try
        {
            await operation(context);
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task AddAsync(T entity)
    {
        await ExecuteWithTransactionAsync(async context =>
        {
            await context.Set<T>().AddAsync(entity);
            await context.SaveChangesAsync();
        });
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await ExecuteWithTransactionAsync(async context =>
        {
            await context.Set<T>().AddRangeAsync(entities);
            await context.SaveChangesAsync();
        });
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        await using var context = new BcbpContext();
        return await context.Set<T>().ToListAsync();
    }

    public async Task<T?> GetWithQueryAsync(params Func<IQueryable<T>, IQueryable<T>>[] queryOperations)
    {
        await using var context = new BcbpContext();
        IQueryable<T> query = context.Set<T>();
        foreach (var queryOperation in queryOperations)
        {
            query = queryOperation(query);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<T>> GetAllWithQueryAsync(params Func<IQueryable<T>, IQueryable<T>>[] queryOperations)
    {
        await using var context = new BcbpContext();
        IQueryable<T> query = context.Set<T>();
        foreach (var queryOperation in queryOperations)
        {
            query = queryOperation(query);
        }
        return await query.ToListAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        await ExecuteWithTransactionAsync(async context =>
        {
            var tracker = context.Attach(entity);
            tracker.State = EntityState.Modified;
            await context.SaveChangesAsync();
        });
    }

    public async Task UpdateRangeAsync(IEnumerable<T> entities)
    {
        await ExecuteWithTransactionAsync(async context =>
        {
            context.Set<T>().UpdateRange(entities);
            await context.SaveChangesAsync();
        });
    }

    public async Task DeleteAsync(T entity)
    {
        await ExecuteWithTransactionAsync(async context =>
        {
            context.Set<T>().Remove(entity);
            await context.SaveChangesAsync();
        });
    }

    public async Task DeleteRangeAsync(IEnumerable<T> entities)
    {
        await ExecuteWithTransactionAsync(async context =>
        {
            context.Set<T>().RemoveRange(entities);
            await context.SaveChangesAsync();
        });
    }
}