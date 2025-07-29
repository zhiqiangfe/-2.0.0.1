using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// plc_protocol:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class plc_protocol
    {
        public plc_protocol() { }

        #region Model
        private string? _protocol_name;
        private string? _model;
        private string? _remark;

        /// <summary>
        ///
        /// </summary>
        public string? protocol_name
        {
            set { _protocol_name = value; }
            get { return _protocol_name; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? model
        {
            set { _model = value; }
            get { return _model; }
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
