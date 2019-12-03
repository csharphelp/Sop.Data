using System.Collections.Generic;

namespace Sop.WebApi
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