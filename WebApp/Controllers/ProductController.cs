using Admin_APP.Services.Categories;
using Admin_APP.Services.Product;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly ICategoriesApiClient _categoriesApiClient;

        public ProductController(IProductApiClient productApiClient,
            ICategoriesApiClient categoriesApiClient)
        {
            _productApiClient = productApiClient;
            _categoriesApiClient = categoriesApiClient;
        }

        public IActionResult Detail(int id)
        {
            return View();
        }

        public async Task<IActionResult> Category(string culture, int id, int page = 1)
        {
            var products = await _productApiClient.GetPaging(new ViewModels.Catalog.Products.GetManageProductPagingRequest()
            {
                CategoryId = id,
                pageIndex = page,
                LanguageId = culture,
                pageSize = 10
            });
            return View(new ProductCategoryViewModel()
            {
                Category = await _categoriesApiClient.GetById(culture, id),
                Products = products
            });
        }
    }
}