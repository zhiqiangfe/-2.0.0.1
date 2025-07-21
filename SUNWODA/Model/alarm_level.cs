using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// alarm_level:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class alarm_level
    {
        public alarm_level() { }

        #region Model
        private int _id;
        private string? _level_name;
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
        public string? level_name
        {
            set { _level_name = value; }
            get { return _level_name; }
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
