using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {

        internal DataContext _context;
        internal DbSet<T> _dbSet;

        public SQLRepository(DataContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<T>();
        }

        public IQueryable<T> Collection()
        {
            return _dbSet;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }

        public void Delete(string Id)
        {
            var t = Find(Id);
            if (_context.Entry(t).State == EntityState.Detached)
                _dbSet.Attach(t);

            _dbSet.Remove(t);
        }

        public T Find(string Id)
        {
            return _dbSet.Find(Id);
        }

        public void Insert(T t)
        {
            _dbSet.Add(t);
        }

        public void Update(T t)
        {
            _dbSet.Attach(t);
            _context.Entry(t).State = EntityState.Modified;
        }
    }
}
