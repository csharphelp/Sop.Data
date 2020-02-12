using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sop.Data.Tests.Entities;
using Sop.Data.Tests.Repositories;
using System;
using System.Data;
using Sop.Data.Repository;
using Xunit;

namespace Sop.Data.Tests
{
    /// <summary>
    /// 
    /// </summary>
    public class EfDbBaseDbContext : BaseDbContext
    {
        public EfDbBaseDbContext(DbContextOptions options) : base(options)
        {
            base.SetOnModelCreatingType(OnModelCreatingType.UseEntityMap);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}
