using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Common
{
    public class PagedResult<T> : PagedResultBase // dùng cho nhiều đối tượng
    {
        public List<T> Items { set; get; }
    }
}