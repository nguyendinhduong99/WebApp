using System;
using System.Collections.Generic;
using System.Text;
using ViewModels.Common;

namespace ViewModels.System.User
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}