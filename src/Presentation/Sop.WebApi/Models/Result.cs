namespace Sop.WebApi.Data
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public class Result<TData>
    {
        public Result(ResultCode resultCode = ResultCode.Success)
        {
            Code = resultCode;
            Message = "";
        }
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
        public bool Status { get; set; } = true;
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
}
