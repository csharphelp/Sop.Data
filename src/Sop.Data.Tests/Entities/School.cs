using System;

namespace Sop.Data.Tests.Entities
{
    public class School : IEntity
    {
        public int Id { get; set; }
        public string SchoolId { get; set; }
        public string Name { get; set; }
    }
}
