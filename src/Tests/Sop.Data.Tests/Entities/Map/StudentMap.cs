using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace Sop.Data.Tests.Entities.Map
{
   
    public partial class StudentMap : SopEntityTypeConfiguration<Student>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable(nameof(Student));
            //builder.ToTable("student");
            builder.HasKey(n => n.Id); 
            base.Configure(builder);
        }

        #endregion
    }
}
