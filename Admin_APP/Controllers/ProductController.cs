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
using ViewModels.Common;

namespace Admin_APP.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;
        private readonly ICategoriesApiClient _categoryApiClient;

        public ProductController(IProductApiClient productApiClient,
            IConfiguration configuration,
            ICategoriesApiClient categoryApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
            _categoryApiClient = categoryApiClient;
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

            var categories = await _categoryApiClient.GetAll(languageId);
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

        #region thêm

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
            if (result)//nỏ biết
            {
                TempData["thongbao"] = "Thêm Product OK";
                return RedirectToAction("Index"); //chuyển đến cái thằng có tên Index
            }

            ModelState.AddModelError("", "Thêm thất bại");
            return View(request);
        }

        #endregion thêm

        #region Phân quyền sản phẩm

        [HttpGet]
        public async Task<IActionResult> CategoryAssign(int Id)
        {
            var roleAssignRequest = await GetCategoryAssignRequest(Id);
            return View(roleAssignRequest);
        }

        private async Task<CategoryAssignRequest> GetCategoryAssignRequest(int Id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var productObj = await _productApiClient.GetById(Id, languageId);//lay danh sach san pham
            var categories = await _categoryApiClient.GetAll(languageId); //lay danh sach the loai
            var categoryAssignRequest = new CategoryAssignRequest();
            foreach (var role in categories)
            {
                categoryAssignRequest.Categories.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = productObj.Categories.Contains(role.Name)
                });
            }
            return categoryAssignRequest;
        }

        [HttpPost]
        public async Task<IActionResult> CategoryAssign(CategoryAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _productApiClient.CategoryAssign(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["thongbao"] = "Cập nhật quyền thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetCategoryAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        #endregion Phân quyền sản phẩm

        #region update

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);

            var product = await _productApiClient.GetById(id, languageId);
            var editVm = new UpdateProduct_DTO()
            {
                Id = product.Id,
                Description = product.Description,
                Details = product.Details,
                Name = product.Name,
                SeoAlias = product.SeoAlias,
                SeoDescription = product.SeoDescription,
                SeoTitle = product.SeoTitle
            };
            return View(editVm);
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Edit([FromForm] UpdateProduct_DTO request)
        {
            if (!ModelState.IsValid)
                return View(request);

            var result = await _productApiClient.UpdateProduct(request);
            if (result)
            {
                TempData["thongbao"] = "Cập nhật sản phẩm thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Cập nhật sản phẩm thất bại");
            return View(request);
        }

        #endregion update
    }
}