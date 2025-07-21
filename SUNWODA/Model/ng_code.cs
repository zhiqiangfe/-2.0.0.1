using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// ngcode:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class ng_code
    {
        public ng_code() { }

        #region Model
        private string? _code;
        private string? _reason;

        /// <summary>
        ///
        /// </summary>
        public string? code
        {
            set { _code = value; }
            get { return _code; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? reason
        {
            set { _reason = value; }
            get { return _reason; }
        }
        #endregion Model
    }
}
