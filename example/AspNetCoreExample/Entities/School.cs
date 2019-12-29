using System;
using Sop.Data;

namespace AspNetCoreExample.Entities
{
    public class School : IEntity
    {
        public int Id { get; set; }
        public string SchoolId { get; set; }
        public string Name { get; set; }
    }
}
