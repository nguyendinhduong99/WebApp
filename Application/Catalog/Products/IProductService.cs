using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Catalog.ProductImages;
using ViewModels.Catalog.Products;
using ViewModels.Common;

namespace Application.Catalog.Products
{
    public interface IProductService
    {
        Task<int> Create_Product(CreateProduct_DTO request);

        Task<int> Update_Product(UpdateProduct_DTO request);

        Task<int> Delete_Product(int productId);

        Task<ProductViewModel> GetById(int productId, string languageId);

        Task<bool> Update_Price(int productId, decimal newPrice);

        Task<bool> Update_Stock(int productId, int addedQuantity);

        Task Add_ViewCount(int productId);

        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request); //truyền vô 1 request, lấy có chọn lọc, trong ProductViewModel chỉ cung cấp thông số

        Task<int> Add_Image(int productId, ProductImageCreateRequest request);

        Task<int> Remove_Image(int imageId);

        Task<int> Update_Image(int imageId, ProductImageUpdateRequest request);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImages(int productId);

        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request);
    }
}