using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using HomeBanking.Data;

namespace HomeBanking.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        //Hacemos uso del contexto para manejar la comunicación y mapeo (ORM) con la base de datos.
        protected HomeBankingContext RepositoryContext { get; set; } 
        public RepositoryBase(HomeBankingContext repositoryContext) //Constructor. Cuando cree una instancia de esta clase (algun hijo)
        {                                                           //Voy a necesitar pasarles algo de tipo RepositoryContext, haciendo que su contexto
            this.RepositoryContext = repositoryContext;             //sea de tipo HomeBankingContext.
        }
        public IQueryable<T> FindAll()
        { 
            return this.RepositoryContext.Set<T>().AsNoTrackingWithIdentityResolution();
        }
        
        public IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null) 
        {
            IQueryable<T> queryable = this.RepositoryContext.Set<T>();
            if (includes != null) //Si le pasamos algun includes, como el de traer las cuentas de cada cliente junto con el cliente en sí.
            {
                queryable = includes(queryable);
            }
            return queryable.AsNoTrackingWithIdentityResolution();
        }

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression)
        {
            return this.RepositoryContext.Set<T>().Where(expression).AsNoTrackingWithIdentityResolution();
        }
        public void Create (T entity) 
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }
        public void Update(T entity)
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }
        public void Delete(T entity)
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }
        public void SaveChanges()
        {
            this.RepositoryContext.SaveChanges();
        }
    }
}
