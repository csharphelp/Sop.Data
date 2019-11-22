using System;

namespace Sop.Domain.Domain.Entities
{
    /// <summary>
    ///     Sop_Test 测试数据实体类
    /// </summary>
    [Serializable]
    public class Tests
    {
        /// <summary>
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        ///     Type
        /// </summary>
        public virtual TestType Type { get; set; }

        /// <summary>
        ///     IsDel tinyint
        /// </summary>
        public virtual bool IsDel { get; set; }

        /// <summary>
        ///     Status bit
        /// </summary>
        public virtual bool Status { get; set; }

        /// <summary>
        ///     LongValue
        /// </summary>
        public virtual long LongValue { get; set; }

        /// <summary>
        ///     FloatValue
        /// </summary>
        public virtual float FloatValue { get; set; }

        /// <summary>
        ///     DecimalValue
        /// </summary>
        public virtual decimal DecimalValue { get; set; }

        /// <summary>
        ///     Body
        /// </summary>
        public virtual string Body { get; set; }

        /// <summary>
        ///     DateCreated
        /// </summary>
        public virtual DateTime DateCreated { get; set; }
    }
}