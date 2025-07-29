using Microsoft.Web.WebView2.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SUNWODA_SEVB.Component.UserControls
{
    /// <summary>
    /// WebBrowserControl.xaml 的交互逻辑
    /// </summary>
    public partial class WebBrowserControl : UserControl
    {
        #region 依赖属性

        // Source属性
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register(
                nameof(Source),
                typeof(string),
                typeof(WebBrowserControl),
                new PropertyMetadata(string.Empty, OnSourceChanged));

        public string Source
        {
            get => (string)GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        // ShowNavigationBar属性
        public static readonly DependencyProperty ShowNavigationBarProperty =
            DependencyProperty.Register(
                nameof(ShowNavigationBar),
                typeof(bool),
                typeof(WebBrowserControl),
                new PropertyMetadata(false, OnShowNavigationBarChanged));

        public bool ShowNavigationBar
        {
            get => (bool)GetValue(ShowNavigationBarProperty);
            set => SetValue(ShowNavigationBarProperty, value);
        }

        // IsLoading属性
        public static readonly DependencyProperty IsLoadingProperty =
            DependencyProperty.Register(
                nameof(IsLoading),
                typeof(bool),
                typeof(WebBrowserControl),
                new PropertyMetadata(false));

        public bool IsLoading
        {
            get => (bool)GetValue(IsLoadingProperty);
            private set => SetValue(IsLoadingProperty, value);
        }

        #endregion

        #region 事件

        public event EventHandler<CoreWebView2NavigationCompletedEventArgs>? NavigationCompleted;
        public event EventHandler<CoreWebView2NavigationStartingEventArgs>? NavigationStarting;
        public event EventHandler<CoreWebView2NewWindowRequestedEventArgs>? NewWindowRequested;

        #endregion

        #region 属性

        private bool _isInitialized;

        public CoreWebView2? CoreWebView2 => webView?.CoreWebView2;

        #endregion

        #region 构造函数

        public WebBrowserControl()
        {
            InitializeComponent();
            InitializeAsync();
        }

        #endregion

        #region 初始化

        private async void InitializeAsync()
        {
            try
            {
                LoadingPanel.Visibility = Visibility.Visible;

                // 配置WebView2环境（可选）
                var env = await CoreWebView2Environment.CreateAsync(
                    browserExecutableFolder: null,
                    userDataFolder: System.IO.Path.Combine(
                        System.IO.Path.GetTempPath(),
                        "WebView2UserData"));

                await webView.EnsureCoreWebView2Async(env);

                SetupWebView();

                _isInitialized = true;
                LoadingPanel.Visibility = Visibility.Collapsed;

                // 如果已设置Source，则导航
                if (!string.IsNullOrEmpty(Source))
                {
                    Navigate(Source);
                }
            }
            catch (Exception)
            {
                //MessageBox.Show($"WebView2初始化失败：{ex.Message}", "错误",
                //    MessageBoxButton.OK, MessageBoxImage.Error);
                LoadingPanel.Visibility = Visibility.Collapsed;
            }
        }

        private void SetupWebView()
        {
            // 订阅事件
            webView.NavigationStarting += OnNavigationStarting;
            webView.NavigationCompleted += OnNavigationCompleted;
            webView.CoreWebView2.NewWindowRequested += OnNewWindowRequested;

            // 配置设置
            var settings = webView.CoreWebView2.Settings;
            settings.IsScriptEnabled = true;
            settings.IsWebMessageEnabled = true;
            settings.AreDefaultScriptDialogsEnabled = true;
            settings.IsZoomControlEnabled = true;
        }

        #endregion

        #region 导航方法

        public void Navigate(string url)
        {
            if (!_isInitialized || string.IsNullOrEmpty(url))
                return;

            try
            {
                // 自动添加协议
                if (!url.StartsWith("http://") && !url.StartsWith("https://") && !url.StartsWith("file://"))
                {
                    url = "https://" + url;
                }

                webView.CoreWebView2.Navigate(url);

                if (ShowNavigationBar)
                {
                    AddressBar.Text = url;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导航失败：{ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void NavigateToString(string html)
        {
            if (!_isInitialized)
                return;

            webView.NavigateToString(html);
        }

        public void GoBack()
        {
            if (_isInitialized && webView.CanGoBack)
            {
                webView.GoBack();
            }
        }

        public void GoForward()
        {
            if (_isInitialized && webView.CanGoForward)
            {
                webView.GoForward();
            }
        }

        public void Refresh()
        {
            if (_isInitialized)
            {
                webView.Reload();
            }
        }

        public void Stop()
        {
            if (_isInitialized)
            {
                webView.Stop();
            }
        }

        #endregion

        #region 事件处理

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (WebBrowserControl)d;
            var newValue = (string)e.NewValue;

            if (control._isInitialized && !string.IsNullOrEmpty(newValue))
            {
                control.Navigate(newValue);
            }
        }

        private static void OnShowNavigationBarChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (WebBrowserControl)d;
            control.NavigationBar.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        private void OnNavigationStarting(object? sender, CoreWebView2NavigationStartingEventArgs e)
        {
            IsLoading = true;
            NavigationStarting?.Invoke(this, e);

            UpdateNavigationButtons();
        }

        private void OnNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
        {
            IsLoading = false;
            NavigationCompleted?.Invoke(this, e);

            UpdateNavigationButtons();

            if (ShowNavigationBar)
            {
                AddressBar.Text = webView.Source?.ToString() ?? string.Empty;
            }
        }

        private void OnNewWindowRequested(object? sender, CoreWebView2NewWindowRequestedEventArgs e)
        {
            // 默认在当前窗口打开
            e.Handled = true;
            Navigate(e.Uri);

            NewWindowRequested?.Invoke(this, e);
        }

        private void UpdateNavigationButtons()
        {
            if (ShowNavigationBar && _isInitialized)
            {
                BackButton.IsEnabled = webView.CanGoBack;
                ForwardButton.IsEnabled = webView.CanGoForward;
            }
        }

        #endregion

        #region 导航栏事件

        private void BackButton_Click(object sender, RoutedEventArgs e) => GoBack();
        private void ForwardButton_Click(object sender, RoutedEventArgs e) => GoForward();
        private void RefreshButton_Click(object sender, RoutedEventArgs e) => Refresh();
        //private void GoButton_Click(object sender, RoutedEventArgs e) => Navigate(AddressBar.Text);

        private void AddressBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Navigate(AddressBar.Text);
            }
        }

        #endregion

        #region 公共属性

        public bool CanGoBack => _isInitialized && webView.CanGoBack;
        public bool CanGoForward => _isInitialized && webView.CanGoForward;

        #endregion

        #region IDisposable

        public void Dispose()
        {
            if (webView != null)
            {
                webView.Dispose();
            }
        }

        #endregion
    }
}
