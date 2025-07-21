using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// role:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class user_role
    {
        public user_role() { }

        #region Model
        private int _id;
        private string? _role_name;
        private string? _mes_user_level;
        private int? _user_level_plc_value;
        private string? _permission_codes;
        private DateTime _create_time;
        private DateTime _modify_time;
        private string? _remark;

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
        public string? role_name
        {
            set { _role_name = value; }
            get { return _role_name; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? mes_user_level
        {
            set { _mes_user_level = value; }
            get { return _mes_user_level; }
        }

        /// <summary>
        ///
        /// </summary>
        public int? user_level_plc_value
        {
            set { _user_level_plc_value = value; }
            get { return _user_level_plc_value; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? permission_codes
        {
            set { _permission_codes = value; }
            get { return _permission_codes; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }

        /// <summary>
        /// on update current_timestamp()
        /// </summary>
        public DateTime modify_time
        {
            set { _modify_time = value; }
            get { return _modify_time; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model
    }
}
