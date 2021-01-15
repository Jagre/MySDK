using System.Collections.Generic;

namespace MySDK.MongoDB.Models
{
    public class PagingResult<T>
    {
        public PagingResult()
        {
            PageIndex = 1;
            PageSize = 25;
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public long TotalCount { get; set; }

        public IList<T> Items { get; set; }
    }
}