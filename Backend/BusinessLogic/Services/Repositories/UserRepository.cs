using BusinessLogic.Abstractions;
using DataAccess;
using DataAccess.Abstractions;
using DataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.Repositories
{
    public sealed class UserRepository : Repository<AppUser, string>, IUserRepository
    {
        public UserRepository(ApplicationContext context) : base(context) 
        {
        }
    }
}
