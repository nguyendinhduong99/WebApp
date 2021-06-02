using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Common
{
    public class PagingRequestBase
    {
        public int pageIndex { set; get; }
        public int pageSize { set; get; }
    }
}