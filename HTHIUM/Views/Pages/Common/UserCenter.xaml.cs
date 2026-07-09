using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace HTHIUM.Views.Pages.Common
{
    /// <summary>
    /// UserCenter.xaml 的交互逻辑
    /// </summary>
    public partial class UserCenter : Page
    {
        public UserCenter()
        {
            InitializeComponent();
            AssistantImage.Source = LoginAssistantImageAssets.Idle;
        }

        private void PasswordBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SetAssistantPeeking(true);
        }

        private void PasswordBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            SetAssistantPeeking(false);
        }

        private void SetAssistantPeeking(bool isPeeking)
        {
            AssistantStatusText.Text = isPeeking
                ? "正在输入密码，智能助手已为您遮挡"
                : "准备登录，智能助手正在守护安全";
            AssistantHintText.Text = isPeeking ? "保证安全，放心输入" : "请输入账号与密码";

            AssistantImage.Source = isPeeking
                ? LoginAssistantImageAssets.Cover
                : LoginAssistantImageAssets.Idle;

            Animate(AssistantImageScale, ScaleTransform.ScaleXProperty, isPeeking ? 1.05 : 1);
            Animate(AssistantImageScale, ScaleTransform.ScaleYProperty, isPeeking ? 1.05 : 1);
            Animate(AssistantImage, UIElement.OpacityProperty, isPeeking ? 0.98 : 1);
        }

        private static void Animate(Animatable target, DependencyProperty property, double value)
        {
            var animation = new DoubleAnimation
            {
                To = value,
                Duration = TimeSpan.FromMilliseconds(260),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            target.BeginAnimation(property, animation);
        }

        private static void Animate(UIElement target, DependencyProperty property, double value)
        {
            var animation = new DoubleAnimation
            {
                To = value,
                Duration = TimeSpan.FromMilliseconds(220),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            target.BeginAnimation(property, animation);
        }
    }
}
