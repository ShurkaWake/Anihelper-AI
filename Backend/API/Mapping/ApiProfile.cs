using API.Requests.Auth;
using API.Requests.Category;
using API.Requests.Video;
using API.Requests.VideoProcessing;
using AutoMapper;
using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Category;
using BusinessLogic.ViewModels.Video;
using BusinessLogic.ViewModels.VideoProcessing;

namespace API.Mapping
{
    public class ApiProfile : Profile
    {
        public ApiProfile() 
        {
            CreateMap<LoginRequest, UserLoginModel>();
            CreateMap<RegisterRequest, UserRegisterModel>();

            CreateMap<CategoryUpdateRequest, CategoryUpdateModel>();

            CreateMap<VideoCreateRequest, VideoCreateModel>();
            CreateMap<VideoUpdateRequest, VideoUpdateModel>();

            CreateMap<VideoProcessingRequest, VideoProcessingModel>();
        }
    }
}
