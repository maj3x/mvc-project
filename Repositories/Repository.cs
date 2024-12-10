using Microsoft.EntityFrameworkCore;
using OdevDagitimPortali.Data;

namespace OdevDagitimPortali.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IEnumerable<T> GetAll()
    {
        return _dbSet.ToList();
    }

    public T? GetById(int id)
    {
        return _dbSet.Find(id);
    }

    public void Insert(T entity)
    {
        _dbSet.Add(entity);
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public void Delete(int id)
    {
        var entity = _dbSet.Find(id);
        if (entity != null)
            _dbSet.Remove(entity);
    }

    public void Save()
    {
        _context.SaveChanges();
    }
}
}
