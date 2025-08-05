using SUNWODA_SEVB.Core.Models.PLC;
using System.Windows;
using System.Windows.Controls;

namespace SUNWODA_SEVB.Component.UserControls
{
    /// <summary>
    /// ConnectInfoPanel.xaml 的交互逻辑
    /// </summary>
    public partial class ConnectInfoPanel : UserControl
    {
        /// <summary>
        /// 连接信息集合
        /// </summary>
        public IList<ConnectInfo> ConnectInfoList
        {
            get { return (IList<ConnectInfo>)GetValue(ConnectInfoListProperty); }
            set { SetValue(ConnectInfoListProperty, value); }
        }

        public static readonly DependencyProperty ConnectInfoListProperty = DependencyProperty.Register(
            "ConnectInfoList",
            typeof(IList<ConnectInfo>),
            typeof(ConnectInfoPanel),
            new PropertyMetadata(null)
        );

        public ConnectInfoPanel()
        {
            InitializeComponent();
        }
    }
}
