using System;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;
//using Flurl.Http;

namespace MySDK.Url
{
    public class Class1
    {
        public Class1()
        {

        }

        public async Task t()
        {
            var uri = new Uri("");
            //uri.AppendPathSegment()
            var url = string.Empty;
            await url.AppendPathSegment("")
                .SetQueryParam("")
                //.WithOAuthBearerToken("")
                .PostJsonAsync(new { })
                .ReceiveJson<string>();
                ;
        }

        //public string Task<>
    }
}
