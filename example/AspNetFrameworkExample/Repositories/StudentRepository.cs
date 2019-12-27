﻿using Microsoft.EntityFrameworkCore;
using Sop.Data.Repository;
using AspNetFrameworkExample.Entities;

namespace Sop.Data.Tests.Repositories
{
    public class StudentRepository: EfCoreRepository<Student>,IStudentRepository
    {
        public StudentRepository(DbContext context) : base(context)
        {
        }
    }

    public interface IStudentRepository : IRepository<Student>
    {

    }
}
