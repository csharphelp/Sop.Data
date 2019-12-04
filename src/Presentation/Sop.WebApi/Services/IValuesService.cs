using System.Collections.Generic;

namespace Sop.WebApi.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface iValuesService
    {
        IEnumerable<string> FindAll();

        string Find(int id);
    }
}