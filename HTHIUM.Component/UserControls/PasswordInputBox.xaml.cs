using System.Windows;
using System.Windows.Controls;
using HandyControl.Controls;

namespace HTHIUM.Component.UserControls
{
    /// <summary>
    /// PasswordInputBox.xaml 的交互逻辑
    /// </summary>
    public partial class PasswordInputBox : UserControl
    {
        #region 字段和属性
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(
            "Password",
            typeof(string),
            typeof(PasswordInputBox),
            new FrameworkPropertyMetadata(
                "",
                FrameworkPropertyMetadataOptions.Inherits,
                new PropertyChangedCallback(OnPasswordChanged)
            )
        );
        public static readonly DependencyProperty WaterMarkProperty = DependencyProperty.Register(
            "WaterMark",
            typeof(string),
            typeof(PasswordInputBox),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.Inherits,
                new PropertyChangedCallback(OnWaterMarkChanged)
            )
        );

        public static readonly DependencyProperty ShowPasswordProperty =
            DependencyProperty.Register(
                "ShowPassword",
                typeof(bool),
                typeof(PasswordInputBox),
                new FrameworkPropertyMetadata(
                    false,
                    FrameworkPropertyMetadataOptions.Inherits,
                    new PropertyChangedCallback(OnShowPasswordChanged)
                )
            );

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }

        public string WaterMark
        {
            get { return (string)GetValue(WaterMarkProperty); }
            set { SetValue(WaterMarkProperty, value); }
        }

        public bool ShowPassword
        {
            get { return (bool)GetValue(ShowPasswordProperty); }
            set { SetValue(ShowPasswordProperty, value); }
        }
        #endregion

        #region 构造方法
        public PasswordInputBox()
        {
            InitializeComponent();
        }
        #endregion

        #region 方法
        private static void OnPasswordChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var passwordInputBox = d as PasswordInputBox;
            if (passwordInputBox is not null)
            {
                if (passwordInputBox.passwordBox.Password != null)
                {
                    if (string.IsNullOrEmpty(e.NewValue.ToString()))
                        passwordInputBox.passwordBox.Clear();
                    passwordInputBox.passwordBox.Password = e.NewValue.ToString();
                }
            }
        }

        private static void OnWaterMarkChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var passwordInputBox = d as PasswordInputBox;
            if (passwordInputBox is not null)
                passwordInputBox.SetValue(InfoElement.PlaceholderProperty, e.NewValue);
        }

        private static void OnShowPasswordChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var passwordInputBox = d as PasswordInputBox;
            if (passwordInputBox is not null)
            {
                if (passwordInputBox.passwordBox != null)
                {
                    passwordInputBox.passwordBox.ShowPassword = (bool)e.NewValue;
                }
            }
        }

        private void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as HandyControl.Controls.PasswordBox;
            if (passwordBox != null)
            {
                Password = passwordBox.Password;
            }
        }
        #endregion

        private void passwordBox_Click(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as HandyControl.Controls.PasswordBox;
            if (passwordBox != null)
            {
                ShowPassword = passwordBox.ShowPassword;
            }
        }
    }
}
