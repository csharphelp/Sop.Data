using System;
using System.Collections.Generic;
using System.Text;

namespace Sop.Data.Tests.Entities
{
    public class SchoolAndStudent : IEntity
    {

        public int Id { get; set; } 

        public string Body { get; set; }


        public DateTime DateCreated { get; set; }

        public string SchoolId { get; set; }
        public string SchoolName { get; set; }
    }
}
