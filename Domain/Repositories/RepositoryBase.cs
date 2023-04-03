using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Domain.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : BaseEntity
{
    protected DbContext Context;

    protected RepositoryBase(DbContext context)
    {
        Context = context;
    }

    public IQueryable<T> All()
    {
        return Context.Set<T>().AsNoTracking();
    }

    public IQueryable<T> Where(Expression<Func<T, bool>> expression)
    {
        return Context.Set<T>().Where(expression).AsNoTracking();
    }

    public async Task Create(T entity)
    {
        await Context.Set<T>().AddAsync(entity);
    }

    public void Update(T entity)
    {
        Context.Set<T>().Update(entity);
    }

    public async Task Upsert(T entity, Expression<Func<T, bool>> expression)
    {
        var existing = await Context.Set<T>().FirstOrDefaultAsync(expression);

        if (existing is null)
        {
            Create(entity);
            return;
        }

        Context.Entry(existing).CurrentValues.SetValues(entity);
    }

    public void Delete(T entity)
    {
        Context.Set<T>().Remove(entity);
    }

    public void DeleteById(string id)
    {
        var entity = Context.Set<T>().Find(id);
        if (entity is null) return;
        Context.Set<T>().Remove(entity);
    }

    public void DeleteAll()
    {
        Context.Set<T>().RemoveRange(Context.Set<T>());
    }
}
