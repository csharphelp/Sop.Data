using System.Collections.Generic;

namespace Sop.WebApi.Services
{
    /// <summary>
    /// 
    /// </summary>
    public interface IValuesService
    {
        IEnumerable<string> FindAll();

        string Find(int id);
    }
}