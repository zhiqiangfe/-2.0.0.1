using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// plc_config:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class plc_config
    {
        public plc_config() { }

        #region Model
        private int _id;
        private string? _name;
        private string? _device_id;
        private string? _ip;
        private string? _port;
        private string? _brand_specification_protocal;
        private string? _data_sort_rule;
        private int? _state;
        private string? _remark;
        private int _enabled = 1;

        /// <summary>
        /// auto_increment
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }

        /// <summary>
        /// PLC名称
        /// </summary>
        public string? name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 设备ID
        /// </summary>
        public string? device_id
        {
            set { _device_id = value; }
            get { return _device_id; }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        public string? ip
        {
            set { _ip = value; }
            get { return _ip; }
        }

        /// <summary>
        /// 端口号
        /// </summary>
        public string? port
        {
            set { _port = value; }
            get { return _port; }
        }

        /// <summary>
        /// 品牌_规格_协议
        /// </summary>
        public string? brand_specification_protocal
        {
            set { _brand_specification_protocal = value; }
            get { return _brand_specification_protocal; }
        }

        public string? data_sort_rule
        {
            set { _data_sort_rule = value; }
            get { return _data_sort_rule; }
        }

        /// <summary>
        /// PLC状态
        /// </summary>
        public int? state
        {
            set { _state = value; }
            get { return _state; }
        }

        /// <summary>
        /// 注释
        /// </summary>
        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        /// <summary>
        /// 是否启用
        /// </summary>
        public int enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }
        #endregion Model
    }
}
