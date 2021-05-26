using LongViet_ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModels.System.User;

namespace Application.System
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request); //login

        Task<bool> Register(RegisterRequest request);

        Task<PagedResult<UserViewModel>> GetUsersPaging(GetUserPagingRequest request);//lấy danh sách user, trả ra 1 model phân trang PagedResult
    }
}