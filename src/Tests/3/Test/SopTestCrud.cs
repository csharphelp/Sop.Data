using System;
using Autofac;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sop.Data.Environment;
using Sop.Data.Test.Models;
using Sop.Data.Test.Service;

namespace Sop.Data.Test.Test
{
    [TestClass]
    public class SopTestCrud
    {
        public ITestService TestService = DiContainer.Resolve<TestService>();
         
        [TestMethod]
        public void Test_Create()
        {
            var info = new Models.Tests()
            {
                IsDel = true,
                Body = "TEST",
                DateCreated = DateTime.Now,
                DecimalValue = 123.34M,
                FloatValue = 123322.32f,
                LongValue = 1467896543L,
                Status = false,
                Type = TestType.Again
            };

            TestService.Create(info);


        }

    }
}
