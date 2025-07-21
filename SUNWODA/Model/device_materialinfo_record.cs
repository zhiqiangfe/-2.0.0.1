using System;
using System.ComponentModel;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_materialinfo_record:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_materialinfo_record
    {
        public device_materialinfo_record() { }

        #region Model
        private int _id;
        private string? _material_code;
        private string? _a_or_b;
        private string? _model;
        private string? _person_code;
        private string? _grade;
        private string? _product_length;
        private string? _speed;
        private string? _weight_cpk;
        private string? _weight_cov;
        private string? _weight_mean;
        private string? _weight_sigma;
        private string? _weight_rate;
        private string? _size_cpk;
        private string? _size_mean;
        private string? _size_sigma;
        private string? _size_rate;
        private DateTime _start_time;
        private DateTime _end_time;
        private string? _use_time;
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
        [Description("膜卷号")]
        public string? material_Code
        {
            set { _material_code = value; }
            get { return _material_code; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("AB面")]
        public string? A_OR_B
        {
            set { _a_or_b = value; }
            get { return _a_or_b; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("产品型号")]
        public string? model
        {
            set { _model = value; }
            get { return _model; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("生产员工")]
        public string? person_code
        {
            set { _person_code = value; }
            get { return _person_code; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("膜卷等级")]
        public string? grade
        {
            set { _grade = value; }
            get { return _grade; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("生产米数")]
        public string? product_length
        {
            set { _product_length = value; }
            get { return _product_length; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("涂布速度")]
        public string? speed
        {
            set { _speed = value; }
            get { return _speed; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("重量CPK")]
        public string? weight_cpk
        {
            set { _weight_cpk = value; }
            get { return _weight_cpk; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("重量COV")]
        public string? weight_cov
        {
            set { _weight_cov = value; }
            get { return _weight_cov; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("重量MEAN")]
        public string? weight_mean
        {
            set { _weight_mean = value; }
            get { return _weight_mean; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("重量sigma")]
        public string? weight_sigma
        {
            set { _weight_sigma = value; }
            get { return _weight_sigma; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("重量优率")]
        public string? weight_rate
        {
            set { _weight_rate = value; }
            get { return _weight_rate; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("尺寸CPK")]
        public string? size_cpk
        {
            set { _size_cpk = value; }
            get { return _size_cpk; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("尺寸MEAN")]
        public string? size_mean
        {
            set { _size_mean = value; }
            get { return _size_mean; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("尺寸sigma")]
        public string? size_sigma
        {
            set { _size_sigma = value; }
            get { return _size_sigma; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("尺寸优率")]
        public string? size_rate
        {
            set { _size_rate = value; }
            get { return _size_rate; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("开始生产时间")]
        public DateTime start_time
        {
            set { _start_time = value; }
            get { return _start_time; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("结束生产时间")]
        public DateTime end_time
        {
            set { _end_time = value; }
            get { return _end_time; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("用时(分钟)")]
        public string? use_time
        {
            set { _use_time = value; }
            get { return _use_time; }
        }

        /// <summary>
        ///
        /// </summary>
        [Description("最终分数")]
        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model
    }
}
