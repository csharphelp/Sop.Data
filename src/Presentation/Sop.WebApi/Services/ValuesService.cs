using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Sop.Data;
using Sop.Data.NhRepositories;
using Sop.Domain.Domain.Entities;

namespace Sop.WebApi.Services
{
    /// <summary>
    /// 
    /// </summary>

    public class ValuesService : IValuesService
    {
        private readonly ILogger<ValuesService> _logger;
        public IRepository<Tests> _testRepository { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ValuesService(ILogger<ValuesService> logger)
        {
            this._logger = logger;
//            this._testRepository = testRepository;
        }

        public IEnumerable<string> FindAll()
        {
            _testRepository.Create(new Tests
            {
               
                Type = TestType.Again,
                IsDel = true,
                Status = false,
                LongValue = 123131L,
                FloatValue = 123213213,
                DecimalValue =333 ,
                Body ="dsasssśsà   " ,
                DateCreated = DateTime.Now
            });
            
            
            
            this._logger.LogDebug("{method} called", nameof(this.FindAll));

            return new[] { "value1", "value2" };
         }

        public string Find(int id)
        {
            this._logger.LogDebug("{method} called with {id}", nameof(this.Find), id);

            return $"value{id}";
        }


    }
}