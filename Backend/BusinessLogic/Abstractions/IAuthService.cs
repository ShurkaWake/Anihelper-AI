using BusinessLogic.Core;
using BusinessLogic.Validators.AppUser;
using BusinessLogic.ViewModels.AppUser;
using DataAccess.Entities;
using FluentResults;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Abstractions
{
    public interface IAuthService
    {
        Task<Result<UserAuthModel>> LoginAsync(UserLoginModel model);

        Task<Result<UserViewModel>> RegisterAsync(UserRegisterModel model);
    }
}
