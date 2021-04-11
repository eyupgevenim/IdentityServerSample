namespace Identity.API.Data.Mapping.Identity
{
    using global::Identity.API.Data.Entities.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class UserRoleMapping : EntityTypeConfiguration<UserRole>
    {
        public override void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable($"{nameof(UserRole)}s");
            //TODO:...
            base.Configure(builder);
        }
    }
}
