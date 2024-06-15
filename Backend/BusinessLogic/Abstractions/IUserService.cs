using BusinessLogic.Filtering;
using BusinessLogic.Validators.AppUser;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.General;
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
    public interface IUserService
    {
        Task<Result> ChangePasswordAsync(string userId, UserChangePasswordModel model);

        Task<Result<string>> DeleteAsync(string userId);

        Task<PaginationViewModel<UserViewModel>> GetAllUsersAsync(AppUserFilter filter);
    }
}
