using System;
using Sop.Data;

namespace AspNetCoreExample.Entities
{
    public class Student : IEntity
    {

        public int Id { get; set; }


        public int Type { get; set; }


        public bool IsDel { get; set; }


        public Status Status { get; set; }


        public long LongValue { get; set; }


        public float FloatValue { get; set; }


        public decimal DecimalValue { get; set; }


        public string Body { get; set; }


        public DateTime DateCreated { get; set; }

        public string SchoolId { get; set; }
    }
}
