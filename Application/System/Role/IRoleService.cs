using System.Collections.Generic;
using System.Threading.Tasks;
using ViewModels.System.Role;

namespace Application.System.Role
{
    public interface IRoleService
    {
        Task<List<RoleViewModel>> GetAll();
    }
}