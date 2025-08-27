using HandyControl.Controls;
using SUNWODA_SEVB.ViewModels.Windows.Common;
//using System.Windows;
using System.Windows.Controls.Primitives;

namespace SUNWODA_SEVB.Views.Windows.Common
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (NavigationBar.Visibility == System.Windows.Visibility.Visible)
            {
                NavigationColumnDefinition.MinWidth = 200;
            }
        }
    }
}
