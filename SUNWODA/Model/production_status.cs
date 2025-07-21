using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// probably:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class production_status
    {
        public production_status() { }

        #region Model
        private int _id;
        private int? _plc_config_id;
        private string? _user_name;
        private int _ok_count = 0;
        private int? _ng_count = 0;
        private string? _is_baking = "false";
        private int _ppm = 0;
        private string? _model;
        private int _feed_count = 0;
        private int _discharge_count = 0;

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
        public int? plc_config_id
        {
            set { _plc_config_id = value; }
            get { return _plc_config_id; }
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
        public int ok_count
        {
            set { _ok_count = value; }
            get { return _ok_count; }
        }

        /// <summary>
        ///
        /// </summary>
        public int? ng_count
        {
            set { _ng_count = value; }
            get { return _ng_count; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? is_baking
        {
            set { _is_baking = value; }
            get { return _is_baking; }
        }

        /// <summary>
        ///
        /// </summary>
        public int ppm
        {
            set { _ppm = value; }
            get { return _ppm; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? model
        {
            set { _model = value; }
            get { return _model; }
        }

        /// <summary>
        ///
        /// </summary>
        public int feed_count
        {
            set { _feed_count = value; }
            get { return _feed_count; }
        }

        /// <summary>
        ///
        /// </summary>
        public int discharge_count
        {
            set { _discharge_count = value; }
            get { return _discharge_count; }
        }
        #endregion Model
    }
}
