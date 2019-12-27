﻿using Microsoft.EntityFrameworkCore;
using Sop.Data.Repository;
using AspNetFrameworkExample.Entities;

namespace Sop.Data.Tests.Repositories
{
    public class SchoolRepository: EfCoreRepository<School>,ISchoolRepository
    {
        public SchoolRepository(DbContext context) : base(context)
        {
        }
    }

    public interface ISchoolRepository : IRepository<School>
    {

    }


   
}
