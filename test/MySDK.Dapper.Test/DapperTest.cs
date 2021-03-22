using Dapper.Contrib.Extensions;
using MySDK.Configuration;
using MySDK.Dapper.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Entities;
using Xunit;

namespace MySDK.Dapper.Test
{
    public class DapperTest: TestBase
    {
        private readonly string _connectionString;

        public DapperTest()
        {
            _connectionString = ConfigurationRoot.GetConnectionString("test");
        }

        [Fact]
        public async Task Test_GetProductAsync_Ok()
        {
            using (var conn = DapperContext.GetMySqlConnection("test"))
            {
                var prod = await conn.GetAsync<Product>(1);
                Assert.Equal(1, prod.Id);
            }
        }

        [Fact]
        public async Task Test_PagingAsync_Ok()
        {
            var prods = await "Select * From Product"
                .PagingAsync<Product>("test", "Id", 1, 2);
            Assert.Equal(2, prods.Items.Count);
        }

        [Fact]
        public async Task Test_GetRelationalTablesAsync_Ok()
        {
            using (var repo = new ProductRepository())
            {
                var prods = await repo.GetRelationalTablesAsync<Product, int, Product>(new List<int> { 1, 2, 3 });
                Assert.True(prods.Count > 0);
            }
        }
    }
}
