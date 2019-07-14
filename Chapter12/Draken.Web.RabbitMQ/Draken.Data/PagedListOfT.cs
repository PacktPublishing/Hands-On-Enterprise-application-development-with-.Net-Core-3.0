using System;
using System.Collections.Generic;
using System.Text;

namespace Draken.Data
{
    public class PagedList<T> where T : class, new()
    {
        public IEnumerable<T> Results { get; set; }
        public Paging Paging { get; set; }
    }

    public class Paging
    {
        public string Previous { get; set; }
        public string Next { get; set; }
    }
}
