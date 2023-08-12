using Bulky.Models;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    public interface ICompanyRepository : IRepository<Company>
    {
        void Update(Company obj);
    }
    
    /*public interface IProductRepo : IRepository<Category>
    {
        void Update(Category obj);
    }*/
}
