using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using SUNWODA_SEVB.Component.UserControls;

namespace SUNWODA_SEVB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 订阅双击事件
            modelViewer.ModelDoubleClicked += OnModelDoubleClicked;

            // 可以程序化加载模型
            // modelViewer.LoadModel(@"C:\Models\example.obj", "示例模型");
        }

        //private void OpenImage_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog
        //    {
        //        Filter = "图片文件|*.jpg;*.jpeg;*.png;*.bmp;*.gif;*.tiff|所有文件|*.*",
        //    };

        //    if (openFileDialog.ShowDialog() == true)
        //    {
        //        imageViewer.LoadImage(openFileDialog.FileName);
        //    }
        //}

        //private void SaveAnnotatedImage_Click(object sender, RoutedEventArgs e)
        //{
        //    SaveFileDialog saveFileDialog = new SaveFileDialog
        //    {
        //        Filter = "PNG图片|*.png|JPEG图片|*.jpg|BMP图片|*.bmp",
        //    };

        //    if (saveFileDialog.ShowDialog() == true)
        //    {
        //        imageViewer.SaveImageWithAnnotations(saveFileDialog.FileName);
        //    }
        //}

        //private void Exit_Click(object sender, RoutedEventArgs e)
        //{
        //    Application.Current.Shutdown();
        //}

        private void OnModelDoubleClicked(object? sender, ModelDoubleClickEventArgs e)
        {
            // 处理模型双击事件
            Console.WriteLine($"双击了模型: {e.ModelInfo.Name}");

            // 可以显示自定义对话框或执行其他操作
        }
    }
}
