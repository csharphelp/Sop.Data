namespace Sop.Core.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlDiffOperation
    {
        /// <summary>
        /// 
        /// </summary>
        public HtmlDiffAction Action { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int StartInOld { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EndInOld { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int StartInNew { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int EndInNew { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="startInOld"></param>
        /// <param name="endInOld"></param>
        /// <param name="startInNew"></param>
        /// <param name="endInNew"></param>
        public HtmlDiffOperation(HtmlDiffAction action, int startInOld, int endInOld, int startInNew, int endInNew)
        {
            this.Action = action;
            this.StartInOld = startInOld;
            this.EndInOld = endInOld;
            this.StartInNew = startInNew;
            this.EndInNew = endInNew;
        }
    }
}