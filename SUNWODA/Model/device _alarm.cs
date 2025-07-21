using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_alarm:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_alarm
    {
        public device_alarm() { }

        #region Model
        private int _id;
        private string? _alarmcode_ime;
        private string? _alarmcode_vendor;
        private string? _groupremark;
        private string? _alarmclassify;
        private string? _alarmdescription;
        private int _alarmlevel;
        private string? _alarmpart;
        private string? _collectionremark;
        private string? _value_type;
        private int _variable_length = 1;
        private int? _plc_rw_config_id;
        private string? _plc_address;
        private string? _description;
        private bool _is_monitor = false;
        private int? _enabled = 1;
        private string? _remark;
        private DateTime _datatime;

        /// <summary>
        /// id
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// 报警编码（IME）
        /// </summary>
        public string? alarmcode_ime
        {
            set { _alarmcode_ime = value; }
            get { return _alarmcode_ime; }
        }

        /// <summary>
        /// 报警编码（供应商）
        /// </summary>
        public string? alarmcode_vendor
        {
            set { _alarmcode_vendor = value; }
            get { return _alarmcode_vendor; }
        }

        /// <summary>
        /// 分组标识
        /// </summary>
        public string? groupremark
        {
            set { _groupremark = value; }
            get { return _groupremark; }
        }

        /// <summary>
        /// 报警分类
        /// </summary>
        public string? alarmclassify
        {
            set { _alarmclassify = value; }
            get { return _alarmclassify; }
        }

        /// <summary>
        /// 报警描述
        /// </summary>
        public string? alarmdescription
        {
            set { _alarmdescription = value; }
            get { return _alarmdescription; }
        }

        /// <summary>
        /// 报警等级
        /// </summary>
        public int alarmlevel
        {
            set { _alarmlevel = value; }
            get { return _alarmlevel; }
        }

        /// <summary>
        /// 报警故障部件
        /// </summary>
        public string? alarmpart
        {
            set { _alarmpart = value; }
            get { return _alarmpart; }
        }

        /// <summary>
        /// 报警采集备注
        /// </summary>
        public string? collectionremark
        {
            set { _collectionremark = value; }
            get { return _collectionremark; }
        }

        /// <summary>
        /// 报警地址数据值类型
        /// </summary>
        public string? value_type
        {
            set { _value_type = value; }
            get { return _value_type; }
        }

        /// <summary>
        /// 报警地址数据长度
        /// </summary>
        public int variable_length
        {
            set { _variable_length = value; }
            get { return _variable_length; }
        }

        /// <summary>
        /// plc_rw_config_id
        /// </summary>
        public int? plc_rw_config_id
        {
            set { _plc_rw_config_id = value; }
            get { return _plc_rw_config_id; }
        }

        /// <summary>
        /// PLC地址
        /// </summary>
        public string? plc_address
        {
            set { _plc_address = value; }
            get { return _plc_address; }
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
        /// 是否进行数据监控
        /// </summary>
        public bool is_monitor
        {
            set { _is_monitor = value; }
            get { return _is_monitor; }
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
