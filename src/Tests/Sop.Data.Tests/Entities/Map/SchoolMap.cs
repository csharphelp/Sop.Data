using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace Sop.Data.Tests.Entities.Map
{
    /// <summary>
    /// Represents a vendor mapping configuration
    /// </summary>
    public partial class SchoolMap : BaseMapEntityTypeConfiguration<School>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<School> builder)
        {
            //builder.ToTable(nameof(School));
            builder.ToTable("school_demo");
            builder.HasKey(n => n.Id);
            builder.Property(n => n.Name).HasMaxLength(60).IsRequired();
            base.Configure(builder);
        }
    }
}
