﻿using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using BulkyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public ICategoryRepository Category { get; private set; } // private set because we don't want to set it from outside

        public UnitOfWork(ApplicationDbContext db) // constructor
        {
            _db = db;
            Category = new CategoryRepository(_db);
        }

        public void Save()
        {
            _db.SaveChanges();
        }   

       /* public void Dispose()
        {
            _db.Dispose();
        }*/

    }
}
