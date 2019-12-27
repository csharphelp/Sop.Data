using System;
using System.Collections.Generic;
using System.Text;
using Sop.Data;

namespace AspNetFrameworkExample.Entities
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
