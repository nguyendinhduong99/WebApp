using Application.System.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.System.User;

namespace Backend_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        //sign in
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var resultToken = await _userService.Authenticate(request);
            if (string.IsNullOrEmpty(resultToken))
            {
                return BadRequest("Sai tài khoản mật khẩu");
            }
            return Ok(resultToken);
        }

        //sign up
        [HttpPost("SignUp")]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.Register(request);
            if (result == false) return BadRequest("Đăng ký thất bại");
            return Ok();
        }

        //phân trang
        //http://localhost/api/users/paging?pageIndex=1&pageSize=10&Keyword=
        [HttpGet("{paging}")]
        public async Task<IActionResult> PagingUser([FromQuery] GetUserPagingRequest request)
        {
            var user = await _userService.GetUsersPaging(request);
            return Ok(user);
        }
    }
}