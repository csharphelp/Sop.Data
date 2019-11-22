using System.Collections.Generic;

namespace Sop.Domain.Domain.Services
{
    public interface IPostService
    {
        IEnumerable<string> FindAll();

        string Find(int id);
    }
    
}
