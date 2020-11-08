using Foxy.Core.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NorthwindEFCoreSqlite;
using System;
using System.Linq;

namespace Examples.Linq
{
    public static class LinqExamples
    {
        public static void Run_LeftJoin_Example()
        {
            var options = BuildOptions();
            using var db = new NorthwindContext(options);

            var result = db.Customers
                .LeftJoin(db.Orders, outer => outer.CustomerId, inner => inner.CustomerId, (inner, outer) => new { inner, outer })
                .Where(e => e.outer == null)
                .Select(e => e.inner.CompanyName)
                .ToList();

            Console.WriteLine($"Customers with no order: {string.Join(", ", result)}");
        }

        private static DbContextOptions<NorthwindContext> BuildOptions()
        {
            var optionsBuilder = new DbContextOptionsBuilder<NorthwindContext>();
            optionsBuilder.UseLoggerFactory(LoggerFactory.Create(builder =>
            {
                builder
                    .AddFilter((category, level) =>
                        category == DbLoggerCategory.Database.Command.Name
                        && level == LogLevel.Information)
                    .AddConsole();
            }));
            return optionsBuilder.Options;
        }
    }
}
