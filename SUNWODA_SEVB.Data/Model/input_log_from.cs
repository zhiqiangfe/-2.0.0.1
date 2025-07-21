using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// input_log_from:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class input_log_from
    {
        public input_log_from() { }

        #region Model
        private string? _source;

        /// <summary>
        ///
        /// </summary>
        public string? source
        {
            set { _source = value; }
            get { return _source; }
        }
        #endregion Model
    }
}
