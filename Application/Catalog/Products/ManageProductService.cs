using Application.Common;
using Data.EF;
using Data.Entities;
using LongViet_Data.Entities;
using LongViet_ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Catalog.ProductImages;
using ViewModels.Catalog.Products;

namespace LongViet_Application.Catalog.Products
{
    class ManageProductService : IManageProductService
    {
        //cần khai báo 1 biến nội bộ, chỉ dùng 1 lần    
        private readonly DB_Context _context;
        private readonly IStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";
        //đặt 1 constructor
        public ManageProductService(DB_Context context,IStorageService storageService)
        {
            //cần đầu vào 1 cái DbContext
            //add reference tầng LongViet_Data để lấy LongViet_DB_Context...

            //gán vào
            _context = context;
            _storageService = storageService;
        }

        public async Task AddViewCount(int ProductId)
        {
            var product = await _context.Products.FindAsync(ProductId);
            product.ViewCount = product.ViewCount + 1;
            await _context.SaveChangesAsync();
        }

        public async Task<int> Create_Product(CreateProduct_DTO request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Today,
                
                ProductTranslations = new List<ProductTranslation>()//cha
                {
                    new ProductTranslation()//con
                    {
                        Name = request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoTitle = request.SeoTitle,
                        SeoAlias = request.SeoAlias,
                        LanguageId = request.LanguageId
                    }
                }
            };
            _context.Products.Add(product);
            return await _context.SaveChangesAsync(); //khi save ở db xong thì nó nhả cái thresh phục vụ request khác,chạy backgroud ĐỂ giảm thời gian chờ, 
        }

        public async Task<int> Delete_Product(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null)
            {
                throw new Exception($"Can't not find product :{productId}");
            }
            _context.Products.Remove(product);
            return await _context.SaveChangesAsync();
        }

        public Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            //1. select + join, using LinQ
            var query = from p in _context.Products
                        join pt in _context.Product_TransLations on p.Id equals pt.ProductId
                        join p_i_c in _context.Product_in_Category on p.Id equals p_i_c.ProductId
                        join c in _context.Category on p_i_c.CategoryId equals c.Id
                        select new { p, pt, p_i_c };

            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.p_i_c.CategoryId));
            }

            //3. Paging = phân trang
            //phải có totalRow, using frameworkcore
            int totalRow = await query.CountAsync();
             var data = await query.Skip((request.pageIndex - 1) * request.pageSize).Take(request.pageSize).Select(x => new ProductViewModel()
            {
                //bảng product
                Id = x.p.Id,
                Price = x.p.Price,
                OriginalPrice = x.p.OriginalPrice,
                Stock = x.p.Stock,
                ViewCount = x.p.ViewCount,
                DateCreated = x.p.DateCreated,

                //bảng product translate
                Name = x.pt.Name,
                Description = x.pt.Description,
                Details = x.pt.Details,
                LanguageId = x.pt.LanguageId,
                SeoAlias = x.pt.SeoAlias,
                SeoDescription = x.pt.SeoDescription,
                SeoTitle = x.pt.SeoTitle
            }).ToListAsync();

            //4. select and projection = chọn và tham chiếu
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data //dạng await
            };
            return pagedResult;
        }

        public Task<int> Update_Product(UpdateProduct_DTO request)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdatePrice(int ProductId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStock(int ProductId, int addedQuantity)
        {
            throw new NotImplementedException();
        }

        public Task<int> AddImage(int productId, ProductImageCreateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveImage(int imageId)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdateImage(int imageId, ProductImageUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ProductImageViewModel> GetImageById(int imageId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            throw new NotImplementedException();
        }
    }
}
