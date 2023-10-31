using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace userManagerApplication.Repository.Interfaces
{
    public interface IGenericRepository<TEntity>
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(int id);
        TEntity Find(Expression<Func<TEntity,bool>> filter);
        void Add(TEntity data);
        void Delete(int id);
        void Update(TEntity data);
        void Save();

    }
}
