using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
            //_db.Categories == dbSet , je pan debset pass kelya jail tevha pan _db.Categories use karu shakto
            // this is the table in the database that we want to work with 
            // Set <T> is a method that returns the table from the database
            // DbSet is a class that represents the table in the database
            _db.Products.Include(u => u.Category).Include(u => u.CategoryId);
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProp = null)
        {
            IQueryable<T> query = dbSet; // IQueryable is an interface that allows us to write queries against the database
            query = query.Where(filter); // filter is a lambda expression that returns a boolean value
            if (!String.IsNullOrEmpty(includeProp))
            {
                foreach(var item in includeProp.Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }

            /*IEnumerable<T> any = _db.Categories.FirstOrDefault(u => u.Id = object.id);
            return any;*/
            // this is for when object is taken

            return query.FirstOrDefault();
            // above process is similar to _db.Categories.FirstOrDefault(c => c.Id == id);
        }

        public IEnumerable<T> GetAll(string? includeProp = null)
        {
            IQueryable<T> query = dbSet; // IQueryable is an interface that allows us to write queries against the database , IQueryable is a generic interface that takes a type parameter 
            if (!String.IsNullOrEmpty(includeProp))
            {
                foreach(var item in includeProp.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return query.ToList();
            /*return dbSet.ToList();*/
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
