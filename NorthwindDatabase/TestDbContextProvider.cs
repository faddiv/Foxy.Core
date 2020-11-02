using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace NorthwindDatabase
{
    public class TestDbContextProvider : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<TestDbContext>();
            options.UseSqlite("Data Source=:memory:");
            return new TestDbContext(options.Options);
        }

        public static TestDbContext CreateDbContext(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<TestDbContext>();
            options.UseSqlite(connection);
            return new TestDbContext(options.Options);
        }
    }
}
