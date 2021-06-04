using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Catalog.Products;
using ViewModels.Common;

namespace Admin_APP.Services.Product
{
    public interface IProductApiClient
    {
        Task<PagedResult<ProductViewModel>> GetPaging(GetManageProductPagingRequest request);
    }
}