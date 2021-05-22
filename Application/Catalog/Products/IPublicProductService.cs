﻿using LongViet_ViewModels.Common;
using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.Catalog.Products;

namespace LongViet_Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(GetPublicProductPagingRequest request);
        Task<List<ProductViewModel>> GetAll();
    }
}
