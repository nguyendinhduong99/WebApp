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

        Task<bool> CreateProduct(CreateProduct_DTO request);

        Task<bool> UpdateProduct(UpdateProduct_DTO request);

        Task<ApiResult<bool>> CategoryAssign(int Id, CategoryAssignRequest request);

        Task<ProductViewModel> GetById(int Id, string languageId);

        Task<List<ProductViewModel>> GetFeatureProducts(string languageId, int take);

        Task<List<ProductViewModel>> GetLatestProducts(string languageId, int take);
    }
}