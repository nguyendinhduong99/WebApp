using Admin_APP.Services.Role;
using Admin_APP.Services.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.Role;
using ViewModels.System.User;

namespace Admin_APP.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;
        private readonly IRoleApiClient _roleApiClient;

        public UsersController(IUserApiClient userApiClient, IConfiguration configuration, IRoleApiClient roleApiClient)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
            _roleApiClient = roleApiClient;
        }

        #region Thông tin

        public async Task<IActionResult> Index(string Keyword, int pageIndex = 1, int pageSize = 3)
        {
            var request = new GetUserPagingRequest()
            {
                keyWord = Keyword,
                pageIndex = pageIndex,
                pageSize = pageSize
            };
            var data = await _userApiClient.GetUserPaging(request);
            ViewBag.Keyword = Keyword;
            if (TempData["thongbao"] != null)
            {
                ViewBag.SuccessMsg = TempData["thongbao"];
            }
            return View(data.ResultObj);
        }

        #endregion Thông tin

        #region Đăng Ký

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.RegisterUser(request);
            if (result.IsSuccessed)
            {
                TempData["thongbao"] = "Thêm OK";
                return RedirectToAction("Index"); //chuyển đến cái thằng có tên Index
            }

            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        #endregion Đăng Ký

        #region Đăng Xuất

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index", "Login");
        }

        #endregion Đăng Xuất

        #region Cập nhật TK

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var result = await _userApiClient.GetById(id);
            if (result.IsSuccessed)
            {
                var user = result.ResultObj;
                var updateRequest = new UserUpdateRequest()
                {
                    Dob = user.Dob,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    PhoneNumber = user.PhoneNumber,
                    Id = id
                };
                return View(updateRequest);
            }
            return RedirectToAction("Error", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.UpdateUser(request.Id, request);
            if (result.IsSuccessed)
            {
                TempData["thongbao"] = "Cập Nhật OK";
                return RedirectToAction("Index"); //chuyển đến cái thằng có tên Index
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        #endregion Cập nhật TK

        #region Chi Tiết

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var user = await _userApiClient.GetById(id);
            return View(user.ResultObj);
        }

        #endregion Chi Tiết

        #region Xóa

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            return View(new UserDeleteRequest()
            {
                Id = id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(UserDeleteRequest request)
        {
            if (!ModelState.IsValid)
                return View();
            var result = await _userApiClient.Delete(request.Id);
            if (result.IsSuccessed)
            {
                TempData["thongbao"] = "Xóa OK";
                return RedirectToAction("Index"); //chuyển đến cái thằng có tên Index
            }
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        #endregion Xóa

        #region Phân Quyền

        [HttpGet]
        public async Task<IActionResult> RoleAssign(Guid id)
        {
            var roleAssignRequest = await GetRoleAssignRequest(id);
            return View(roleAssignRequest);
        }

        private async Task<RoleAssignRequest> GetRoleAssignRequest(Guid id)
        {
            var userObj = await _userApiClient.GetById(id);
            var roleObj = await _roleApiClient.GetAll();
            var roleAssignRequest = new RoleAssignRequest();
            foreach (var role in roleObj.ResultObj)
            {
                roleAssignRequest.Roles.Add(new SelectItem()
                {
                    Id = role.Id.ToString(),
                    Name = role.Name,
                    Selected = userObj.ResultObj.Roles.Contains(role.Name)
                });
            }
            return roleAssignRequest;
        }

        [HttpPost]
        public async Task<IActionResult> RoleAssign(RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = await _roleApiClient.RoleAssign(request.Id, request);

            if (result.IsSuccessed)
            {
                TempData["thongbao"] = "Cập nhật quyền thành công";
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", result.Message);
            var roleAssignRequest = await GetRoleAssignRequest(request.Id);

            return View(roleAssignRequest);
        }

        #endregion Phân Quyền
    }
}