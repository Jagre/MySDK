using System;
using System.Collections.Generic;
using System.Text;
using Test.Entities;

namespace MySDK.Dapper.Test
{
    public class ProductRepository : MySqlDapperRepository<Product, int>, IDapperRepository<Product, int>
    {
        public ProductRepository() 
            : base("test")
        {
        }
    }
}
