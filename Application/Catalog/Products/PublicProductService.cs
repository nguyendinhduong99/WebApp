using LongViet_ViewModels.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Catalog.Products;
using System.Linq;
using Data.EF;

namespace Application.Catalog.Products
{
    public class PublicProductService : IPublicProductService
    {
        private readonly DB_Context _context;
        public PublicProductService(DB_Context context)
        {
            _context = context;
        }
        public async Task<List<ProductViewModel>> GetAll()
        {
            //1. select + join, using LinQ
            var query = from p in _context.Products
                        join pt in _context.Product_TransLations on p.Id equals pt.ProductId
                        join p_i_c in _context.Product_in_Category on p.Id equals p_i_c.ProductId
                        join c in _context.Category on p_i_c.CategoryId equals c.Id
                        select new { p, pt, p_i_c };

           
            var data = await query.Select(x => new ProductViewModel()
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

            return data;
        }

        public async Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request)
        {
            //1. select + join, using LinQ
            var query = from p in _context.Products
                        join pt in _context.Product_TransLations on p.Id equals pt.ProductId
                        join p_i_c in _context.Product_in_Category on p.Id equals p_i_c.ProductId
                        join c in _context.Category on p_i_c.CategoryId equals c.Id
                        select new { p, pt, p_i_c };

            //2. filter
          
            if (request.CategoryId.HasValue && request.CategoryId.Value>0)
            {
                query = query.Where(p =>p.p_i_c.CategoryId== request.CategoryId);
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
    }
}
