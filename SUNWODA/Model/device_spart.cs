using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_spart:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_spart
    {
        public device_spart() { }

        #region Model
        private int _id;
        private string? _equipment_id;
        private string? _upload_param_id;
        private string? _param_name;
        private string? _type;
        private int _spart_expected_life;
        private decimal _mes_download_used_life;
        private decimal _used_life;
        private string? _limit_control;
        private decimal _percent_warning;
        private decimal _param_value_rate;
        private DateTime _change_date;
        private string? _change_user;

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
        public string? equipment_id
        {
            set { _equipment_id = value; }
            get { return _equipment_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? upload_param_id
        {
            set { _upload_param_id = value; }
            get { return _upload_param_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? param_name
        {
            set { _param_name = value; }
            get { return _param_name; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? type
        {
            set { _type = value; }
            get { return _type; }
        }

        /// <summary>
        ///
        /// </summary>
        public int spart_expected_life
        {
            set { _spart_expected_life = value; }
            get { return _spart_expected_life; }
        }

        /// <summary>
        ///
        /// </summary>
        public decimal mes_download_used_life
        {
            set { _mes_download_used_life = value; }
            get { return _mes_download_used_life; }
        }

        /// <summary>
        ///
        /// </summary>
        public decimal used_life
        {
            set { _used_life = value; }
            get { return _used_life; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? limit_control
        {
            set { _limit_control = value; }
            get { return _limit_control; }
        }

        /// <summary>
        ///
        /// </summary>
        public decimal percent_warning
        {
            set { _percent_warning = value; }
            get { return _percent_warning; }
        }

        /// <summary>
        ///
        /// </summary>
        public decimal param_value_rate
        {
            set { _param_value_rate = value; }
            get { return _param_value_rate; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime change_date
        {
            set { _change_date = value; }
            get { return _change_date; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? change_user
        {
            set { _change_user = value; }
            get { return _change_user; }
        }
        #endregion Model
    }
}
