using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Market.DAL.Repositories
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        private DbSet<TEntity> entitySet;

        protected MarketDB Context { get; private set; }

        public GenericRepository(MarketDB context)
        {
            this.Context = context;
            this.entitySet = this.Context.Set<TEntity>();
        }

        /// <summary>
        /// Gets the entities that can pass the filters, and return them in a certain order.
        /// </summary>
        /// <param name="filter">Defines which kind of entities can meet the requirement.</param>
        /// <param name="orderBy">Tells how to order the results.</param>
        /// <param name="includeProperties">Tells which properties we need to include in the result.</param>
        /// <returns>The matched data in a collection.</returns>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = this.entitySet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).AsQueryable();
            }
            else
            {
                return query.AsQueryable();
            }
        }

        /// <summary>
        /// Gets an entity with a specific Id.
        /// </summary>
        /// <param name="id">Tells the Id of which the wanted entity.</param>
        /// <returns>The matched entity.</returns>
        public virtual TEntity GetById(object id)
        {
            return this.entitySet.Find(id);
        }

        /// <summary>
        /// Insert a new entity to repository.
        /// </summary>
        /// <param name="entity">Represents the new record.</param>
        /// <returns>the new entity in repository.</returns>
        public virtual TEntity Insert(TEntity entity)
        {
            this.entitySet.Add(entity);
            return entity;
        }


        /// <summary>
        /// Remove a record with a specific Id from repository.
        /// </summary>
        /// <param name="id">Tells the Id of wanted entity.</param>
        public virtual void Delete(object id)
        {
            TEntity entityToDelete = this.entitySet.Find(id);
            this.Delete(entityToDelete);
        }

        /// <summary>
        /// Remove a record with a specific Id from repository.
        /// </summary>
        /// <param name="entityToDelete">Tells the wanted entity.</param>
        public virtual void Delete(TEntity entityToDelete)
        {
            if (this.Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                this.entitySet.Attach(entityToDelete);
            }

            this.entitySet.Remove(entityToDelete);
        }

        /// <summary>
        /// Delete a bunch of entities in one time.
        /// </summary>
        /// <param name="entities">A collection contains the entities that need to be removed.</param>
        public virtual void BatchDelete(IEnumerable<TEntity> entities)
        {
            foreach (TEntity entity in entities)
            {
                if (this.Context.Entry(entity).State == EntityState.Detached)
                {
                    this.entitySet.Attach(entity);
                }

                this.entitySet.RemoveRange(entities);
            }
        }

        /// <summary>
        /// update an exists entity in repository.
        /// </summary>
        /// <param name="entityToUpdate">Represents the new entity.</param>
        public virtual void Update(TEntity entityToUpdate)
        {
            this.entitySet.Attach(entityToUpdate);
            this.Context.Entry(entityToUpdate).State = EntityState.Modified;
        }
    }
}
