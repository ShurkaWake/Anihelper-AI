using BusinessLogic.Abstractions;
using BusinessLogic.Validators.Like;
using DataAccess.Abstractions;
using DataAccess.Entities;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class LikeService : ILikeService
    {
        private readonly IVideoRepository _videoRepository;
        private readonly IUserRepository _userRepository;

        public LikeService(IVideoRepository videoRepository, IUserRepository userRepository)
        {
            _videoRepository = videoRepository;
            _userRepository = userRepository;
        }

        public async Task<Result> LikeAsync(LikeCreationModel model)
        {
            var video = await _videoRepository.GetVideoIncludingAll(model.videoId);
            if (video is null)
            {
                return Result.Fail("Unaible to find video");
            }

            var user = await _userRepository.GetAsync(model.userId);
            if (user is null)
            {
                return Result.Fail("Unaible to find user");
            }

            var duplicateLike = video.Likes.FirstOrDefault(x => x.User.Id == model.userId);
            if (duplicateLike is not null)
            {
                return Result.Fail("This video already was liked");
            }

            var like = new Like()
            {
                Time = DateTime.Now,
                User = user,
                Video = video,
            };
            video.Likes.Add(like);

            var updateResult = await _videoRepository.ConfirmAsync();
            return updateResult > 0
                ? Result.Ok()
                : Result.Fail("Unaible to save like");
        }

        public async Task<Result> UnlikeAsync(LikeDeletionModel model)
        {
            var video = await _videoRepository.GetVideoIncludingAll(model.videoId);
            if (video is null)
            {
                return Result.Fail("Unaible to find video");
            }

            var user = await _userRepository.GetAsync(model.userId);
            if (user is null)
            {
                return Result.Fail("Unaible to find user");
            }

            var duplicateLike = video.Likes.FirstOrDefault(x => x.User.Id == model.userId);
            if (duplicateLike is null)
            {
                return Result.Ok();
            }

            video.Likes.Remove(duplicateLike);
            var updateResult = await _videoRepository.ConfirmAsync();
            return updateResult > 0
                ? Result.Ok()
                : Result.Fail("Unaible to save like");
        }
    }
}
