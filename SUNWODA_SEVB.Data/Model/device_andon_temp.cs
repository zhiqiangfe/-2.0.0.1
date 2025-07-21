using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_andon_temp:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_andon_temp
    {
        public device_andon_temp() { }

        #region Model
        private int _id;
        private int? _dispose_state;
        private DateTime? _create_time;
        private DateTime? _dispose_time;
        private string? _andon_code;
        private string? _andon_description;
        private decimal? _duration;

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
        public int? dispose_state
        {
            set { _dispose_state = value; }
            get { return _dispose_state; }
        }

        /// <summary>
        ///
        /// </summary>
        public DateTime? create_time
        {
            set { _create_time = value; }
            get { return _create_time; }
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
        public string? andon_code
        {
            set { _andon_code = value; }
            get { return _andon_code; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? andon_description
        {
            set { _andon_description = value; }
            get { return _andon_description; }
        }

        /// <summary>
        ///
        /// </summary>
        public decimal? duration
        {
            set { _duration = value; }
            get { return _duration; }
        }
        #endregion Model
    }
}
