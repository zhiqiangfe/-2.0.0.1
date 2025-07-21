using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// plc_rw_config:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class plc_rw_config
    {
        public plc_rw_config() { }

        #region Model
        private int _id;
        private string? _name;
        private int _plc_config_id;
        private string? _area_name;
        private string? _start_address;
        private int _length;
        private string? _rw;
        private int _cycle;
        private DateTime _last_time;
        private int _enabled = 1;
        private int _address_type = 1;

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
        public string? name
        {
            set { _name = value; }
            get { return _name; }
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
        public string? area_name
        {
            set { _area_name = value; }
            get { return _area_name; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? start_address
        {
            set { _start_address = value; }
            get { return _start_address; }
        }

        /// <summary>
        ///
        /// </summary>
        public int length
        {
            set { _length = value; }
            get { return _length; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? rw
        {
            set { _rw = value; }
            get { return _rw; }
        }

        /// <summary>
        ///
        /// </summary>
        public int cycle
        {
            set { _cycle = value; }
            get { return _cycle; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime last_time
        {
            set { _last_time = value; }
            get { return _last_time; }
        }

        /// <summary>
        ///
        /// </summary>
        public int enabled
        {
            set { _enabled = value; }
            get { return _enabled; }
        }

        /// <summary>
        ///
        /// </summary>
        public int address_type
        {
            set { _address_type = value; }
            get { return _address_type; }
        }
        #endregion Model
    }
}
