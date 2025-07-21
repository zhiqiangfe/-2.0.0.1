using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// user_define_data:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class user_define_data
    {
        public user_define_data() { }

        #region Model
        private int _id;
        private int _user_define_config_id;
        private string? _param_names;
        private string? _param_values;
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
        public int user_define_config_id
        {
            set { _user_define_config_id = value; }
            get { return _user_define_config_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? param_names
        {
            set { _param_names = value; }
            get { return _param_names; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? param_values
        {
            set { _param_values = value; }
            get { return _param_values; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime data_time
        {
            set { _data_time = value; }
            get { return _data_time; }
        }
        #endregion Model
    }
}
