using System;
using System.Collections.Generic;
using System.Text;

namespace Sop.Data.Tests.Entities
{
    public class SchoolAndStudentDto 
    {

        public int Id { get; set; } 

        public string Body { get; set; }


        public DateTime DateCreated { get; set; }

        public string SchoolId { get; set; }
        public string SchoolName { get; set; }
    }
}
