using LongViet_ViewModels.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Catalog.ProductImages;
using ViewModels.Catalog.Products;

namespace Application.Catalog.Products
{
    public interface IManageProductService
    {
        Task<int> Create_Product(CreateProduct_DTO request);
        Task<int> Update_Product(UpdateProduct_DTO request);
        Task<int> Delete_Product(int productId);
        Task<ProductViewModel> GetById(int productId);

        Task<bool> UpdatePrice(int ProductId, decimal newPrice);

        Task<bool> UpdateStock(int ProductId, int addedQuantity);
        Task AddViewCount(int productId);

        Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request); //truyền vô 1 request, lấy có chọn lọc, trong ProductViewModel chỉ cung cấp thông số

        Task<int> AddImage(int productId, ProductImageCreateRequest request);

        Task<int> RemoveImage(int imageId);

        Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request);

        Task<ProductImageViewModel> GetImageById(int imageId);

        Task<List<ProductImageViewModel>> GetListImages(int productId);

    }
}
