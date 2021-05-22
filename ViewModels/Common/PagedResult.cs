using System;
using System.Collections.Generic;
using System.Text;

namespace LongViet_ViewModels.Common
{
    public class PagedResult<T> // dùng cho nhiều đối tượng
    {
        public List<T> Items { set; get; }
        public int TotalRecord { set; get; }
    }
}
