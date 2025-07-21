using System;

namespace SUNWODA_SEVB.Data.Model
{
    /// <summary>
    /// device_childeqcode:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class device_childeqcode
    {
        public device_childeqcode() { }

        #region Model
        private string? _child_equipment_id;
        private string? _child_equipment_address;
        private string? _remark;

        /// <summary>
        ///
        /// </summary>
        public string? child_equipment_id
        {
            set { _child_equipment_id = value; }
            get { return _child_equipment_id; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? child_equipment_address
        {
            set { _child_equipment_address = value; }
            get { return _child_equipment_address; }
        }

        /// <summary>
        ///
        /// </summary>
        public string? Remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model
    }
}
