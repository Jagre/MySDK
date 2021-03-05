using Flurl.Http;
using System.Threading.Tasks;
using Xunit;

namespace MySDK.URL.Test
{
    public class UrlTest
    {
        [Fact]
        public async Task Test_GetRequestUrl_Ok()
        {
            //request.GetRequestUrl();
            var content = await "https://www.baidu.com".GetStringAsync();
            Assert.NotNull(content);
        }
    }
}
