using ERPortal.Core.Contracts;
using ERPortal.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace ERPortal.DataAccess.InMemory
{
    public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity
    {
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string _className;

        public InMemoryRepository()
        {
            _className = typeof(T).Name;
            items = cache[_className] as List<T>;
            if (null == items)
            {
                items = new List<T>();
            }
        }

        public void Commit()
        {
            cache[_className] = items;
        }

        public void Delete(string Id)
        {
            T tToDelete = items.Find(t => t.Id == Id);
            if(null == tToDelete)
            {
                throw new Exception(_className + " Not Found");
            }
            else
            {
                items.Remove(tToDelete);
            }
        }

        public T Find(string Id)
        {
            T tToFind = items.Find(t => t.Id == Id);
            if(null == tToFind)
            {
                throw new Exception(_className + " Not Found");
            }
            else
            {
                return tToFind;
            }
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(i => i.Id == t.Id);
            if (null == tToUpdate)
            {
                throw new Exception(_className + " Not Found");
            }
            else
            {
                tToUpdate = t;
            }
        }

        public IQueryable<T> Collection()
        {
            return items.AsQueryable<T>();
        } 

        public void Insert(T t)
        {
            items.Add(t);
        }
    }
}
