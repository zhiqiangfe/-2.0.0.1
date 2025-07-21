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
        private string? _plc_name;
        private string? _equipment_id;
        private string? _address;
        private string? _address_para;
        private string? _protocol_name;
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
        ///
        /// </summary>
        public string? plc_name
        {
            set { _plc_name = value; }
            get { return _plc_name; }
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
        public string? address
        {
            set { _address = value; }
            get { return _address; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? address_para
        {
            set { _address_para = value; }
            get { return _address_para; }
        }

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
        public int? state
        {
            set { _state = value; }
            get { return _state; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }

        /// <summary>
        ///
        /// </summary>
        public int enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }
        #endregion Model
    }
}
