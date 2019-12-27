using System.Collections.Generic;
using System.Web.Http;
using Microsoft.Extensions.DependencyInjection;
using Sop.Data;
using Sop.Data.Tests.Repositories;

namespace AspNetFrameworkExample.Controllers
{
    public class HomeController : ApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISchoolRepository _schoolRepository;
       private readonly IStudentRepository _studentRepository;
        private readonly int count = 1000;

        public HomeController()
        {
            var services = new ServiceCollection();

            services.AddSopData(opt =>
            {
            });
            //services.AddSopData<EfDbBaseDbContext>(opt =>
            //{
            //    opt.UseMySql("server =127.0.0.1;database=soptestdb;uid=root;password=123456;");
            //});

            var sp = services.BuildServiceProvider();
            _unitOfWork = sp.GetRequiredService<IUnitOfWork>();
            _schoolRepository = sp.GetRequiredService<ISchoolRepository>();
            _studentRepository = sp.GetService<IStudentRepository>();
        }
        // GET: api/Home
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Home/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Home
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Home/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Home/5
        public void Delete(int id)
        {
        }
    }
}
