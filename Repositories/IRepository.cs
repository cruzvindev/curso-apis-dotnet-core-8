using System.Linq.Expressions;

namespace APICatalogo.Repositories;

//Repositório genérico - Cuidado para não violar o princípio ISP do SOLID 
public interface IRepository<T>
{
    IEnumerable<T> GetAll();
   // IQueryable<T> GetAll2(); Retorna uma consulta e não um resultado
    T? Get(Expression<Func<T, bool>> predicate); //Recebe uma expressão lambda que dado um tipo T retorna um valor booleano
    T Create(T entity);
    T Update (T entity);
    T Delete (T entity);

}
