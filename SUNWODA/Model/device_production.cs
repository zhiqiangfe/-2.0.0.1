using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_production
    {
        public device_production() { }

        #region Model
        private string? _id;
        private int? _plc_config_id;
        private string? _name;
        private int? _enabled;
        private string? _group_code;
        private string? _group_alias;
        private string? _remark;

        /// <summary>
        ///
        /// </summary>
        public string? id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        ///
        /// </summary>
        public int? plc_config_id
        {
            set { _plc_config_id = value; }
            get { return _plc_config_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        ///
        /// </summary>
        public int? enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? group_code
        {
            set { _group_code = value; }
            get { return _group_code; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? group_alias
        {
            set { _group_alias = value; }
            get { return _group_alias; }
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
