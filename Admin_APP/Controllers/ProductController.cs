using Admin_APP.Services.Product;
using Admin_APP.Services.Role;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Utilties.Constant;
using ViewModels.Catalog.Products;

namespace Admin_APP.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductApiClient _productApiClient;
        private readonly IConfiguration _configuration;

        public ProductController(IProductApiClient productApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _productApiClient = productApiClient;
            _configuration = configuration;
        }

        #region Thông tin

        public async Task<IActionResult> Index(string Keyword, int pageIndex = 1, int pageSize = 3)
        {
            var languageId = HttpContext.Session.GetString(SystemConstants.AppSettings.DefaultLanguageId);
            var request = new GetManageProductPagingRequest()
            {
                Keyword = Keyword,
                pageIndex = pageIndex,
                pageSize = pageSize,
                LanguageId = languageId
            };
            var data = await _productApiClient.GetPaging(request);
            ViewBag.Keyword = Keyword;
            if (TempData["thongbao"] != null)
            {
                ViewBag.SuccessMsg = TempData["thongbao"];
            }
            return View(data);
        }

        #endregion Thông tin
    }
}