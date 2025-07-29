using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using OpenCvSharp;
using OpenCvSharp.WpfExtensions;
using Point = System.Windows.Point;
using Size = System.Windows.Size;
using Rect = System.Windows.Rect;

namespace SUNWODA_SEVB.Component.UserControls
{
    /// <summary>
    /// ImageViewerControl.xaml 的交互逻辑
    /// </summary>
    public partial class ImageViewerControl : UserControl
    {
        private Mat? _originalImage;
        private double _zoomLevel = 1.0;
        private Point _lastMousePosition;
        private bool _isPanning;
        private bool _isAnnotating;
        private Point _annotationStartPoint;
        private List<AnnotationItem> _annotations = new List<AnnotationItem>();
        private DockingPosition _dockingPosition = DockingPosition.Center;

        public ImageViewerControl()
        {
            InitializeComponent();
        }

        #region Properties

        public Mat? Image
        {
            get => _originalImage;
            set
            {
                _originalImage = value;
                DisplayImage();
                UpdateImageInfo();
            }
        }

        public double ZoomLevel
        {
            get => _zoomLevel;
            set
            {
                _zoomLevel = Math.Max(0.1, Math.Min(10, value));
                DisplayImage();
                UpdateZoomInfo();
            }
        }

        #endregion

        #region Image Display Methods

        private void DisplayImage()
        {
            if (_originalImage == null || _originalImage.Empty())
            {
                imageDisplay.Source = null;
                return;
            }

            // Convert Mat to BitmapSource
            var bitmap = BitmapSourceConverter.ToBitmapSource(_originalImage);
            imageDisplay.Source = bitmap;

            // Apply zoom
            imageDisplay.Width = _originalImage.Width * _zoomLevel;
            imageDisplay.Height = _originalImage.Height * _zoomLevel;

            // Apply docking
            ApplyDocking();

            // Update annotation canvas size
            annotationCanvas.Width = imageDisplay.Width;
            annotationCanvas.Height = imageDisplay.Height;

            // 更新所有标注的位置和大小
            UpdateAnnotations();
        }

        private void UpdateAnnotations()
        {
            foreach (var annotation in _annotations)
            {
                // 根据原始图片坐标计算当前显示坐标
                double x = annotation.ImageBounds.X * _zoomLevel;
                double y = annotation.ImageBounds.Y * _zoomLevel;
                double width = annotation.ImageBounds.Width * _zoomLevel;
                double height = annotation.ImageBounds.Height * _zoomLevel;

                if (annotation.Visual is Rectangle rect)
                {
                    Canvas.SetLeft(rect, x);
                    Canvas.SetTop(rect, y);
                    rect.Width = width;
                    rect.Height = height;
                    rect.StrokeThickness = annotation.OriginalStrokeThickness * _zoomLevel;
                }
                else if (annotation.Visual is Ellipse ellipse)
                {
                    Canvas.SetLeft(ellipse, x);
                    Canvas.SetTop(ellipse, y);
                    ellipse.Width = width;
                    ellipse.Height = height;
                    ellipse.StrokeThickness = annotation.OriginalStrokeThickness * _zoomLevel;
                }
                else if (annotation.Visual is Border border)
                {
                    Canvas.SetLeft(border, x);
                    Canvas.SetTop(border, y);

                    if (border.Child is TextBlock textBlock)
                    {
                        textBlock.FontSize = annotation.OriginalFontSize * _zoomLevel;
                    }

                    border.BorderThickness = new Thickness(annotation.OriginalStrokeThickness * _zoomLevel);
                }
            }
        }

        private void ApplyDocking()
        {
            if (imageDisplay?.Source == null) return;

            // Reset alignment
            imageContainer.HorizontalAlignment = HorizontalAlignment.Center;
            imageContainer.VerticalAlignment = VerticalAlignment.Center;

            switch (_dockingPosition)
            {
                case DockingPosition.Top:
                    imageContainer.VerticalAlignment = VerticalAlignment.Top;
                    break;
                case DockingPosition.Bottom:
                    imageContainer.VerticalAlignment = VerticalAlignment.Bottom;
                    break;
                case DockingPosition.Left:
                    imageContainer.HorizontalAlignment = HorizontalAlignment.Left;
                    break;
                case DockingPosition.Right:
                    imageContainer.HorizontalAlignment = HorizontalAlignment.Right;
                    break;
                case DockingPosition.Center:
                    // Default is center
                    break;
            }
        }

        private void FitImageToWindow()
        {
            if (_originalImage == null || _originalImage.Empty()) return;

            double windowWidth = scrollViewer.ActualWidth;
            double windowHeight = scrollViewer.ActualHeight;

            double imageWidth = _originalImage.Width;
            double imageHeight = _originalImage.Height;

            double scaleX = windowWidth / imageWidth;
            double scaleY = windowHeight / imageHeight;

            //ZoomLevel = Math.Min(scaleX, scaleY) * 0.95; // 95% to leave some margin
            ZoomLevel = Math.Min(scaleX, scaleY) * 1.0; // 100%
        }

        #endregion

        #region Event Handlers

        private void BtnZoomIn_Click(object sender, RoutedEventArgs e)
        {
            ZoomLevel *= 1.2;
        }

        private void BtnZoomOut_Click(object sender, RoutedEventArgs e)
        {
            ZoomLevel /= 1.2;
        }

        private void BtnFitWindow_Click(object sender, RoutedEventArgs e)
        {
            FitImageToWindow();
        }

        private void BtnReset_Click(object sender, RoutedEventArgs e)
        {
            ZoomLevel = 1.0;
            scrollViewer.ScrollToHorizontalOffset(0);
            scrollViewer.ScrollToVerticalOffset(0);
        }

        private void CmbDocking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbDocking.SelectedIndex < 0) return;

            _dockingPosition = (DockingPosition)cmbDocking.SelectedIndex;
            ApplyDocking();
        }

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                e.Handled = true;

                double scaleFactor = e.Delta > 0 ? 1.1 : 0.9;
                Point mousePosition = e.GetPosition(imageDisplay);

                // Calculate new zoom level
                double newZoom = _zoomLevel * scaleFactor;
                newZoom = Math.Max(0.1, Math.Min(10, newZoom));

                // Calculate offset to keep mouse position stable
                double offsetX = mousePosition.X * (scaleFactor - 1);
                double offsetY = mousePosition.Y * (scaleFactor - 1);

                ZoomLevel = newZoom;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset + offsetX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset + offsetY);
            }
        }

        private void ImageContainer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (btnAnnotateText.IsChecked == true ||
                btnAnnotateRect.IsChecked == true ||
                btnAnnotateCircle.IsChecked == true)
            {
                _isAnnotating = true;
                _annotationStartPoint = e.GetPosition(annotationCanvas);
            }
            else
            {
                _isPanning = true;
                _lastMousePosition = e.GetPosition(scrollViewer);
                imageContainer.CaptureMouse();
            }
        }

        private void ImageContainer_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isAnnotating)
            {
                Point endPoint = e.GetPosition(annotationCanvas);
                CreateAnnotation(_annotationStartPoint, endPoint);
                _isAnnotating = false;
            }
            else if (_isPanning)
            {
                _isPanning = false;
                imageContainer.ReleaseMouseCapture();
            }
        }

        private void ImageContainer_MouseMove(object sender, MouseEventArgs e)
        {
            // Update mouse position
            Point mousePos = e.GetPosition(imageDisplay);
            if (_originalImage != null && !_originalImage.Empty())
            {
                int x = (int)(mousePos.X / _zoomLevel);
                int y = (int)(mousePos.Y / _zoomLevel);

                if (x >= 0 && x < _originalImage.Width && y >= 0 && y < _originalImage.Height)
                {
                    txtMousePosition.Text = $"X: {x}, Y: {y}";
                }
            }

            // Handle panning
            if (_isPanning && e.LeftButton == MouseButtonState.Pressed)
            {
                Point currentPosition = e.GetPosition(scrollViewer);
                double deltaX = currentPosition.X - _lastMousePosition.X;
                double deltaY = currentPosition.Y - _lastMousePosition.Y;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - deltaX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - deltaY);

                _lastMousePosition = currentPosition;
            }
        }

        private void ImageContainer_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Context menu for annotations
            Point clickPoint = e.GetPosition(annotationCanvas);

            // Check if click is on an annotation
            foreach (var annotation in _annotations)
            {
                if (annotation.HitTest(clickPoint))
                {
                    // Show context menu for annotation
                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem deleteItem = new MenuItem { Header = "删除标注" };
                    deleteItem.Click += (s, args) =>
                    {
                        annotationCanvas.Children.Remove(annotation.Visual);
                        _annotations.Remove(annotation);
                    };
                    contextMenu.Items.Add(deleteItem);

                    if (annotation.Type == AnnotationType.Text)
                    {
                        MenuItem editItem = new MenuItem { Header = "编辑文字" };
                        editItem.Click += (s, args) =>
                        {
                            // Edit text annotation
                            EditTextAnnotation(annotation);
                        };
                        contextMenu.Items.Add(editItem);
                    }

                    contextMenu.IsOpen = true;
                    break;
                }
            }
        }

        private void BtnClearAnnotations_Click(object sender, RoutedEventArgs e)
        {
            annotationCanvas.Children.Clear();
            _annotations.Clear();
        }

        #endregion

        #region Annotation Methods

        private void CreateAnnotation(Point start, Point end)
        {
            UIElement? visual = null;
            AnnotationType type = AnnotationType.Rectangle;

            // 转换为相对于原始图片的坐标
            Point imageStart = new Point(start.X / _zoomLevel, start.Y / _zoomLevel);
            Point imageEnd = new Point(end.X / _zoomLevel, end.Y / _zoomLevel);

            if (btnAnnotateRect.IsChecked == true)
            {
                visual = CreateRectangleAnnotation(start, end);
                type = AnnotationType.Rectangle;
            }
            else if (btnAnnotateCircle.IsChecked == true)
            {
                visual = CreateCircleAnnotation(start, end);
                type = AnnotationType.Circle;
            }
            else if (btnAnnotateText.IsChecked == true)
            {
                visual = CreateTextAnnotation(start);
                type = AnnotationType.Text;
            }

            if (visual != null)
            {
                annotationCanvas.Children.Add(visual);
                _annotations.Add(new AnnotationItem
                {
                    Visual = visual,
                    Type = type,
                    Bounds = new Rect(start, end),
                    ImageBounds = new Rect(imageStart, imageEnd) // 保存相对坐标
                });
            }
        }

        private Rectangle CreateRectangleAnnotation(Point start, Point end)
        {
            Rectangle rect = new Rectangle
            {
                Stroke = Brushes.Red,
                StrokeThickness = 2 * _zoomLevel, // 根据缩放调整线条粗细
                Fill = new SolidColorBrush(Color.FromArgb(50, 255, 0, 0))
            };

            double x = Math.Min(start.X, end.X);
            double y = Math.Min(start.Y, end.Y);
            double width = Math.Abs(end.X - start.X);
            double height = Math.Abs(end.Y - start.Y);

            Canvas.SetLeft(rect, x);
            Canvas.SetTop(rect, y);
            rect.Width = width;
            rect.Height = height;

            return rect;
        }

        private Ellipse CreateCircleAnnotation(Point start, Point end)
        {
            Ellipse ellipse = new Ellipse
            {
                Stroke = Brushes.Blue,
                StrokeThickness = 2 * _zoomLevel, // 根据缩放调整线条粗细
                Fill = new SolidColorBrush(Color.FromArgb(50, 0, 0, 255))
            };

            double x = Math.Min(start.X, end.X);
            double y = Math.Min(start.Y, end.Y);
            double width = Math.Abs(end.X - start.X);
            double height = Math.Abs(end.Y - start.Y);

            Canvas.SetLeft(ellipse, x);
            Canvas.SetTop(ellipse, y);
            ellipse.Width = width;
            ellipse.Height = height;

            return ellipse;
        }

        private UIElement CreateTextAnnotation(Point position)
        {
            TextBox textBox = new TextBox
            {
                Text = "标注文字",
                Background = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255)),
                BorderBrush = Brushes.Green,
                BorderThickness = new Thickness(2 * _zoomLevel),
                Padding = new Thickness(5),
                MinWidth = 100,
                FontSize = 14 * _zoomLevel,
                AcceptsReturn = false // 不允许多行
            };

            Canvas.SetLeft(textBox, position.X);
            Canvas.SetTop(textBox, position.Y);

            // Auto select text for immediate editing
            textBox.Focus();
            textBox.SelectAll();

            // 创建完成编辑的处理方法
            Action completeEdit = () =>
            {
                if (string.IsNullOrWhiteSpace(textBox.Text))
                {
                    annotationCanvas.Children.Remove(textBox);
                    var annotation = _annotations.Find(a => a.Visual == textBox);
                    if (annotation != null)
                    {
                        _annotations.Remove(annotation);
                    }
                    return;
                }

                TextBlock textBlock = new TextBlock
                {
                    Text = textBox.Text,
                    Background = new SolidColorBrush(Color.FromArgb(200, 255, 255, 255)),
                    Padding = new Thickness(5),
                    FontSize = 14 * _zoomLevel
                };

                Border border = new Border
                {
                    BorderBrush = Brushes.Green,
                    BorderThickness = new Thickness(2 * _zoomLevel),
                    Child = textBlock
                };

                Canvas.SetLeft(border, Canvas.GetLeft(textBox));
                Canvas.SetTop(border, Canvas.GetTop(textBox));

                int index = annotationCanvas.Children.IndexOf(textBox);
                if (index >= 0)
                {
                    annotationCanvas.Children.RemoveAt(index);
                    annotationCanvas.Children.Insert(index, border);
                }

                // Update annotation reference
                var existingAnnotation = _annotations.Find(a => a.Visual == textBox);
                if (existingAnnotation != null)
                {
                    existingAnnotation.Visual = border;
                    existingAnnotation.Text = textBox.Text;
                    existingAnnotation.ImageBounds = new Rect(
                        Canvas.GetLeft(border) / _zoomLevel,
                        Canvas.GetTop(border) / _zoomLevel,
                        border.ActualWidth / _zoomLevel,
                        border.ActualHeight / _zoomLevel
                    );
                    existingAnnotation.OriginalFontSize = 14;
                    existingAnnotation.OriginalStrokeThickness = 2;
                }
            };

            // 标记是否已经完成编辑，避免重复处理
            bool editCompleted = false;

            // 处理回车键
            textBox.KeyDown += (s, e) =>
            {
                if (e.Key == Key.Enter && !editCompleted)
                {
                    editCompleted = true;
                    completeEdit();
                    e.Handled = true;
                }
                else if (e.Key == Key.Escape)
                {
                    editCompleted = true;
                    textBox.Text = "";
                    completeEdit();
                    e.Handled = true;
                }
            };

            // 处理失去焦点（点击其他地方）
            textBox.LostFocus += (s, e) =>
            {
                if (!editCompleted)
                {
                    editCompleted = true;
                    completeEdit();
                }
            };

            return textBox;
        }

        private void EditTextAnnotation(AnnotationItem annotation)
        {
            if (annotation.Visual is Border border && border.Child is TextBlock textBlock)
            {
                TextBox textBox = new TextBox
                {
                    Text = textBlock.Text,
                    Background = textBlock.Background,
                    Padding = textBlock.Padding,
                    FontSize = textBlock.FontSize,
                    MinWidth = 100,
                    AcceptsReturn = false // 不允许多行
                };

                Canvas.SetLeft(textBox, Canvas.GetLeft(border));
                Canvas.SetTop(textBox, Canvas.GetTop(border));

                int index = annotationCanvas.Children.IndexOf(border);
                annotationCanvas.Children.RemoveAt(index);
                annotationCanvas.Children.Insert(index, textBox);

                textBox.Focus();
                textBox.SelectAll();

                // 创建完成编辑的处理方法
                Action completeEdit = () =>
                {
                    if (string.IsNullOrWhiteSpace(textBox.Text))
                    {
                        annotationCanvas.Children.Remove(textBox);
                        _annotations.Remove(annotation);
                        return;
                    }

                    textBlock.Text = textBox.Text;
                    annotation.Text = textBox.Text;

                    int currentIndex = annotationCanvas.Children.IndexOf(textBox);
                    if (currentIndex >= 0)
                    {
                        annotationCanvas.Children.RemoveAt(currentIndex);
                        annotationCanvas.Children.Insert(currentIndex, border);
                    }

                    // 更新相对坐标
                    annotation.ImageBounds = new Rect(
                        Canvas.GetLeft(border) / _zoomLevel,
                        Canvas.GetTop(border) / _zoomLevel,
                        border.ActualWidth / _zoomLevel,
                        border.ActualHeight / _zoomLevel
                    );
                };

                // 标记是否已经完成编辑
                bool editCompleted = false;

                // 处理回车键
                textBox.KeyDown += (s, e) =>
                {
                    if (e.Key == Key.Enter && !editCompleted)
                    {
                        editCompleted = true;
                        completeEdit();
                        e.Handled = true;
                    }
                    else if (e.Key == Key.Escape)
                    {
                        editCompleted = true;
                        // 恢复原始文本
                        textBox.Text = annotation.Text ?? "";
                        completeEdit();
                        e.Handled = true;
                    }
                };

                // 处理失去焦点
                textBox.LostFocus += (s, e) =>
                {
                    if (!editCompleted)
                    {
                        editCompleted = true;
                        completeEdit();
                    }
                };
            }
        }

        #endregion

        #region Helper Methods

        private void UpdateImageInfo()
        {
            if (_originalImage == null || _originalImage.Empty())
            {
                txtImageInfo.Text = "无图片";
                return;
            }

            string info = $"尺寸: {_originalImage.Width}x{_originalImage.Height} | " +
                         $"通道: {_originalImage.Channels()} | " +
                         $"类型: {_originalImage.Type()} | " +
                         $"深度: {_originalImage.Depth()}";

            txtImageInfo.Text = info;
        }

        private void UpdateZoomInfo()
        {
            txtZoomLevel.Text = $"缩放: {_zoomLevel:P0}";
        }

        #endregion

        #region Public Methods

        public void LoadImage(string filePath)
        {
            try
            {
                Mat image = Cv2.ImRead(filePath, ImreadModes.Unchanged);
                if (!image.Empty())
                {
                    Image = image;
                    FitImageToWindow();
                }
                else
                {
                    MessageBox.Show("无法加载图片文件", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载图片时出错: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void SaveImageWithAnnotations(string filePath)
        {
            if (_originalImage == null || _originalImage.Empty()) return;

            // Create a copy of the original image
            Mat output = _originalImage.Clone();

            // Draw annotations on the image
            foreach (var annotation in _annotations)
            {
                DrawAnnotationOnMat(output, annotation);
            }

            // Save the image
            Cv2.ImWrite(filePath, output);
            output.Dispose();
        }

        private void DrawAnnotationOnMat(Mat image, AnnotationItem annotation)
        {
            // Convert WPF coordinates to image coordinates
            double scaleX = (double)image.Width / imageDisplay.Width;
            double scaleY = (double)image.Height / imageDisplay.Height;

            if (annotation.Visual is Rectangle rect)
            {
                int x = (int)(Canvas.GetLeft(rect) * scaleX);
                int y = (int)(Canvas.GetTop(rect) * scaleY);
                int width = (int)(rect.Width * scaleX);
                int height = (int)(rect.Height * scaleY);

                Cv2.Rectangle(image, new OpenCvSharp.Rect(x, y, width, height),
                    new Scalar(0, 0, 255), 2);
            }
            else if (annotation.Visual is Ellipse ellipse)
            {
                int x = (int)((Canvas.GetLeft(ellipse) + ellipse.Width / 2) * scaleX);
                int y = (int)((Canvas.GetTop(ellipse) + ellipse.Height / 2) * scaleY);
                int radiusX = (int)(ellipse.Width / 2 * scaleX);
                int radiusY = (int)(ellipse.Height / 2 * scaleY);

                Cv2.Ellipse(image, new OpenCvSharp.Point(x, y),
                    new OpenCvSharp.Size(radiusX, radiusY),
                    0, 0, 360, new Scalar(255, 0, 0), 2);
            }
            else if (annotation.Visual is Border border && border.Child is TextBlock textBlock)
            {
                int x = (int)(Canvas.GetLeft(border) * scaleX);
                int y = (int)((Canvas.GetTop(border) + textBlock.ActualHeight) * scaleY);

                Cv2.PutText(image, annotation.Text ?? "", new OpenCvSharp.Point(x, y),
                    HersheyFonts.HersheySimplex, 0.8, new Scalar(0, 255, 0), 2);
            }
        }

        #endregion
    }

    #region Helper Classes

    public enum DockingPosition
    {
        Center,
        Top,
        Bottom,
        Left,
        Right
    }

    public enum AnnotationType
    {
        Rectangle,
        Circle,
        Text
    }

    public class AnnotationItem
    {
        public UIElement? Visual { get; set; }
        public AnnotationType Type { get; set; }
        public Rect Bounds { get; set; }
        public string? Text { get; set; }

        // 添加相对于原始图片的坐标信息
        public Rect ImageBounds { get; set; } // 相对于原始图片的坐标
        public double OriginalFontSize { get; set; } = 14; // 原始字体大小
        public double OriginalStrokeThickness { get; set; } = 2; // 原始线条粗细

        public bool HitTest(Point point)
        {
            if (Visual is Rectangle rect)
            {
                double x = Canvas.GetLeft(rect);
                double y = Canvas.GetTop(rect);
                return point.X >= x && point.X <= x + rect.Width &&
                       point.Y >= y && point.Y <= y + rect.Height;
            }
            else if (Visual is Ellipse ellipse)
            {
                double x = Canvas.GetLeft(ellipse);
                double y = Canvas.GetTop(ellipse);
                return point.X >= x && point.X <= x + ellipse.Width &&
                       point.Y >= y && point.Y <= y + ellipse.Height;
            }
            else if (Visual is Border border)
            {
                double x = Canvas.GetLeft(border);
                double y = Canvas.GetTop(border);
                return point.X >= x && point.X <= x + border.ActualWidth &&
                       point.Y >= y && point.Y <= y + border.ActualHeight;
            }

            return false;
        }
    }

    #endregion
}
