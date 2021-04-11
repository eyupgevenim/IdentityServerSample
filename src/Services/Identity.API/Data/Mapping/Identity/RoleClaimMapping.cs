﻿namespace Identity.API.Data.Mapping.Identity
{
    using global::Identity.API.Data.Entities.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class RoleClaimMapping : EntityTypeConfiguration<RoleClaim>
    {
        public override void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable($"{nameof(RoleClaim)}s");
            //TODO:...
            base.Configure(builder);
        }
    }
}
