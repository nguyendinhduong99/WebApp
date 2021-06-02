using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace ViewModels.System.User
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            //thiết lập các quy định cho đăng ký

            RuleFor(d => d.FirstName).NotEmpty().WithMessage("Thiếu thông tin");
            RuleFor(d => d.LastName).NotEmpty().WithMessage("Thiếu thông tin");
            RuleFor(d => d.Dob).NotEmpty().WithMessage("Thiếu thông tin");
            RuleFor(d => d.Dob).GreaterThan(DateTime.Now.AddYears(-100)).WithMessage("Mời bạn xác nhận lại tuổi bạn");
            RuleFor(d => d.Email).NotEmpty().WithMessage("Thiếu thông tin").
                Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$").WithMessage("Sai định dạng Email");
            RuleFor(d => d.PhoneNumber).NotEmpty().WithMessage("Thiếu thông tin");
            RuleFor(d => d.UserName).NotEmpty().WithMessage("Thiếu thông tin")
                .MaximumLength(10).WithMessage("Tài khoản vượt quá 10 ký tự");
            RuleFor(d => d.PassWord).NotEmpty().WithMessage("Thiếu thông tin")
                .MinimumLength(8).WithMessage("Bảo mật yếu");
            RuleFor(d => d.ConfirmPass).Equal(d => d.PassWord).WithMessage("Không khớp");
        }
    }
}