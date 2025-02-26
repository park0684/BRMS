using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using common.DTOs;
using common.Helpers;
using common.Model;
using BRIC.Repositories;

namespace BRIC.Services
{
    public class SupplierService
    {
        ISupplierRepository _repository;

        public SupplierService(ISupplierRepository repository)
        {
            _repository = repository;
        }

        public DataTable LoadSeachList()
        {
            var result = _repository.LoadSupplierSearchList();
            return result;
        }
    }
}
