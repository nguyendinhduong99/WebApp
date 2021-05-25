using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.System
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        //truyền 1 constructor
        public LoginRequestValidator()
        {
            //thiết lập 1 quy đinh= rule
            RuleFor(d => d.UserName).NotEmpty().WithMessage("Tài khoản bắt buộc phải nhập");
            RuleFor(d => d.Password).NotEmpty().WithMessage("Mật khẩu bắt buộc phải nhập")
                .MinimumLength(6).WithMessage("Mật khẩu phải lớn hơn 6 ký tự");
        }
    }
}