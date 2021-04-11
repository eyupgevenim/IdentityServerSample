namespace Identity.API.Data.Mapping.Identity
{
    using global::Identity.API.Data.Entities.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RoleMapping : EntityTypeConfiguration<Role>
    {
        public override void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable($"{nameof(Role)}s");
            //TODO:...
            base.Configure(builder);
        }
    }
}
