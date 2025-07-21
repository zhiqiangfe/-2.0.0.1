using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// valuetypes:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class value_types
    {
        public value_types() { }

        #region Model
        private string? _value_type;
        private string? _remark;

        /// <summary>
        ///
        /// </summary>
        public string? value_type
        {
            set { _value_type = value; }
            get { return _value_type; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model
    }
}
