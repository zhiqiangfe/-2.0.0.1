using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SUNWODA_SEVB.Data.Model
{
    [Serializable]
    public partial class device_instation_data
    {
        public device_instation_data() { }

        #region Model
        private int _id;
        private string? _barcode;
        private DateTime _instationtime;
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
        [Description("记录条码")]
        public string? barcode
        {
            set { _barcode = value; }
            get { return _barcode; }
        }

        [Description("进站时间")]
        public DateTime instationtime
        {
            set { _instationtime = value; }
            get { return _instationtime; }
        }

        [Description("备注")]
        public string? remark
        {
            set { _remark = value; }
            get { return _remark; }
        }
        #endregion Model
    }
}
