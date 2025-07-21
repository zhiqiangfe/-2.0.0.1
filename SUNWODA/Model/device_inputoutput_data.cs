using System;
using System.ComponentModel;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_inputoutput_data:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_inputoutput_data
    {
        public device_inputoutput_data() { }

        #region Model
        private int _id;
        private string? _equipment_id;
        private string? _upload_param_id;
        private string? _param_name;
        private double? _value;
        private DateTime? _data_time;
        private string? _param_unit;

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
        [Description("设备编号")]
        public string? equipment_id
        {
            set { _equipment_id = value; }
            get { return _equipment_id; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("参数ID")]
        public string? upload_param_id
        {
            set { _upload_param_id = value; }
            get { return _upload_param_id; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("参数名称")]
        public string? param_name
        {
            set { _param_name = value; }
            get { return _param_name; }
        }

        /// <summary>
        ///
        /// </summary>
        ///
        [Description("单位")]
        public string? param_unit
        {
            set { _param_unit = value; }
            get { return _param_unit; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("值")]
        public double? value
        {
            set { _value = value; }
            get { return _value; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("记录时间")]
        public DateTime? data_time
        {
            set { _data_time = value; }
            get { return _data_time; }
        }
        #endregion Model
    }
}
