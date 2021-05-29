using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.User;

namespace Admin_APP.Services
{
    public interface IUserApiClient
    {
        Task<ApiResult<string>> Authenticate(LoginRequest request);

        Task<ApiResult<PagedResult<UserViewModel>>> GetUserPaging(GetUserPagingRequest request);

        Task<ApiResult<bool>> RegisterUser(RegisterRequest request);

        Task<ApiResult<bool>> UpdateUser(Guid id, UserUpdateRequest request);

        Task<ApiResult<UserViewModel>> GetById(Guid id);
    }
}