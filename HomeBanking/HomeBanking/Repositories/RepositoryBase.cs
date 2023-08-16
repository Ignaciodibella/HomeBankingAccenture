using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using System.Linq;
using System;
using Microsoft.EntityFrameworkCore;
using HomeBanking.Data;

/*
 Patrón Repositorio:

- Define una capa intermedia para acceder a los datos (BD) lo cual permite definir un único punto de conexión,
  en vez de hacer accesos desde diferentes partes del código.
  De esta forma la lógica de acceso a datos queda separada de la lógica de negocio, lo cual hace que el
  sistema sea facilmente mantenible.

- Una ventaja de aplicar este patrón y su repositorio base es que si en algún momento se decide modificar
  la BD (de SQL a NoSQL, por ejemplo) solo hay que adaptar este repo base.

Repositorio Base:

- Para implementar el patrón repositorio se define un repositorio base a través de una clase abstracta
  ya que el resto de repositorios (Concretos) heredarán de este para poder hacer uso de los métodos de 
  acceso y modificación de datos.

- Define los métodos genéricos que luego se espcializan en los repositorios concretos.
 
- Recordar que las clases abstractas no son instanciables, sino que vamos a instanciar a sus hijos
  AccountRepository, ClientRepository, etc.

 */


namespace HomeBanking.Repositories
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class   //Se define una clase abstracta paramétrica ya que los hijos serán de distintos tipos (Client/Account/Transaction)
    {                                                                              //Como implementa a la Interfaz IRepositoryBase debe tener todos los métodos que allí se definieron.
                                                                                   //La constraint Where T: Class nos permite asegurar que solo vamos a manejar clases y no otro tipo de datos (int/float/...)
        
        //Hacemos uso del contexto para manejar la comunicación y mapeo (ORM) con la base de datos.
        protected HomeBankingContext RepositoryContext { get; set; } 
        public RepositoryBase(HomeBankingContext repositoryContext) 
        {                                                           
            this.RepositoryContext = repositoryContext;             
        }

        //Polimorfismo sobre el método FindAll con sobrecarga. Uno recibe parámetros y el otro no para aquellas
        //consultas donde necesitamos hacer algún join con otra tabla y simplificar aquellas que no.
        public IQueryable<T> FindAll()
        {
            return this.RepositoryContext.Set<T>().AsNoTrackingWithIdentityResolution(); //permite agilizar las consultas,
        }                                                                                //pero no permite hacer modificaciones sobre los datos,
                                                                                         //por eso se emplea solo en los Get.

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
        public void Create (T entity) //SQL: INSERT INTO <table_name> ([<cols>]) VALUES ([<col_value>])
        {
            this.RepositoryContext.Set<T>().Add(entity);
        }
        public void Update(T entity) //SQL: UPDATE <table_name> SET <attribute> WHERE <condition>
        {
            this.RepositoryContext.Set<T>().Update(entity);
        }
        public void Delete(T entity) //SQL: DROP FROM <table_name> WHERE <condition>
        {
            this.RepositoryContext.Set<T>().Remove(entity);
        }
        public void SaveChanges() //Impacta las operaciones anteriores mediante una transacción, ejecutando un rollback si ocurre algún error.
        {
            this.RepositoryContext.SaveChanges();
        }
    }
}
