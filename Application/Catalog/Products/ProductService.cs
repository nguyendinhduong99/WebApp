using Application.Common;
using Data.EF;
using Data.Entities;
using LongViet_Data.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Catalog.ProductImages;
using ViewModels.Catalog.Products;
using ViewModels.Common;

namespace Application.Catalog.Products
{
    public class ProductService : IProductService
    {
        //cần khai báo 1 biến nội bộ, chỉ dùng 1 lần
        private readonly DB_Context _context;

        private readonly IFileStorageService _storageService;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";//tên folder chứa ảnh

        //đặt 1 constructor
        public ProductService(DB_Context context, IFileStorageService storageService)
        {
            //cần đầu vào 1 cái DbContext
            //add reference tầng LongViet_Data để lấy LongViet_DB_Context...

            //gán vào
            _context = context;
            _storageService = storageService;
        }

        #region Phân trang

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetManageProductPagingRequest request)
        {
            //1. select + join, using LinQ
            var query = from p in _context.Products
                        join pt in _context.Product_TransLations on p.Id equals pt.ProductId
                        join p_i_c in _context.Product_in_Category on p.Id equals p_i_c.ProductId into ppic
                        from p_i_c in ppic.DefaultIfEmpty()
                        join c in _context.Category on p_i_c.CategoryId equals c.Id into picc
                        from c in picc.DefaultIfEmpty()
                        where pt.LanguageId == request.LanguageId
                        select new { p, pt, p_i_c };

            //2. filter
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));
            }
            if (request.CategoryId != null && request.CategoryId != 0)
            {
                query = query.Where(p => p.p_i_c.CategoryId == request.CategoryId);
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
                TotalRecords = totalRow,
                PageIndex = request.pageIndex,
                PageSize = request.pageSize,
                Items = data //dạng await
            };
            return pagedResult;
        }

        #endregion Phân trang

        #region Product

        public async Task<ProductViewModel> GetById(int productId, string languageId)
        {
            var product = await _context.Products.FindAsync(productId);
            var productTranslation = await _context.Product_TransLations.FirstOrDefaultAsync(x => x.ProductId == productId && x.LanguageId == languageId);
            var categories = await (from c in _context.Category
                                    join ct in _context.Category_Translations on c.Id equals ct.CategoryId
                                    join p_i_c in _context.Product_in_Category on c.Id equals p_i_c.CategoryId
                                    where p_i_c.ProductId == productId && ct.LanguageId == languageId
                                    select ct.Name).ToListAsync();
            var productViewModel = new ProductViewModel()
            {
                //product
                Id = product.Id,
                Price = product.Price,
                OriginalPrice = product.OriginalPrice,
                Stock = product.Stock,
                ViewCount = product.ViewCount,
                DateCreated = product.DateCreated,

                //productTraslation
                Name = productTranslation != null ? productTranslation.Name : null,
                //if (productTranslation !=null)
                //{
                //    Name = productTranslation.Name;
                //}
                //else
                //{
                //    Name = null;
                //}
                Description = productTranslation != null ? productTranslation.Description : null,
                Details = productTranslation != null ? productTranslation.Details : null,
                SeoDescription = productTranslation != null ? productTranslation.SeoDescription : null,
                SeoTitle = productTranslation != null ? productTranslation.SeoTitle : null,
                SeoAlias = productTranslation != null ? productTranslation.SeoAlias : null,
                LanguageId = productTranslation.LanguageId,
                Categories = categories
            };
            return productViewModel;
        }

        public async Task Add_ViewCount(int productId)
        {
            var product = await _context.Products.FindAsync(productId);
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

            //save image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption="Thumbnail Image",
                        DateCreated=DateTime.Now,
                        FileSize=request.ThumbnailImage.Length,
                        ImagePath=await this.SaveFile(request.ThumbnailImage),
                        IsDefault=true,
                        SortOrder=1
                    }
                };
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync(); //khi save ở db xong thì nó nhả cái thresh phục vụ request khác,chạy backgroud ĐỂ giảm thời gian chờ,
            return product.Id;
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

        public async Task<int> Update_Product(UpdateProduct_DTO request)
        {
            var product = await _context.Products.FindAsync(request.Id);
            var productTranslations = await _context.Product_TransLations.FirstOrDefaultAsync(x => x.ProductId == request.Id
            && x.LanguageId == request.LanguageId);

            if (product == null || productTranslations == null) throw new Exception($"Cannot find a product with id: {request.Id}");

            productTranslations.Name = request.Name;
            productTranslations.SeoAlias = request.SeoAlias;
            productTranslations.SeoDescription = request.SeoDescription;
            productTranslations.SeoTitle = request.SeoTitle;
            productTranslations.Description = request.Description;
            productTranslations.Details = request.Details;

            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _context.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await SaveFile(request.ThumbnailImage);
                    _context.ProductImages.Update(thumbnailImage);
                }
            }

            return await _context.SaveChangesAsync();
        }

        #endregion Product

        #region Price

        public async Task<bool> Update_Price(int productId, decimal newPrice)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new Exception($"Cannot find a product with id: {productId}");

            product.Price = newPrice;

            return await _context.SaveChangesAsync() > 0;
        }

        #endregion Price

        #region Stock

        public async Task<bool> Update_Stock(int productId, int addedQuantity)
        {
            var product = await _context.Products.FindAsync(productId);
            if (product == null) throw new Exception($"Cannot find a product with id: {productId}");

            product.Stock += addedQuantity;

            return await _context.SaveChangesAsync() > 0;
        }

        #endregion Stock

        #region Image

        public async Task<ProductImageViewModel> GetImageById(int imageId)
        {
            var image = await _context.ProductImages.FindAsync(imageId);
            if (image == null)
                throw new Exception($"Cannot find an image with id {imageId}");

            var viewModel = new ProductImageViewModel()
            {
                Caption = image.Caption,
                DateCreated = image.DateCreated,
                FileSize = image.FileSize,
                Id = image.Id,
                ImagePath = image.ImagePath,
                IsDefault = image.IsDefault,
                ProductId = image.ProductId,
                SortOrder = image.SortOrder
            };
            return viewModel;
        }

        public async Task<List<ProductImageViewModel>> GetListImages(int productId)
        {
            return await _context.ProductImages.Where(x => x.ProductId == productId)
                .Select(i => new ProductImageViewModel()
                {
                    Caption = i.Caption,
                    DateCreated = i.DateCreated,
                    FileSize = i.FileSize,
                    Id = i.Id,
                    ImagePath = i.ImagePath,
                    IsDefault = i.IsDefault,
                    ProductId = i.ProductId,
                    SortOrder = i.SortOrder
                }).ToListAsync();
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return "/" + USER_CONTENT_FOLDER_NAME + "/" + fileName;
        }

        //.........................................................................

        public async Task<int> Add_Image(int productId, ProductImageCreateRequest request)
        {
            var productImage = new ProductImage()
            {
                Caption = request.Caption,
                DateCreated = DateTime.Now,
                IsDefault = request.IsDefault,
                ProductId = productId,
                SortOrder = request.SortOrder
            };

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Add(productImage);
            await _context.SaveChangesAsync();
            return productImage.Id;//sau khi thêm và lưu trả ra cái id của ảnh để sau sang phần api có cái mà dùng
        }

        public async Task<int> Remove_Image(int imageId)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new Exception($"Cannot find an image with id {imageId}");
            _context.ProductImages.Remove(productImage);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Update_Image(int imageId, ProductImageUpdateRequest request)
        {
            var productImage = await _context.ProductImages.FindAsync(imageId);
            if (productImage == null)
                throw new Exception($"Cannot find an image with id {imageId}");

            if (request.ImageFile != null)
            {
                productImage.ImagePath = await SaveFile(request.ImageFile);
                productImage.FileSize = request.ImageFile.Length;
            }
            _context.ProductImages.Update(productImage);
            return await _context.SaveChangesAsync();
        }

        #endregion Image

        #region Category lay id

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(string languageId, GetPublicProductPagingRequest request)
        {
            //1. select + join, using LinQ
            var query = from p in _context.Products
                        join pt in _context.Product_TransLations on p.Id equals pt.ProductId
                        join p_i_c in _context.Product_in_Category on p.Id equals p_i_c.ProductId
                        join c in _context.Category on p_i_c.CategoryId equals c.Id
                        where pt.LanguageId == languageId
                        select new { p, pt, p_i_c };

            //2. filter

            if (request.CategoryId.HasValue && request.CategoryId.Value > 0)
            {
                query = query.Where(p => p.p_i_c.CategoryId == request.CategoryId);
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
                TotalRecords = totalRow,
                PageIndex = request.pageIndex,
                PageSize = request.pageSize,
                Items = data //dạng await
            };
            return pagedResult;
        }

        #endregion Category lay id

        #region phan quyen san pham

        public async Task<ApiResult<bool>> CategoryAssign(int Id, CategoryAssignRequest request)
        {
            var user = await _context.Products.FindAsync(Id);
            if (user == null)
            {
                return new ApiErrorResult<bool>($"Không tìm thấy sản phẩm với mã = {Id}");
            }
            foreach (var category in request.Categories)
            {
                var product_in_Category = await _context.Product_in_Category
                    .FirstOrDefaultAsync(d => d.CategoryId == int.Parse(category.Id)
                    && d.ProductId == Id);
                if (product_in_Category != null && category.Selected == false)
                {
                    _context.Product_in_Category.Remove(product_in_Category);
                }
                else if (product_in_Category == null && category.Selected)
                {
                    await _context.Product_in_Category.AddAsync(new ProductInCategory()
                    {
                        CategoryId = int.Parse(category.Id),
                        ProductId = Id
                    });
                }
            }
            await _context.SaveChangesAsync();
            return new ApiSuccessResult<bool>();
        }

        #endregion phan quyen san pham
    }
}