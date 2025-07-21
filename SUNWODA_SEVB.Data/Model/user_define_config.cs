using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// user_define_config:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class user_define_config
    {
        public user_define_config() { }

        #region Model
        private int _id;
        private string? _config_list;
        private string? _state;
        private string? _remark;
        private DateTime _datatime;
        private int _cycle;
        private string? _equipment_id;
        private string? _signal_address;

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
        public string? config_list
        {
            set { _config_list = value; }
            get { return _config_list; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? state
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
        /// on update current_timestamp()
        /// </summary>
        public DateTime datatime
        {
            set { _datatime = value; }
            get { return _datatime; }
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
        public string? equipment_id
        {
            set { _equipment_id = value; }
            get { return _equipment_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? signal_address
        {
            set { _signal_address = value; }
            get { return _signal_address; }
        }
        #endregion Model
    }
}
