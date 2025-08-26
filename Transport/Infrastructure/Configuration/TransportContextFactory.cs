using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configuration
{
    public class TransportContextFactory : IDesignTimeDbContextFactory<TransportContext>
    {
        public TransportContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("LocalConnection");

            var optionsBuilder = new DbContextOptionsBuilder<TransportContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new TransportContext(optionsBuilder.Options);
        }
    }
}
