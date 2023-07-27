using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace HomeBanking.Repositories
{
    public interface IRepositoryBase<T> //Interfaz Genérica (<T>)
    {
        IQueryable<T> FindAll(); //permite obtener un lista de objetos sin filtro.
        IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null);
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
