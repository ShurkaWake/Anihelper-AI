using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstractions
{
    public interface IVideoRepository : IRepository<Video, int>
    {
        Task<Video> GetVideoIncludingAll(int videoId);

        Task<IEnumerable<Video>> GetUserVideos(string userId);
    }
}
