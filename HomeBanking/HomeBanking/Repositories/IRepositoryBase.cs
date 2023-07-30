using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace HomeBanking.Repositories
{
    public interface IRepositoryBase<T> //Interfaz Genérica (<T>)
    {
        IQueryable<T> FindAll(); //Permite obtener un lista de objetos sin filtro: SELECT * FROM
        IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null); //Permite obtener una lista de objetos con filtros y/o incluyendo otros elementos de un objeto
                                                                                                     //Parecido a usar un JOIN en SQL.       
        IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression);//SELECT * FROM <nombreTabla> WHERE <condiociones>
        void Create(T entity); //INSERT INTO ... VALUES
        void Update(T entity);
        void Delete(T entity);
        void SaveChanges();
    }
}
