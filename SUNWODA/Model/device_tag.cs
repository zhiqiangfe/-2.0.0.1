using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_tag:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_tag
    {
        public device_tag() { }

        #region Model
        private int _id;
        private int _plc_config_id;
        private string? _tag_name;
        private string? _value_type;
        private string? _description;

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
        public int plc_config_id
        {
            set { _plc_config_id = value; }
            get { return _plc_config_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? tag_name
        {
            set { _tag_name = value; }
            get { return _tag_name; }
        }

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
        public string? description
        {
            set { _description = value; }
            get { return _description; }
        }
        #endregion Model
    }
}
