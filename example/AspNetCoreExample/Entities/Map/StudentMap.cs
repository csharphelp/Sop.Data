using System;
using System.Collections.Generic;
using System.Text;
using AspNetCoreExample.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sop.Data.Mapping;

namespace AspNetCoreExample.Entities.Map
{

    public partial class StudentMap : BaseMapEntityTypeConfiguration<Student>
    {
        #region Methods

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
