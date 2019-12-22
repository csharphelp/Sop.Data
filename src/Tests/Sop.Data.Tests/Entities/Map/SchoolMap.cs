using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace Sop.Data.Tests.Entities.Map
{
    /// <summary>
    /// Represents a vendor mapping configuration
    /// </summary>
    public partial class SchoolMap : SopEntityTypeConfiguration<School>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<School> builder)
        {
            //builder.ToTable(nameof(School));
            builder.ToTable("school_demo");
            builder.HasKey(n => n.Id); 
            builder.Property(n => n.Name).HasMaxLength(60).IsRequired(); 
            base.Configure(builder);
        }

        #endregion
    }
}
