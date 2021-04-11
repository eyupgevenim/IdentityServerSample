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
    /// dotnet ef migrations add Initial --context ApplicationConfigurationDbContext -o "Data/Migrations/Configuration"
    /// Add-Migration Initial -Context ApplicationConfigurationDbContext -OutputDir "Data/Migrations/Configuration"
    /// </summary>
    public class ApplicationConfigurationDbContextFactory : IDesignTimeDbContextFactory<ApplicationConfigurationDbContext>
    {
        public ApplicationConfigurationDbContext CreateDbContext(string[] args)
        {
            //Database connection string
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=Configuration_db;Trusted_Connection=True;MultipleActiveResultSets=true";
            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;

            var builder = new DbContextOptionsBuilder<ConfigurationDbContext>();
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssembly));
            ConfigurationStoreOptions storeOptions = new ConfigurationStoreOptions();

            return new ApplicationConfigurationDbContext(builder.Options, storeOptions);
        }
    }
}
