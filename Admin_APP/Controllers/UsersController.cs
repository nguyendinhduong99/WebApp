using Admin_APP.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ViewModels.System.User;

namespace Admin_APP.Controllers
{
    public class UsersController : BaseController
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IConfiguration _configuration;

        public UsersController(IUserApiClient userApiClient, IConfiguration configuration)
        {
            _userApiClient = userApiClient;
            _configuration = configuration;
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
                return RedirectToAction("Index"); //chuyển đến cái thằng có tên Index
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
            return RedirectToAction("Login", "Users");
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
                return RedirectToAction("Index"); //chuyển đến cái thằng có tên Index
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
                return RedirectToAction("Index"); //chuyển đến cái thằng có tên Index
            ModelState.AddModelError("", result.Message);
            return View(request);
        }

        #endregion Xóa
    }
}