using LongViet_ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.System.User
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}