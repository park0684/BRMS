using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using BRIC.Repositories;

namespace BRIC.Services
{
    public class CategoryService
    {
        IProductRepository _repository;

        public CategoryService(IProductRepository repository)
        {
            _repository = repository;
        }

        public DataTable LoadCategory(int top, int mid, bool invalidity)
        {
            var result = _repository.LoadCategory(top, mid, invalidity);
            return result;

        }
    }
}
