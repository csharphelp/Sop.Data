using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Sop.Domain.Services
{
    public class PostService : Domain.Services.IPostService
    {
        public PostService()
        {
        }

        public IEnumerable<string> FindAll()
        {
           

            return new[] { "value1", "value2" };
        }

        public string Find(int id)
        {  
            return $"value{id}";
        }
    }
}
