using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModels.Common;
using ViewModels.System.Role;

namespace Application.System.Role
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAll();

        Task<ApiResult<bool>> RoleAssign(Guid id, RoleAssignRequest request);
    }
}