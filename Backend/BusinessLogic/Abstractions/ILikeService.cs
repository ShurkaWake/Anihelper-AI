using BusinessLogic.Validators.Like;
using FluentResults;

namespace BusinessLogic.Abstractions
{
    public interface ILikeService
    {
        Task<Result> LikeAsync(LikeCreationModel model);
        Task<Result> UnlikeAsync(LikeDeletionModel model);
    }
}