using AspNetCoreExample.Entities;
using Microsoft.EntityFrameworkCore;
using Sop.Data;
using Sop.Data.Repository;

namespace AspNetCoreExample.Repositories
{
    public class StudentRepository : EfCoreRepository<Student>, IStudentRepository
    {
        public StudentRepository(DbContext context) : base(context)
        {
        }
    }

    public interface IStudentRepository : IRepository<Student>
    {

    }
}
