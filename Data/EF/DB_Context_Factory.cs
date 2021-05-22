using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Data.EF
{
    class DB_Context_Factory : IDesignTimeDbContextFactory<DB_Context>
    {
        public DB_Context CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var connectionString = configuration.GetConnectionString("Web_App");

            var optionsBuilder = new DbContextOptionsBuilder<DB_Context>();
            optionsBuilder.UseSqlServer(connectionString);

            return new DB_Context(optionsBuilder.Options);
        }
    }
}
