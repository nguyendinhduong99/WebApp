using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.Common
{
    public class PagingRequestBase : RequestBase
    {
        public int pageIndex { set; get; }
        public int pageSize { set; get; }
    }
}