namespace Sop.Core.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class HtmlDiffMatch
    {
        public HtmlDiffMatch(int startInOld, int startInNew, int size)
        {
            this.StartInOld = startInOld;
            this.StartInNew = startInNew;
            this.Size = size;
        }

        public int StartInOld { get; set; }

        public int StartInNew { get; set; }

        public int Size { get; set; }

        public int EndInOld
        {
            get
            {
                return this.StartInOld + this.Size;
            }
        }

        public int EndInNew
        {
            get
            {
                return this.StartInNew + this.Size;
            }
        }
    }
}