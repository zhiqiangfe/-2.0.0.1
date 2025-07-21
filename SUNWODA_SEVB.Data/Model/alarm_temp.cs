using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// alarm_temp:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class alarm_temp
    {
        public alarm_temp() { }

        #region Model
        private int _id;
        private string? _equipment_id;
        private string? _upload_param_id;
        private DateTime _alarm_time;
        private int _dispose_state = 0;
        private DateTime? _dispose_time;
        private string? _create_by;
        private string? _mhandler;
        private decimal _duration = 0.00M;

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
        public DateTime alarm_time
        {
            set { _alarm_time = value; }
            get { return _alarm_time; }
        }

        /// <summary>
        ///
        /// </summary>
        public int dispose_state
        {
            set { _dispose_state = value; }
            get { return _dispose_state; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? dispose_time
        {
            set { _dispose_time = value; }
            get { return _dispose_time; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? create_by
        {
            set { _create_by = value; }
            get { return _create_by; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? mhandler
        {
            set { _mhandler = value; }
            get { return _mhandler; }
        }

        /// <summary>
        ///
        /// </summary>
        public decimal duration
        {
            set { _duration = value; }
            get { return _duration; }
        }
        #endregion Model
    }
}
