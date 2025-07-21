using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// variabletype:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class variable_type
    {
        public variable_type() { }

        #region Model
        private int _id;
        private string? _remark;

        /// <summary>
        ///
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
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
