using LongViet_ViewModels.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Catalog.Products;

namespace Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId,GetPublicProductPagingRequest request);
        
    }
}
