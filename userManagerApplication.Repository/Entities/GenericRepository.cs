using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using userManagerAplication.Models.Data;
using userManagerApplication.Repository.Interfaces;

namespace userManagerApplication.Repository.Entities
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private UserManagerAplicationContext _context;
        private DbSet<TEntity> _dbSet;

        public GenericRepository(UserManagerAplicationContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public void Add(TEntity data)
        {
            _dbSet.Add(data);
        }

        public void Delete(int id)
        {
            var dataToDelete = _dbSet.Find(id);
            if (dataToDelete != null)
            {
                _dbSet.Remove(dataToDelete);
            }
        }

        public TEntity Get(int id)
        {
            return _dbSet.Find(id);
        }

        public IEnumerable<TEntity> GetAll()
        {

            return _dbSet.ToList();
        }

        public TEntity Find(Expression<Func<TEntity, bool>> filter)
        {
            return _dbSet.FirstOrDefault(filter);
        }

        public void Update(TEntity data)
        {
            _dbSet.Attach(data);
            _context.Entry(data).State = EntityState.Modified;
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
