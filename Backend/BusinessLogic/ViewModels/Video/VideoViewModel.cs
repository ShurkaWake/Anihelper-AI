using BusinessLogic.ViewModels.AppUser;
using BusinessLogic.ViewModels.Category;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.ViewModels.Video
{
    public class VideoViewModel
    {
        public int Id { get; set; }

        public UserViewModel Creator { get; set; }

        public string FileId { get; set; }

        public string Prompt { get; set; }

        public string Tittle { get; set; }

        public int Height { get; set; }

        public int Width { get; set; }

        public int Seconds { get; set; }

        public int Fps { get; set; }

        public string DownloadLink { get; set; }

        public bool IsLiked { get; set; }

        public CategoryViewModel Category { get; set; }

        public int LikesCount { get; set; }
    }
}
