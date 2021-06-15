using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ViewModels.System.Role;

namespace Application.System.Role
{
    public class RoleService : IRoleService
    {
        private readonly RoleManager<AppRole> _roleManager;

        public RoleService(RoleManager<AppRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<List<RoleViewModel>> GetAll()
        {
            var roles = await _roleManager.Roles.Select(d => new RoleViewModel()
            {
                Id = d.Id,
                Name = d.Name,
                Description = d.Description
            }).ToListAsync();
            return roles;
        }
    }
}