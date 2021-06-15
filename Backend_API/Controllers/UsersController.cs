using Application.System.Role;
using Application.System.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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

        #region sign in

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.Authenticate(request);
            if (string.IsNullOrEmpty(result.ResultObj))
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        #endregion sign in

        #region sign up

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.Register(request);
            if (!result.IsSuccessed) return BadRequest(result);
            return Ok(result);
        }

        #endregion sign up

        #region phân trang

        //http://localhost/api/Users/paging?pageIndex=1&pageSize=10&Keyword=
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request)
        {
            var user = await _userService.GetUsersPaging(request);
            return Ok(user);
        }

        #endregion phân trang

        #region Lấy ID

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var user = await _userService.GetById(id);
            return Ok(user);
        }

        #endregion Lấy ID

        #region Update

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserUpdateRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.Update(id, request);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }

        #endregion Update

        #region Delete

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _userService.Delete(id);
            return Ok(result);
        }

        #endregion Delete

        #region Phân quyền

        [HttpPut("{id}/roles")]
        public async Task<IActionResult> RoleAssign(Guid id, [FromBody] RoleAssignRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _userService.RoleAssign(id, request);
            if (!result.IsSuccessed)
                return BadRequest(result);
            return Ok(result);
        }

        #endregion Phân quyền
    }
}