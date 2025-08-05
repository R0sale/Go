using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructured
{
    public class UsersContext : IdentityDbContext
    {
        public UsersContext(DbContextOptions opts) : base(opts)
        {
        }
    }
}
