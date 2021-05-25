using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ViewModels.System;

namespace Application.System
{
    public interface IUserService
    {
        Task<string> Authenticate(LoginRequest request); //login

        Task<bool> Register(RegisterRequest request);
    }
}