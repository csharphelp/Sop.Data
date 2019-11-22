using System.Collections.Generic;

namespace Sop.Domain.Domain.Services
{
    public interface IValuesService
    {
        IEnumerable<string> FindAll();

        string Find(int id);
    }
    
}
