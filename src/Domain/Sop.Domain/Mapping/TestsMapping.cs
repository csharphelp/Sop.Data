using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using Sop.Domain.Entities;

namespace Sop.Domain.Mapping
{
    /// <summary>
    /// </summary>
    public class TestsMapping : ClassMapping<Tests>
    {
        public TestsMapping()
        {
            Table("Sop_Test");
            Cache(map => map.Usage(CacheUsage.ReadWrite));
            Id(t => t.Id, map => map.Generator(Generators.Identity));
            Property(t => t.Type);
            Property(t => t.IsDel);
            Property(t => t.Status);
            Property(t => t.LongValue);
            Property(t => t.FloatValue);
            Property(t => t.DecimalValue);
            Property(t => t.Body);
            Property(t => t.DateCreated);
        }
    }
}