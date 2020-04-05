using System.Collections.Generic;

namespace MySDK.MongoDB.Models
{
    public class PagingResult<T>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long TotalCount { get; set; }

        public IList<T> Items { get; set; }
    }
}