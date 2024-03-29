﻿using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository
{
    public class ApplicationUserRepo : Repository<ApplicationUser>, IApplicationUserRepo
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepo(ApplicationDbContext db) : base(db)
        {
            _db=db;
        }
    }
}
