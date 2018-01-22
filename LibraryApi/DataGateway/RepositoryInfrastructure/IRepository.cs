using System;
using System.Collections.Generic;
using System.Deployment.Internal;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DataGateway.EntityModels;

namespace DataGateway.RepositoryInfrastructure
{
    public interface IRepository<TEntity> : IDisposable
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        TEntity GetById<T>(T id) where T:struct;

        void Add(TEntity entity);

        void Delete<T>(T id) where T:struct;

        void Delete(TEntity entity);

        void Update(TEntity id);

    }

}
