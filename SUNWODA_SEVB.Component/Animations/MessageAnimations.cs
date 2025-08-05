using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Windows;

namespace SUNWODA_SEVB.Component.Animations
{
    /// <summary>
    /// 消息动画帮助类
    /// </summary>
    public static class MessageAnimations
    {
        /// <summary>
        /// 创建淡入动画
        /// </summary>
        public static Storyboard CreateFadeInAnimation(double duration = 0.3)
        {
            var storyboard = new Storyboard();

            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(duration),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(fadeIn);

            return storyboard;
        }

        /// <summary>
        /// 创建滑入动画
        /// </summary>
        public static Storyboard CreateSlideInAnimation(bool fromRight = false, double duration = 0.3)
        {
            var storyboard = new Storyboard();

            var slideIn = new DoubleAnimation
            {
                From = fromRight ? 50 : -50,
                To = 0,
                Duration = TimeSpan.FromSeconds(duration),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            Storyboard.SetTargetProperty(slideIn,
                new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            storyboard.Children.Add(slideIn);

            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(duration),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            Storyboard.SetTargetProperty(fadeIn, new PropertyPath(UIElement.OpacityProperty));
            storyboard.Children.Add(fadeIn);

            return storyboard;
        }

        /// <summary>
        /// 创建打字机效果动画
        /// </summary>
        public static void ApplyTypewriterEffect(TextBlock textBlock, string text, int speed = 20)
        {
            if (textBlock == null || string.IsNullOrEmpty(text))
                return;

            textBlock.Text = "";

            var animation = new StringAnimationUsingKeyFrames
            {
                Duration = new Duration(TimeSpan.FromMilliseconds(text.Length * speed))
            };

            for (int i = 0; i <= text.Length; i++)
            {
                animation.KeyFrames.Add(new DiscreteStringKeyFrame
                {
                    Value = text.Substring(0, i),
                    KeyTime = KeyTime.FromTimeSpan(TimeSpan.FromMilliseconds(i * speed))
                });
            }

            textBlock.BeginAnimation(TextBlock.TextProperty, animation);
        }

        /// <summary>
        /// 创建脉冲动画（用于加载指示器）
        /// </summary>
        public static Storyboard CreatePulseAnimation()
        {
            var storyboard = new Storyboard
            {
                RepeatBehavior = RepeatBehavior.Forever
            };

            var scaleX = new DoubleAnimation
            {
                From = 1,
                To = 1.2,
                Duration = TimeSpan.FromSeconds(0.6),
                AutoReverse = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            var scaleY = new DoubleAnimation
            {
                From = 1,
                To = 1.2,
                Duration = TimeSpan.FromSeconds(0.6),
                AutoReverse = true,
                EasingFunction = new SineEase { EasingMode = EasingMode.EaseInOut }
            };

            Storyboard.SetTargetProperty(scaleX,
                new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleX)"));
            Storyboard.SetTargetProperty(scaleY,
                new PropertyPath("(UIElement.RenderTransform).(ScaleTransform.ScaleY)"));

            storyboard.Children.Add(scaleX);
            storyboard.Children.Add(scaleY);

            return storyboard;
        }
    }

    /// <summary>
    /// 打字机效果附加属性
    /// </summary>
    public static class TypewriterEffect
    {
        public static readonly DependencyProperty IsEnabledProperty =
            DependencyProperty.RegisterAttached("IsEnabled", typeof(bool),
                typeof(TypewriterEffect), new PropertyMetadata(false, OnIsEnabledChanged));

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.RegisterAttached("Text", typeof(string),
                typeof(TypewriterEffect), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty SpeedProperty =
            DependencyProperty.RegisterAttached("Speed", typeof(int),
                typeof(TypewriterEffect), new PropertyMetadata(20));

        public static bool GetIsEnabled(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsEnabledProperty);
        }

        public static void SetIsEnabled(DependencyObject obj, bool value)
        {
            obj.SetValue(IsEnabledProperty, value);
        }

        public static string GetText(DependencyObject obj)
        {
            return (string)obj.GetValue(TextProperty);
        }

        public static void SetText(DependencyObject obj, string value)
        {
            obj.SetValue(TextProperty, value);
        }

        public static int GetSpeed(DependencyObject obj)
        {
            return (int)obj.GetValue(SpeedProperty);
        }

        public static void SetSpeed(DependencyObject obj, int value)
        {
            obj.SetValue(SpeedProperty, value);
        }

        private static void OnIsEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock && (bool)e.NewValue)
            {
                var text = GetText(textBlock);
                var speed = GetSpeed(textBlock);

                if (string.IsNullOrEmpty(text))
                    text = textBlock.Text;

                MessageAnimations.ApplyTypewriterEffect(textBlock, text, speed);
            }
        }
    }
}
