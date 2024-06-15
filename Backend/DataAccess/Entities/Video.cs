using DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Video : IEntity<int>
    {
        public int Id { get; set; }

        public AppUser Creator { get; set; }

        public string FetchUrl { get; set; }

        public string? FileId { get; set; }

        public string Prompt { get; set; }

        public string Tittle { get; set; }  

        public int Height { get; set; }

        public int Width { get; set; }

        public int Seconds { get; set; }

        public int Fps { get; set; }

        public Category Category { get; set; }

        public ICollection<Like> Likes { get; set; }
    }
}
