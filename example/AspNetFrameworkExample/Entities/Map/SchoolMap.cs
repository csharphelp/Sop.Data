using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace AspNetFrameworkExample.Entities.Map
{
    /// <summary>
    /// Represents a vendor mapping configuration
    /// </summary>
    public partial class SchoolMap : BaseMapEntityTypeConfiguration<School>
    {
        #region Methods

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

        #endregion
    }
}
