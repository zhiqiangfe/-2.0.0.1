using HandyControl.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace SUNWODA_SEVB.Component.Behaviors
{
    /// <summary>
    /// 自动滚动行为
    /// </summary>
    public class AutoScrollBehavior : Behavior<ScrollViewer>
    {
        private bool _autoScroll = true;
        private bool _userInteracting = false;

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.ScrollChanged += OnScrollChanged;
            AssociatedObject.PreviewMouseDown += OnPreviewMouseDown;
            AssociatedObject.PreviewMouseWheel += OnPreviewMouseWheel;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.ScrollChanged -= OnScrollChanged;
            AssociatedObject.PreviewMouseDown -= OnPreviewMouseDown;
            AssociatedObject.PreviewMouseWheel -= OnPreviewMouseWheel;
            base.OnDetaching();
        }

        private void OnScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (!_userInteracting && e.ExtentHeightChange > 0 && _autoScroll)
            {
                AssociatedObject.ScrollToBottom();
            }
        }

        private void OnPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _userInteracting = true;
            _autoScroll = false;
        }

        private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            _userInteracting = true;

            // 如果用户滚动到底部，重新启用自动滚动
            var scrollViewer = (ScrollViewer)sender;
            if (scrollViewer.VerticalOffset >= scrollViewer.ScrollableHeight - 10)
            {
                _autoScroll = true;
            }
            else
            {
                _autoScroll = false;
            }

            _userInteracting = false;
        }
    }

    /// <summary>
    /// 消息选择行为
    /// </summary>
    public class MessageSelectionBehavior : Behavior<FrameworkElement>
    {
        public static readonly DependencyProperty IsSelectedProperty =
            DependencyProperty.RegisterAttached("IsSelected", typeof(bool),
                typeof(MessageSelectionBehavior), new PropertyMetadata(false, OnIsSelectedChanged));

        public static bool GetIsSelected(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedProperty);
        }

        public static void SetIsSelected(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectedProperty, value);
        }

        private static void OnIsSelectedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is FrameworkElement element && (bool)e.NewValue)
            {
                // 添加选中效果
                ApplySelectionEffect(element, true);
            }
            else if (d is FrameworkElement el)
            {
                // 移除选中效果
                ApplySelectionEffect(el, false);
            }
        }

        private static void ApplySelectionEffect(FrameworkElement element, bool isSelected)
        {
            // 根据元素类型应用不同的选中效果
            switch (element)
            {
                case Control control:
                    control.Background = isSelected
                        ? new SolidColorBrush(Color.FromArgb(30, 0, 120, 215))
                        : Brushes.Transparent;
                    break;

                case Panel panel:
                    panel.Background = isSelected
                        ? new SolidColorBrush(Color.FromArgb(30, 0, 120, 215))
                        : Brushes.Transparent;
                    break;

                case Border border:
                    border.Background = isSelected
                        ? new SolidColorBrush(Color.FromArgb(30, 0, 120, 215))
                        : Brushes.Transparent;
                    break;

                default:
                    // 对于其他类型的元素，尝试使用装饰器或其他视觉效果
                    if (isSelected)
                    {
                        // 可以添加边框或其他视觉提示
                        element.Opacity = 0.9;
                    }
                    else
                    {
                        element.Opacity = 1.0;
                    }
                    break;
            }
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.MouseLeftButtonUp += OnMouseLeftButtonUp;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.MouseLeftButtonUp -= OnMouseLeftButtonUp;
            base.OnDetaching();
        }

        private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            var currentValue = GetIsSelected(AssociatedObject);
            SetIsSelected(AssociatedObject, !currentValue);
        }
    }
}
