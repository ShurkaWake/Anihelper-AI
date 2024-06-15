using BusinessLogic.Abstractions;
using DataAccess;
using DataAccess.Abstractions;
using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Repositories
{
    public class VideoRepository : Repository<Video, int>, IVideoRepository
    {
        public VideoRepository(ApplicationContext context) : base(context) { }

        private DbSet<Video> Videos => Context.Videos;

        public async Task<Video> GetVideoIncludingAll(int videoId)
        {
            return Videos
                .Include(x => x.Creator)
                .Include(x => x.Category)
                .Include(x => x.Likes)
                .ThenInclude(x => x.User)
                .FirstOrDefault(x => x.Id == videoId);
        }

        public async Task<IEnumerable<Video>> GetUserVideos(string userId)
        {
            return Videos
                .Include(x => x.Creator)
                .Include(x => x.Category)
                .Where(x => x.Creator.Id == userId);
        }
    }
}
