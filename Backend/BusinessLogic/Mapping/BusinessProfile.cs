using AutoMapper;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Category;
using BusinessLogic.ViewModels.Video;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Mapping
{
    public class BusinessProfile : Profile
    {
        public BusinessProfile()
        {
            CreateMap<AppUser, UserAuthModel>();
            CreateMap<AppUser, UserViewModel>();
            CreateMap<UserRegisterModel, AppUser>();

            CreateMap<CategoryUpdateModel, Category>();
            CreateMap<CategoryCreateModel, Category>();
            CreateMap<Category, CategoryViewModel>();

            CreateMap<VideoCreateModel, Video>();
            CreateMap<Video, VideoViewModel>().ForMember(
                dest => dest.LikesCount, 
                src => src.MapFrom(src => src.Likes.Count()));
        }
    }
}
