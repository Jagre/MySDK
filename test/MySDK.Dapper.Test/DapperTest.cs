using Dapper.Contrib.Extensions;
using MySDK.Configuration;
using MySDK.Dapper.Extensions;
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
            using (var conn = DapperBase.GetMySqlConnection("test"))
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
    }
}
