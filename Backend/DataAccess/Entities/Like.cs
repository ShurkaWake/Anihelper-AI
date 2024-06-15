using DataAccess.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class Like : IEntity<int>
    {
        public int Id { get; set; }

        public DateTime Time { get; set; }

        public AppUser User { get; set; }

        public Video Video { get; set; }
    }
}
