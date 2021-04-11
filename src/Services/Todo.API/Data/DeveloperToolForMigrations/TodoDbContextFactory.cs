namespace Todo.API.Data.DeveloperToolForMigrations
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using System.Reflection;

    /// <summary>
    /// https://www.entityframeworktutorial.net/code-first/code-based-migration-in-code-first.aspx
    /// 
    /// for migrations scripts:
    ///_> dotnet ef migrations add Initial
    ///_> dotnet ef database update
    ///_>#dotnet ef database update Initial
    ///
    /// Add-Migration Initial -OutputDir "Data/Migrations"
    /// dotnet ef migrations add Initial -o "Data/Migrations"
    /// Update-Database
    /// 
    /// dotnet ef migrations add Initial --context TodoDbContext -o "Data/Migrations"
    /// Add-Migration Initial -Context TodoDbContext -OutputDir "Data/Migrations"
    /// 
    /// 
    ///_> Add-Migration [-Name] <String> [-Force] [-ProjectName <String>] [-StartUpProjectName <String>][-ConfigurationTypeName<String>][-ConnectionStringName<String>] 
    /// [-IgnoreChanges] [-AppDomainBaseDirectory<String>][<CommonParameters>]
    /// 
    ///_> Update-Database [-SourceMigration <String>] [-TargetMigration <String>] [-Script] [-Force] [-ProjectName<String>] [-StartUpProjectName<String>] 
    /// [-ConfigurationTypeName<String>] [-ConnectionStringName<String>] [-AppDomainBaseDirectory<String>] [<CommonParameters>]
    /// 
    /// </summary>
    public class TodoDbContextFactory : IDesignTimeDbContextFactory<TodoDbContext>
    {
        public TodoDbContext CreateDbContext(string[] args)
        {
            //Database connection string
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=TodoApiDb;Trusted_Connection=True;MultipleActiveResultSets=true";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name; //"Todo.API"

            var builder = new DbContextOptionsBuilder<TodoDbContext>();
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));

            return new TodoDbContext(builder.Options);
        }
    }
}
