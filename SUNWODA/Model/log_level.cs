using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// loglevel:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class log_level
    {
        public log_level() { }

        #region Model
        private string? _level;

        /// <summary>
        ///
        /// </summary>
        public string? level
        {
            set { _level = value; }
            get { return _level; }
        }
        #endregion Model
    }
}
