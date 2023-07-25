using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Linq;
using System;
using HomeBanking.Models;
using Microsoft.EntityFrameworkCore;


namespace HomeBanking.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected HomeBankingContext RepositoryContext { get; set; }
        public RepositoryBase(HomeBankingContext repositoryContext) 
        {
            this.RepositoryContext = repositoryContext;
        }
        public IQueryable<T> FindAll()
        { 
            return this.RepositoryContext.Set<T>().AsNoTrackingWithIdentityResolution();
        }
        
        public IQueryable<T> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null) 
        {
            IQueryable<T> queryable = this.RepositoryContext.Set<T>();
            if (includes != null) 
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

        public IQueryable<T> FindByContidion(Expression<Func<T, bool>> expression)
        {
            throw new NotImplementedException();
        }
    }
}
