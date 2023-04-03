using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IRepositoryBase<T> where T : class
{
    public IQueryable<T> All();
    public IQueryable<T> Where(Expression<Func<T, bool>> expression);
    public Task Create(T entity);

    public void Update(T entity);
    public void Delete(T entity);
    public void DeleteAll();
    public void DeleteById(string id);
}
