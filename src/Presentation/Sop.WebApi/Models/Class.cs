using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sop.WebApi.Data
{
    public class Class
    {
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class Result<TData>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="message"></param>
        public Result(ResultCode resultCode, string message = "")
        {
            Code = resultCode;
            Message = message;
        }

        /// <summary>
        /// 
        /// </summary>
        public ResultCode Code { get; set; } = ResultCode.Success;
        /// <summary>
        /// 
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public TData Data { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ResultCode
    {
        /// <summary>
        /// 
        /// </summary>
        Success = 200,
        /// <summary>
        /// 
        /// </summary>
        Fail = 400
    }
}
