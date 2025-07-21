using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_spart_config:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_spart_config
    {
        public device_spart_config() { }

        #region Model
        private int _id;
        private string? _equipment_id;
        private int _plc_config_id;
        private string? _upload_param_id;
        private string? _param_name;
        private string? _type;
        private decimal _spart_expected_life = 0M;
        private string? _mes_download_used_life;
        private string? _mes_is_download_plc;
        private string? _set_life_address;
        private string? _used_life_address;
        private string? _limit_control;
        private string? _status;
        private string? _first_alarm_address;
        private string? _second_alarm_address;
        private string? _thread_alarm_address;
        private string? _first_alarm_over;
        private string? _second_alarm_over;
        private string? _param_unit;
        private int _param_value_rate = 0;
        private decimal _percent_warning = 0M;
        private DateTime _change_date;
        private string? _change_user;
        private int? _plc_rw_config_id;

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
        public int plc_config_id
        {
            set { _plc_config_id = value; }
            get { return _plc_config_id; }
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
        public decimal spart_expected_life
        {
            set { _spart_expected_life = value; }
            get { return _spart_expected_life; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? mes_download_used_life
        {
            set { _mes_download_used_life = value; }
            get { return _mes_download_used_life; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? mes_is_download_plc
        {
            set { _mes_is_download_plc = value; }
            get { return _mes_is_download_plc; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? set_life_address
        {
            set { _set_life_address = value; }
            get { return _set_life_address; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? used_life_address
        {
            set { _used_life_address = value; }
            get { return _used_life_address; }
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
        public string? status
        {
            set { _status = value; }
            get { return _status; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? first_alarm_address
        {
            set { _first_alarm_address = value; }
            get { return _first_alarm_address; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? second_alarm_address
        {
            set { _second_alarm_address = value; }
            get { return _second_alarm_address; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? thread_alarm_address
        {
            set { _thread_alarm_address = value; }
            get { return _thread_alarm_address; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? first_alarm_over
        {
            set { _first_alarm_over = value; }
            get { return _first_alarm_over; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? second_alarm_over
        {
            set { _second_alarm_over = value; }
            get { return _second_alarm_over; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? param_unit
        {
            set { _param_unit = value; }
            get { return _param_unit; }
        }

        /// <summary>
        ///
        /// </summary>
        public int param_value_rate
        {
            set { _param_value_rate = value; }
            get { return _param_value_rate; }
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

        /// <summary>
        ///
        /// </summary>
        public int? plc_rw_config_id
        {
            set { _plc_rw_config_id = value; }
            get { return _plc_rw_config_id; }
        }
        #endregion Model
    }
}
