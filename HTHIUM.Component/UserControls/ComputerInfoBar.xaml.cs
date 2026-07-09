using System.Windows;
using System.Windows.Controls;

namespace HTHIUM.Component.UserControls
{
    /// <summary>
    /// ComputerInfoBar.xaml 的交互逻辑
    /// </summary>
    public partial class ComputerInfoBar : UserControl
    {
        public double CPULoad
        {
            get { return (double)GetValue(CPULoadProperty); }
            set { SetValue(CPULoadProperty, value); }
        }

        public static readonly DependencyProperty CPULoadProperty = DependencyProperty.Register(
            "CPULoad",
            typeof(double),
            typeof(ComputerInfoBar),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnCPULoadChanged))
        );

        public double MemoryLoad
        {
            get { return (double)GetValue(MemoryLoadProperty); }
            set { SetValue(MemoryLoadProperty, value); }
        }

        public static readonly DependencyProperty MemoryLoadProperty = DependencyProperty.Register(
            "MemoryLoad",
            typeof(double),
            typeof(ComputerInfoBar),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnMemoryLoadChanged))
        );

        public double GPULoad
        {
            get { return (double)GetValue(GPULoadProperty); }
            set { SetValue(GPULoadProperty, value); }
        }

        public static readonly DependencyProperty GPULoadProperty = DependencyProperty.Register(
            "GPULoad",
            typeof(double),
            typeof(ComputerInfoBar),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnGPULoadChanged))
        );

        public double IORWRate
        {
            get { return (double)GetValue(IORWRateProperty); }
            set { SetValue(IORWRateProperty, value); }
        }

        public static readonly DependencyProperty IORWRateProperty = DependencyProperty.Register(
            "IORWRate",
            typeof(double),
            typeof(ComputerInfoBar),
            new PropertyMetadata(0.0, new PropertyChangedCallback(OnIORWRateChanged))
        );

        public ComputerInfoBar()
        {
            InitializeComponent();
        }

        private static void OnCPULoadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = d as ComputerInfoBar;
            if (bar != null)
            {
                bar.CPULoadText.Text = $"{bar.CPULoad.ToString("F2")}%";
            }
        }

        private static void OnMemoryLoadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = d as ComputerInfoBar;
            if (bar != null)
            {
                bar.MemoryLoadText.Text = $"{bar.MemoryLoad.ToString("F2")} MB";
            }
        }

        private static void OnGPULoadChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = d as ComputerInfoBar;
            if (bar != null)
            {
                bar.GPULoadText.Text = $"{bar.GPULoad.ToString("F2")}%";
            }
        }

        private static void OnIORWRateChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var bar = d as ComputerInfoBar;
            if (bar != null)
            {
                bar.IORWRateText.Text = $"{bar.IORWRate.ToString("F2")} MB/s";
            }
        }
    }
}
