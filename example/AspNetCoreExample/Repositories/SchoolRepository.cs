using AspNetCoreExample.Entities;
using Microsoft.EntityFrameworkCore;
using Sop.Data;
using Sop.Data.Repository;

namespace AspNetCoreExample.Repositories
{
    public class SchoolRepository : EfCoreRepository<School>, ISchoolRepository
    {
        public SchoolRepository(DbContext context) : base(context)
        {
        }
    }

    public interface ISchoolRepository : IRepository<School>
    {

    }



}
