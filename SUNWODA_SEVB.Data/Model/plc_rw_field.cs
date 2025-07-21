using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// plc_rw_field:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class plc_rw_field
    {
        public plc_rw_field() { }

        #region Model
        private string? _rw;
        private string? _remark;

        /// <summary>
        ///
        /// </summary>
        public string? rw
        {
            set { _rw = value; }
            get { return _rw; }
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
