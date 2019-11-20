using Sop.Data;
using Sop.Domain.Entities;

namespace Sop.Domain.Services
{
    public class TestService : ITestService
    {
        public IRepository<Tests> TestRepository;


        public void Create(Tests info)
        {
            var result = TestRepository.Create(info);
        }
    }
}