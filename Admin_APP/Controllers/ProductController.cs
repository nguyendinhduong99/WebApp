using Admin_APP.Services.Categories;
using Admin_APP.Services.Product;
using Admin_APP.Services.Role;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Utilties.Constant;
using ViewModels.Catalog.Products;

namespace Admin_APP.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoriesApiClient _categoriesApiClient;

        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoriesApiClient categoriesApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoriesApiClient = categoriesApiClient;
        }

        #region Thông tin

        public async Task<IActionResult> Index(string Keyword, int? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var request = new GetManageProductPagingRequest()
            {
                Keyword = Keyword,
                pageIndex = pageIndex,
                pageSize = pageSize,
                LanguageId = languageId,
                CategoryId = categoryId
            };
            var data = await _productApiClient.GetPaging(request);
            ViewBag.Keyword = Keyword;

            var categories = await _categoriesApiClient.GetAll(languageId);
            ViewBag.Categories = categories.Select(x => new SelectListItem()
            {
                Text = x.Name,
                Value = x.Id.ToString(),
                Selected = categoryId.HasValue && categoryId.Value == x.Id
            });

            if (TempData["thongbao"] != null)
            {
                ViewBag.SuccessMsg = TempData["thongbao"];
            }
            return View(data);
        }

        #endregion Thông tin

        #region them

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm] CreateProduct_DTO request)
        {
            if (!ModelState.IsValid)
                return View(request);
            var result = await _productApiClient.CreateProduct(request);
            if (result == false)//nỏ biết
            {
                TempData["thongbao"] = "Thêm Product OK";
                return RedirectToAction("Index"); //chuyển đến cái thằng có tên Index
            }

            ModelState.AddModelError("", "Thêm thất bại");
            return View(request);
        }

        #endregion them
    }
}