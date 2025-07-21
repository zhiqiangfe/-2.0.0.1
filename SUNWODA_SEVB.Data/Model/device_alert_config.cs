using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_alert_config:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_alert_config
    {
        public device_alert_config() { }

        #region Model
        private int _id;
        private string? _equipment_id;
        private int? _plc_config_id;
        private int? _plc_rw_config_id;
        private string? _upload_param_id;
        private string? _param_name;
        private string? _alert_level;
        private string? _alert_address;
        private DateTime _data_time;

        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? equipment_id
        {
            set { _equipment_id = value; }
            get { return _equipment_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public int? plc_config_id
        {
            set { _plc_config_id = value; }
            get { return _plc_config_id; }
        }
        public int? plc_rw_config_id
        {
            set { _plc_rw_config_id = value; }
            get { return _plc_rw_config_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? upload_param_id
        {
            set { _upload_param_id = value; }
            get { return _upload_param_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? param_name
        {
            set { _param_name = value; }
            get { return _param_name; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? alert_level
        {
            set { _alert_level = value; }
            get { return _alert_level; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? alert_address
        {
            set { _alert_address = value; }
            get { return _alert_address; }
        }

        /// <summary>
        /// on update current_timestamp()
        /// </summary>
        public DateTime data_time
        {
            set { _data_time = value; }
            get { return _data_time; }
        }
        #endregion Model
    }
}
