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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SUNWODA_SEVB.Component.CustomControls
{
    /// <summary>
    /// 简单的思考指示器
    /// </summary>
    public class ThinkingIndicator : ContentControl
    {
        static ThinkingIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ThinkingIndicator),
                new FrameworkPropertyMetadata(typeof(ThinkingIndicator)));
        }

        /// <summary>
        /// 显示的文本
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register(
                nameof(Text),
                typeof(string),
                typeof(ThinkingIndicator),
                new PropertyMetadata("AI正在思考..."));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        /// <summary>
        /// 是否正在思考
        /// </summary>
        public static readonly DependencyProperty IsThinkingProperty =
            DependencyProperty.Register(
                nameof(IsThinking),
                typeof(bool),
                typeof(ThinkingIndicator),
                new PropertyMetadata(true));

        public bool IsThinking
        {
            get => (bool)GetValue(IsThinkingProperty);
            set => SetValue(IsThinkingProperty, value);
        }
    }
}
