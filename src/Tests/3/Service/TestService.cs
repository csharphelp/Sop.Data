using System;
using System.Collections.Generic;
using System.Text;
using Sop.Data.Environment;
using Sop.Data.NhRepositories;
using Sop.Data.Test.Models;

namespace Sop.Data.Test.Service
{
    public class TestService : ITestService
    {
        public IRepository<Tests> TestRepository;


        public void Create(Models.Tests info)
        {
            var result = TestRepository.Create(info);
        }



    }
}
