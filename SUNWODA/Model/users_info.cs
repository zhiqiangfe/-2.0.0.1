using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// users:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class users_info
    {
        public users_info() { }

        #region Model
        private int _id;
        private string? _user_name;
        private string? _password;
        private string? _name;
        private int _role_id;
        private DateTime _create_time;
        private int? _gender;
        private DateTime? _last_login_time;
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
        public string? user_name
        {
            set { _user_name = value; }
            get { return _user_name; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? password
        {
            set { _password = value; }
            get { return _password; }
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
        public int role_id
        {
            set { _role_id = value; }
            get { return _role_id; }
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
        ///
        /// </summary>
        public int? gender
        {
            set { _gender = value; }
            get { return _gender; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? last_login_time
        {
            set { _last_login_time = value; }
            get { return _last_login_time; }
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
