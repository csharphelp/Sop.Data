using Sop.Data;
using Sop.Domain.Domain.Entities;
using Sop.Domain.Domain.Services;

namespace Sop.Domain.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class TestService : ITestService
    {

        /// <summary>
        /// 
        /// </summary>
        public IRepository<Tests> TestRepository;

/// <summary>
/// 
/// </summary>
/// <param name="info"></param>
        public void Create(Tests info)
        {
            //var result = TestRepository.Create(info);
        }
    }
}