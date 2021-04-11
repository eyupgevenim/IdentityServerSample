namespace Identity.API.Data.DeveloperToolForMigrations
{
    using IdentityServer4.EntityFramework.DbContexts;
    using IdentityServer4.EntityFramework.Options;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using System.Reflection;

    /// <summary>
    /// for migrations scripts:
    ///_> dotnet ef migrations add Initial
    ///_> dotnet ef database update
    ///_>#dotnet ef database update Initial
    ///
    /// Add-Migration Initial -OutputDir "Data/Migrations"
    /// dotnet ef migrations add Initial -o "Data/Migrations"
    /// 
    /// dotnet ef migrations add Initial --context ApplicationPersistedGrantDbContext -o "Data/Migrations/PersistedGrant"
    /// Add-Migration Initial -Context ApplicationPersistedGrantDbContext -OutputDir "Data/Migrations/PersistedGrant"
    /// </summary>
    public class ApplicationPersistedGrantDbContextFactory : IDesignTimeDbContextFactory<ApplicationPersistedGrantDbContext>
    {
        public ApplicationPersistedGrantDbContext CreateDbContext(string[] args)
        {
            //Database connection string
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=PersistedGrant_db;Trusted_Connection=True;MultipleActiveResultSets=true";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name; //"Identity.API"

            var builder = new DbContextOptionsBuilder<PersistedGrantDbContext>();
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));
            OperationalStoreOptions storeOptions = new OperationalStoreOptions();

            return new ApplicationPersistedGrantDbContext(builder.Options, storeOptions);
        }
    }
}
