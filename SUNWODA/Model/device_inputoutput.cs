using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_inputoutput:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_inputoutput
    {
        public device_inputoutput() { }

        #region Model
        private int _id;
        private string? _equipment_id;
        private string? _send_param_id;
        private string? _upload_param_id;
        private string? _param_name;
        private string? _type;
        private string? _set_value;
        private string? _upper_limit_value;
        private string? _lower_limit_value;
        private string? _limit_control;
        private string? _change_monitor;
        private string? _actual_value;
        private string? _bycell_output;
        private decimal _param_value_rate;

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
        public string? send_param_id
        {
            set { _send_param_id = value; }
            get { return _send_param_id; }
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
        public string? type
        {
            set { _type = value; }
            get { return _type; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? set_value
        {
            set { _set_value = value; }
            get { return _set_value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? upper_limit_value
        {
            set { _upper_limit_value = value; }
            get { return _upper_limit_value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? lower_limit_value
        {
            set { _lower_limit_value = value; }
            get { return _lower_limit_value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? limit_control
        {
            set { _limit_control = value; }
            get { return _limit_control; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? change_monitor
        {
            set { _change_monitor = value; }
            get { return _change_monitor; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? actual_value
        {
            set { _actual_value = value; }
            get { return _actual_value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? bycell_output
        {
            set { _bycell_output = value; }
            get { return _bycell_output; }
        }

        /// <summary>
        ///
        /// </summary>
        public decimal param_value_rate
        {
            set { _param_value_rate = value; }
            get { return _param_value_rate; }
        }
        #endregion Model
    }
}
