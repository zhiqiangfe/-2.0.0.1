using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_config:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_config
    {
        public device_config() { }

        #region Model
        private string? _id;
        private string? _devicename;
        private string? _labchinese;
        private string? _labenglish;
        private string? _workship;
        private string? _prefixofvariables;
        private int? _plc_config_id;
        private string? _group_devicealarm;
        private string? _description;
        private int? _enabled = 1;
        private string? _remark;
        private DateTime _datatime;

        /// <summary>
        /// 设备标号
        /// devicesn in user_define_veriable
        /// </summary>
        public string? id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 设备名称
        /// </summary>
        public string? devicename
        {
            set { _devicename = value; }
            get { return _devicename; }
        }

        /// <summary>
        /// 工序名称（中文）
        /// </summary>
        public string? labchinese
        {
            set { _labchinese = value; }
            get { return _labchinese; }
        }

        /// <summary>
        /// 工序名称（英文）
        /// </summary>
        public string? labenglish
        {
            set { _labenglish = value; }
            get { return _labenglish; }
        }

        /// <summary>
        /// 工段信息
        /// </summary>
        public string? workship
        {
            set { _workship = value; }
            get { return _workship; }
        }

        /// <summary>
        /// 变量名称的前缀
        /// variables in user_define_variable
        /// </summary>
        public string? prefixofvariables
        {
            set { _prefixofvariables = value; }
            get { return _prefixofvariables; }
        }

        /// <summary>
        /// plc_config_id
        /// </summary>
        public int? plc_config_id
        {
            set { _plc_config_id = value; }
            get { return _plc_config_id; }
        }

        /// <summary>
        /// 设备报警编码分组标识
        /// </summary>
        public string? group_devicealarm
        {
            set { _group_devicealarm = value; }
            get { return _group_devicealarm; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string? description
        {
            set { _description = value; }
            get { return _description; }
        }

        /// <summary>
        /// 使能标识
        /// </summary>
        public int? enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        /// <summary>
        /// on update current_timestamp()
        /// </summary>
        public DateTime datatime
        {
            set { _datatime = value; }
            get { return _datatime; }
        }
        #endregion Model
    }
}
