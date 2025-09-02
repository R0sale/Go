using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Infrastructured.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        private readonly IConfiguration _config;

        public RoleConfiguration(IConfiguration config)
        {
            _config = config;
        }

        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData
            (
                new IdentityRole
                {
                    Id = _config["RolesConfig:User"],
                    Name = "User",
                    NormalizedName = "USER"
                },
                new IdentityRole
                {
                    Id = _config["RolesConfig:Admin"],
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Id = _config["RolesConfig:Owner"],
                    Name = "Owner",
                    NormalizedName = "OWNER"
                }
            );
        }
    }
}
