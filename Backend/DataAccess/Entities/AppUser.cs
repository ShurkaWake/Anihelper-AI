using DataAccess.Abstractions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Entities
{
    public class AppUser : IdentityUser, IEntity<string>
    {
        public string FullName { get; set; }

        public ICollection<Video> Videos { get; set; }

        public override string ToString()
        {
            return FullName;
        }
    }
}
