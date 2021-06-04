using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using ViewModels.Catalog.Products;
using ViewModels.Common;

namespace Admin_APP.Services.Product
{
    public class ProductApiClient : BaseApiClient, IProductApiClient
    {
        public ProductApiClient(IHttpClientFactory httpClientFactory,
             IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
            : base(httpClientFactory, httpContextAccessor, configuration)
        {
        }

        #region GET LIST PRODUCT

        public async Task<PagedResult<ProductViewModel>> GetPaging(GetManageProductPagingRequest request)
        {
            //rất chi là quan trong
            var data = await GetAsync<PagedResult<ProductViewModel>>(
               $"/api/products/paging?pageIndex={request.pageIndex}" +
               $"&pageSize={request.pageSize}" +
               $"&keyword={request.Keyword}&languageId={request.LanguageId}");
            return data;
        }

        #endregion GET LIST PRODUCT
    }
}