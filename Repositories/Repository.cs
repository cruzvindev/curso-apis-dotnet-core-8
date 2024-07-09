using APICatalogo.Context;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace APICatalogo.Repositories;

public class Repository<T> : IRepository<T> where T : class //Indica que T deve ser uma classe
{
    protected readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public IEnumerable<T> GetAll()
    {
        return _context.Set<T>().AsNoTracking().ToList(); //Set retorna um conjunto do tipo especificado direto do banco que eu posso manipular
    }

    public T? Get(Expression<Func<T, bool>> predicate)
    {
       return _context.Set<T>().FirstOrDefault(predicate);
    }

    public T Create(T entity)
    {
        _context.Set<T>().Add(entity);
       // _context.SaveChanges();

        return entity;
    }

    public T Update(T entity)
    {
       _context.Set<T>().Update(entity);
        // _context.Entry(entity).State = EntityState.Modified;
        //_context.SaveChanges();

        return entity;
    }

    public T Delete(T entity)
    {
       _context.Set<T>().Remove(entity);
       // _context.SaveChanges();

        return entity;
    }
}
