using Dapper.Contrib.Extensions;
using MySDK.Configuration;
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
            _connectionString = ConfigurationRoot.GetConnectionConfig("test");
        }

        [Fact]
        public async Task Test_GetProductAsync_Ok()
        {
            using (var conn = DapperExecuter.GetMySqlConnection("test"))
            {
                var prod = await conn.GetAsync<Product>(1);
                Assert.Equal(1, prod.Id);
            }
        }
    }
}
