using System;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.User;

namespace Application.System.User
{
    public interface IUserService
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request); //login

        Task<ApiResult<bool>> Register(RegisterRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUsersPaging(GetUserPagingRequest request);//lấy danh sách user, trả ra 1 model phân trang PagedResult

        Task<ApiResult<bool>> Update(Guid id, UserUpdateRequest request);

        Task<ApiResult<UserViewModel>> GetById(Guid id);

        Task<ApiResult<bool>> Delete(Guid id);

        //cho tài khoản
        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}