using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using HelixToolkit.Wpf;
using SUNWODA_SEVB.Core.Models.Component;
using Range = SUNWODA_SEVB.Core.Models.Component.Range;

namespace SUNWODA_SEVB.Component.UserControls
{
    /// <summary>
    /// TrajectoryViewer3D.xaml 的交互逻辑
    /// </summary>
    public partial class TrajectoryViewer3D : UserControl
    {
        private SortingVisual3D? sortingVisual3D;
        private readonly Dictionary<string, TrajectoryVisual3D> trajectories = new();
        private DispatcherTimer animationTimer;
        private bool isInitialzed;
        private readonly object legendLockObj = new object();

        public TrajectoryViewer3D()
        {
            InitializeComponent();
            // 初始化动画计时器
            animationTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(50) };
            animationTimer.Tick += OnAnimationTick;

            sortingVisual3D = new SortingVisual3D();
            sortingVisual3D.IsSorting = true;
            sortingVisual3D.SortingFrequency = 30;
            sortingVisual3D.Method = SortingMethod.BoundingBoxCorners;
            sortingVisual3D.CheckForOpaqueVisuals = true;
            viewport.Children.Add(sortingVisual3D);
            // 添加视口的鼠标事件处理
            viewport.MouseDown += OnViewportMouseDown;

            Loaded += TrajectoryViewer3D_Loaded;
        }

        private void TrajectoryViewer3D_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isInitialzed)
            {
                // 添加网格
                AddCoordinateGrids();
                // 添加轴标签
                AddAxisLabels();
                // 添加刻度和刻度标签
                AddAxisTicks();
                // 添加阈值范围可视框
                AddThresholdRangeVisual();
                // 设置相机位置视角
                SetCameraPosition(CameraPosition, CameraLookDirection, CameraUpDirection);
                // 设置标题和标签字体大小和显示
                SetTitleAndLegend();
                isInitialzed = true;
            }
        }

        private void AddCoordinateGrids()
        {
            var lineColor = Color.FromArgb(60, 100, 100, 100);

            for (double i = ZAxisRange.Start; i < ZAxisRange.End; i += ZAxisStep)
            {
                // 平行于X轴的线
                var xLine = new LinesVisual3D
                {
                    Points = new Point3DCollection
                    {
                        new Point3D(XAxisRange.Start, YAxisRange.Start, i),
                        new Point3D(XAxisRange.End, YAxisRange.Start, i),
                    },
                    Color = lineColor,
                    Thickness = GridThickness,
                };
                viewport.Children.Add(xLine);

                // 平行于Y轴的线
                var yLine = new LinesVisual3D
                {
                    Points = new Point3DCollection
                    {
                        new Point3D(XAxisRange.Start, YAxisRange.Start, i),
                        new Point3D(XAxisRange.Start, YAxisRange.End, i),
                    },
                    Color = lineColor,
                    Thickness = GridThickness,
                };
                viewport.Children.Add(yLine);
            }

            var edgeZXLine = new LinesVisual3D
            {
                Points = new Point3DCollection
                {
                    new Point3D(XAxisRange.Start, YAxisRange.Start, ZAxisRange.End),
                    new Point3D(XAxisRange.End, YAxisRange.Start, ZAxisRange.End),
                },
                Color = lineColor,
                Thickness = GridThickness,
            };
            viewport.Children.Add(edgeZXLine);

            var edgeZYLine = new LinesVisual3D
            {
                Points = new Point3DCollection
                {
                    new Point3D(XAxisRange.Start, YAxisRange.Start, ZAxisRange.End),
                    new Point3D(XAxisRange.Start, YAxisRange.End, ZAxisRange.End),
                },
                Color = lineColor,
                Thickness = GridThickness,
            };
            viewport.Children.Add(edgeZYLine);

            for (double i = YAxisRange.Start; i < YAxisRange.End; i += YAxisStep)
            {
                // 平行于X轴的线
                var xLine = new LinesVisual3D
                {
                    Points = new Point3DCollection
                    {
                        new Point3D(XAxisRange.Start, i, ZAxisRange.Start),
                        new Point3D(XAxisRange.End, i, ZAxisRange.Start),
                    },
                    Color = lineColor,
                    Thickness = GridThickness,
                };
                viewport.Children.Add(xLine);

                // 平行于Z轴的线
                var zLine = new LinesVisual3D
                {
                    Points = new Point3DCollection
                    {
                        new Point3D(XAxisRange.Start, i, ZAxisRange.Start),
                        new Point3D(XAxisRange.Start, i, ZAxisRange.End),
                    },
                    Color = lineColor,
                    Thickness = GridThickness,
                };
                viewport.Children.Add(zLine);
            }

            var edgeYXLine = new LinesVisual3D
            {
                Points = new Point3DCollection
                {
                    new Point3D(XAxisRange.Start, YAxisRange.End, ZAxisRange.Start),
                    new Point3D(XAxisRange.End, YAxisRange.End, ZAxisRange.Start),
                },
                Color = lineColor,
                Thickness = GridThickness * 10,
            };
            viewport.Children.Add(edgeYXLine);

            var edgeYZLine = new LinesVisual3D
            {
                Points = new Point3DCollection
                {
                    new Point3D(XAxisRange.Start, YAxisRange.End, ZAxisRange.Start),
                    new Point3D(XAxisRange.Start, YAxisRange.End, ZAxisRange.End),
                },
                Color = lineColor,
                Thickness = GridThickness,
            };
            viewport.Children.Add(edgeYZLine);

            for (double i = XAxisRange.Start; i < XAxisRange.End; i += XAxisStep)
            {
                // 平行于Y轴的线
                var yLine = new LinesVisual3D
                {
                    Points = new Point3DCollection
                    {
                        new Point3D(i, YAxisRange.Start, ZAxisRange.Start),
                        new Point3D(i, YAxisRange.End, ZAxisRange.Start),
                    },
                    Color = lineColor,
                    Thickness = GridThickness,
                };
                viewport.Children.Add(yLine);

                // 平行于Z轴的线
                var zLine = new LinesVisual3D
                {
                    Points = new Point3DCollection
                    {
                        new Point3D(i, YAxisRange.Start, ZAxisRange.Start),
                        new Point3D(i, YAxisRange.Start, ZAxisRange.End),
                    },
                    Color = lineColor,
                    Thickness = GridThickness,
                };
                viewport.Children.Add(zLine);
            }

            var edgeXYLine = new LinesVisual3D
            {
                Points = new Point3DCollection
                {
                    new Point3D(XAxisRange.End, YAxisRange.Start, ZAxisRange.Start),
                    new Point3D(XAxisRange.End, YAxisRange.End, ZAxisRange.Start),
                },
                Color = lineColor,
                Thickness = GridThickness * 10,
            };
            viewport.Children.Add(edgeXYLine);

            var edgeXZLine = new LinesVisual3D
            {
                Points = new Point3DCollection
                {
                    new Point3D(XAxisRange.End, YAxisRange.Start, ZAxisRange.Start),
                    new Point3D(XAxisRange.End, YAxisRange.Start, ZAxisRange.End),
                },
                Color = lineColor,
                Thickness = GridThickness * 10,
            };
            viewport.Children.Add(edgeXZLine);
        }

        private void AddAxisLabels()
        {
            // 添加轴标签
            var xLabel = new BillboardTextVisual3D
            {
                Position = new Point3D(
                    XAxisRange.Start + (XAxisRange.End - XAxisRange.Start) / 2,
                    YAxisRange.End + 4 * TickLength,
                    ZAxisRange.Start
                ),
                Text = XLabel,
                FontSize = AxisLabelFontSize,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Red,
            };
            viewport.Children.Add(xLabel);

            var yLabel = new BillboardTextVisual3D
            {
                Position = new Point3D(
                    XAxisRange.End + 4 * TickLength,
                    YAxisRange.Start + (YAxisRange.End - YAxisRange.Start) / 2,
                    ZAxisRange.Start
                ),
                Text = YLabel,
                FontSize = AxisLabelFontSize,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Green,
            };
            viewport.Children.Add(yLabel);

            var zLabel = new BillboardTextVisual3D
            {
                Position = new Point3D(
                    XAxisRange.End + 4 * TickLength,
                    YAxisRange.Start,
                    ZAxisRange.Start + (ZAxisRange.End - ZAxisRange.Start) / 2
                ),
                Text = ZLabel,
                FontSize = AxisLabelFontSize,
                FontWeight = FontWeights.Bold,
                Foreground = Brushes.Blue,
            };
            viewport.Children.Add(zLabel);
        }

        private void AddAxisTicks()
        {
            // Z轴刻度
            for (double i = ZAxisRange.Start; i <= ZAxisRange.End; i += ZAxisStep)
            {
                // 刻度线
                var tick = new LinesVisual3D
                {
                    Points = new Point3DCollection
                    {
                        new Point3D(XAxisRange.End, YAxisRange.Start, i),
                        new Point3D(XAxisRange.End, YAxisRange.Start - TickLength, i),
                    },
                    Color = Colors.DarkGray,
                    Thickness = GridThickness * 10,
                };
                viewport.Children.Add(tick);

                // 刻度数值
                var label = new BillboardTextVisual3D
                {
                    Position = new Point3D(XAxisRange.End, YAxisRange.Start - 2 * TickLength, i),
                    Text = i.ToString(),
                    FontSize = AxisLabelFontSize * 0.8,
                    Foreground = Brushes.DarkGray,
                };
                viewport.Children.Add(label);
            }

            // Y轴刻度
            for (double i = YAxisRange.Start; i <= YAxisRange.End; i += YAxisStep)
            {
                var tick = new LinesVisual3D
                {
                    Points = new Point3DCollection
                    {
                        new Point3D(XAxisRange.End, i, ZAxisRange.Start),
                        new Point3D(XAxisRange.End + TickLength, i, ZAxisRange.Start),
                    },
                    Color = Colors.DarkGray,
                    Thickness = GridThickness * 10,
                };
                viewport.Children.Add(tick);

                var label = new BillboardTextVisual3D
                {
                    Position = new Point3D(XAxisRange.End + 2 * TickLength, i, ZAxisRange.Start),
                    Text = i.ToString(),
                    FontSize = AxisLabelFontSize * 0.8,
                    Foreground = Brushes.DarkGray,
                };
                viewport.Children.Add(label);
            }

            // X轴刻度
            for (double i = XAxisRange.Start; i <= XAxisRange.End; i += XAxisStep)
            {
                var tick = new LinesVisual3D
                {
                    Points = new Point3DCollection
                    {
                        new Point3D(i, YAxisRange.End, ZAxisRange.Start),
                        new Point3D(i, YAxisRange.End + TickLength, ZAxisRange.Start),
                    },
                    Color = Colors.DarkGray,
                    Thickness = GridThickness * 10,
                };
                viewport.Children.Add(tick);

                var label = new BillboardTextVisual3D
                {
                    Position = new Point3D(i, YAxisRange.End + 2 * TickLength, ZAxisRange.Start),
                    Text = i.ToString(),
                    FontSize = AxisLabelFontSize * 0.8,
                    Foreground = Brushes.DarkGray,
                };
                viewport.Children.Add(label);
            }
        }

        private void AddThresholdRangeVisual()
        {
            if (
                ThresholdBoxHalfLength == 0
                || ThresholdBoxHalfWidth == 0
                || ThresholdBoxHalfHeight == 0
            )
                return;

            // 创建6个独立的矩形面，每个面都会独立排序，这样才能形成透明效果
            var material = new DiffuseMaterial(
                new SolidColorBrush(Color.FromArgb(80, 100, 100, 100))
            );

            // 创建6个面
            // 顶面 (Z+)
            AddRectangleFace(
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                material
            );

            // 底面 (Z-)
            AddRectangleFace(
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                material
            );

            // 后面 (X-)
            AddRectangleFace(
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                material
            );

            // 前面 (X+)
            AddRectangleFace(
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                material
            );

            // 左面 (Y-)
            AddRectangleFace(
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y - ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                material
            );

            // 右面 (Y+)
            AddRectangleFace(
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X - ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z + ThresholdBoxHalfHeight),
                new Point3D(ThresholdBoxCenter.X + ThresholdBoxHalfLength, ThresholdBoxCenter.Y + ThresholdBoxHalfWidth, ThresholdBoxCenter.Z - ThresholdBoxHalfHeight),
                material
            );
        }

        private void AddRectangleFace(
            Point3D p1,
            Point3D p2,
            Point3D p3,
            Point3D p4,
            Material material
        )
        {
            var mesh = new MeshBuilder();
            mesh.AddQuad(p1, p2, p3, p4);

            var geometryModel = new GeometryModel3D
            {
                Geometry = mesh.ToMesh(),
                Material = material,
                BackMaterial = material,
            };

            var modelVisual = new ModelVisual3D { Content = geometryModel };
            modelVisual.SetValue(RenderOptions.EdgeModeProperty, EdgeMode.Unspecified);

            sortingVisual3D?.Children.Add(modelVisual);
        }

        private void SetTitleAndLegend()
        {
            Title.FontSize = AxisLabelFontSize * 1.2;
            Title.Text = ChartTitle;
            Title.FontWeight = FontWeights.Bold;
            
            LegendThresholdBoxLabel.FontSize = AxisLabelFontSize * 0.8;
            LegendThresholdBoxLabel.FontWeight = FontWeights.Bold;
            LegendThresholdWarnLineLabel.FontSize = AxisLabelFontSize * 0.8;
            LegendThresholdWarnLineLabel.FontWeight = FontWeights.Bold;
            if (ThresholdBoxHalfLength == 0 || ThresholdBoxHalfWidth == 0 || ThresholdBoxHalfHeight == 0)
            {
                LegendThresholdBox.Visibility = Visibility.Collapsed;
                LegendThresholdWarnLine.Visibility = Visibility.Collapsed;
            }
            else
            {
                LegendThresholdBox.Visibility = Visibility.Visible;
                LegendThresholdWarnLine.Visibility = Visibility.Visible;
            }

            LegendLines.Children.Clear();
            if (TrajectoriesSource != null)
            {
                foreach (var trajectory in TrajectoriesSource)
                {
                    var legendStackPanel = new StackPanel() { Margin = new Thickness(0, 10, 0, 0), Orientation = Orientation.Horizontal, VerticalAlignment = VerticalAlignment.Top };
                    var legendIcon = new Viewport3D()
                    {
                        Width = 100,
                        Height = 10,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Camera = new PerspectiveCamera(new Point3D(0, 0, 5), new Vector3D(0, 0, -5), new Vector3D(0, 1, 0), 60)
                    };
                    legendIcon.Children.Add(new ModelVisual3D() { Content = new DirectionalLight(Colors.White, new Vector3D(-0.2, -0.3, -1)) });
                    var positions = new Point3DCollection
                {
                    new Point3D(-1, 0, -1),
                    new Point3D(1, 0, -1),
                    new Point3D(1, 0.2, -1),
                    new Point3D(-1, 0.2, -1)
                };
                    legendIcon.Children.Add(new ModelVisual3D() { Content = new GeometryModel3D(new MeshGeometry3D() { Positions = positions, TriangleIndices = new Int32Collection([0, 1, 2, 0, 2, 3]) }, new DiffuseMaterial() { Brush = new SolidColorBrush(trajectory.Color) }) });
                    legendStackPanel.Children.Add(legendIcon);

                    legendStackPanel.Children.Add(new TextBlock() { Text = trajectory.Name, FontSize = AxisLabelFontSize * 0.8, FontWeight = FontWeights.Bold });

                    LegendLines.Children.Add(legendStackPanel);
                }
            }
            Legend.Visibility = IsShowLegend ? Visibility.Visible : Visibility.Collapsed;
        }

        #region 依赖属性

        public static readonly DependencyProperty TrajectoriesSourceProperty =
            DependencyProperty.Register(
                nameof(TrajectoriesSource),
                typeof(ObservableCollection<Trajectory3D>),
                typeof(TrajectoryViewer3D),
                new PropertyMetadata(null, OnTrajectoriesSourceChanged)
            );

        public ObservableCollection<Trajectory3D> TrajectoriesSource
        {
            get => (ObservableCollection<Trajectory3D>)GetValue(TrajectoriesSourceProperty);
            set => SetValue(TrajectoriesSourceProperty, value);
        }

        public static readonly DependencyProperty SelectedTrajectoryProperty =
            DependencyProperty.Register(
                nameof(SelectedTrajectory),
                typeof(Trajectory3D),
                typeof(TrajectoryViewer3D),
                new PropertyMetadata(null)
            );

        public Trajectory3D SelectedTrajectory
        {
            get => (Trajectory3D)GetValue(SelectedTrajectoryProperty);
            set => SetValue(SelectedTrajectoryProperty, value);
        }

        public static readonly DependencyProperty TrajectoryClickCommandProperty =
            DependencyProperty.Register(
                nameof(TrajectoryClickCommand),
                typeof(ICommand),
                typeof(TrajectoryViewer3D)
            );

        public ICommand TrajectoryClickCommand
        {
            get => (ICommand)GetValue(TrajectoryClickCommandProperty);
            set => SetValue(TrajectoryClickCommandProperty, value);
        }

        public Range XAxisRange
        {
            get { return (Range)GetValue(XAxisRangeProperty); }
            set { SetValue(XAxisRangeProperty, value); }
        }

        public static readonly DependencyProperty XAxisRangeProperty = DependencyProperty.Register(
            "XAxisRange",
            typeof(Range),
            typeof(TrajectoryViewer3D),
            new PropertyMetadata(new Range(-25, 25))
        );

        public Range YAxisRange
        {
            get { return (Range)GetValue(YAxisRangeProperty); }
            set { SetValue(YAxisRangeProperty, value); }
        }

        public static readonly DependencyProperty YAxisRangeProperty = DependencyProperty.Register(
            "YAxisRange",
            typeof(Range),
            typeof(TrajectoryViewer3D),
            new PropertyMetadata(new Range(-25, 25))
        );

        public Range ZAxisRange
        {
            get { return (Range)GetValue(ZAxisRangeProperty); }
            set { SetValue(ZAxisRangeProperty, value); }
        }

        public static readonly DependencyProperty ZAxisRangeProperty = DependencyProperty.Register(
            "ZAxisRange",
            typeof(Range),
            typeof(TrajectoryViewer3D),
            new PropertyMetadata(new Range(0, 50))
        );

        public double XAxisStep
        {
            get { return (double)GetValue(XAxisStepProperty); }
            set { SetValue(XAxisStepProperty, value); }
        }

        public static readonly DependencyProperty XAxisStepProperty = DependencyProperty.Register(
            "XAxisStep",
            typeof(double),
            typeof(TrajectoryViewer3D),
            new PropertyMetadata(10.0)
        );

        public double YAxisStep
        {
            get { return (double)GetValue(YAxisStepProperty); }
            set { SetValue(YAxisStepProperty, value); }
        }

        public static readonly DependencyProperty YAxisStepProperty = DependencyProperty.Register(
            "YAxisStep",
            typeof(double),
            typeof(TrajectoryViewer3D),
            new PropertyMetadata(10.0)
        );

        public double ZAxisStep
        {
            get { return (double)GetValue(ZAxisStepProperty); }
            set { SetValue(ZAxisStepProperty, value); }
        }

        public static readonly DependencyProperty ZAxisStepProperty = DependencyProperty.Register(
            "ZAxisStep",
            typeof(double),
            typeof(TrajectoryViewer3D),
            new PropertyMetadata(10.0)
        );

        public string XLabel
        {
            get { return (string)GetValue(XLabelProperty); }
            set { SetValue(XLabelProperty, value); }
        }

        public static readonly DependencyProperty XLabelProperty =
            DependencyProperty.Register("XLabel", typeof(string), typeof(TrajectoryViewer3D), new PropertyMetadata("X"));

        public string YLabel
        {
            get { return (string)GetValue(YLabelProperty); }
            set { SetValue(YLabelProperty, value); }
        }

        public static readonly DependencyProperty YLabelProperty =
            DependencyProperty.Register("YLabel", typeof(string), typeof(TrajectoryViewer3D), new PropertyMetadata("Y"));

        public string ZLabel
        {
            get { return (string)GetValue(ZLabelProperty); }
            set { SetValue(ZLabelProperty, value); }
        }

        public static readonly DependencyProperty ZLabelProperty =
            DependencyProperty.Register("ZLabel", typeof(string), typeof(TrajectoryViewer3D), new PropertyMetadata("Z"));

        public double AxisLabelFontSize
        {
            get { return (double)GetValue(AxisLabelFontSizeProperty); }
            set { SetValue(AxisLabelFontSizeProperty, value); }
        }

        public static readonly DependencyProperty AxisLabelFontSizeProperty =
            DependencyProperty.Register("AxisLabelFontSize", typeof(double), typeof(TrajectoryViewer3D), new PropertyMetadata(14.0));

        public double GridThickness
        {
            get { return (double)GetValue(GridThicknessProperty); }
            set { SetValue(GridThicknessProperty, value); }
        }

        public static readonly DependencyProperty GridThicknessProperty =
            DependencyProperty.Register(
                "GridThickness",
                typeof(double),
                typeof(TrajectoryViewer3D),
                new PropertyMetadata(0.2)
            );

        public double TickLength
        {
            get { return (double)GetValue(TickLengthProperty); }
            set { SetValue(TickLengthProperty, value); }
        }

        public static readonly DependencyProperty TickLengthProperty = DependencyProperty.Register(
            "TickLength",
            typeof(double),
            typeof(TrajectoryViewer3D),
            new PropertyMetadata(2.0)
        );

        public double ThresholdBoxHalfLength
        {
            get { return (double)GetValue(ThresholdBoxHalfLengthProperty); }
            set { SetValue(ThresholdBoxHalfLengthProperty, value); }
        }

        public static readonly DependencyProperty ThresholdBoxHalfLengthProperty =
            DependencyProperty.Register(
                "ThresholdBoxHalfLength",
                typeof(double),
                typeof(TrajectoryViewer3D),
                new PropertyMetadata(0.0, OnThresholdBoxChanged)
            );

        public double ThresholdBoxHalfWidth
        {
            get { return (double)GetValue(ThresholdBoxHalfWidthProperty); }
            set { SetValue(ThresholdBoxHalfWidthProperty, value); }
        }

        public static readonly DependencyProperty ThresholdBoxHalfWidthProperty =
            DependencyProperty.Register(
                "ThresholdBoxHalfWidth",
                typeof(double),
                typeof(TrajectoryViewer3D),
                new PropertyMetadata(0.0, OnThresholdBoxChanged)
            );

        public double ThresholdBoxHalfHeight
        {
            get { return (double)GetValue(ThresholdBoxHalfHeightProperty); }
            set { SetValue(ThresholdBoxHalfHeightProperty, value); }
        }

        public static readonly DependencyProperty ThresholdBoxHalfHeightProperty =
            DependencyProperty.Register(
                "ThresholdBoxHalfHeight",
                typeof(double),
                typeof(TrajectoryViewer3D),
                new PropertyMetadata(0.0, OnThresholdBoxChanged)
            );

        public Point3D ThresholdBoxCenter
        {
            get { return (Point3D)GetValue(ThresholdBoxCenterProperty); }
            set { SetValue(ThresholdBoxCenterProperty, value); }
        }

        public static readonly DependencyProperty ThresholdBoxCenterProperty =
            DependencyProperty.Register(
                "ThresholdBoxCenter",
                typeof(Point3D),
                typeof(TrajectoryViewer3D),
                new PropertyMetadata(new Point3D(0, 0, 0), OnThresholdBoxChanged)
            );

        public Point3D CameraPosition
        {
            get { return (Point3D)GetValue(CameraPositionProperty); }
            set { SetValue(CameraPositionProperty, value); }
        }

        public static readonly DependencyProperty CameraPositionProperty =
            DependencyProperty.Register("CameraPosition", typeof(Point3D), typeof(TrajectoryViewer3D), new PropertyMetadata(new Point3D(110, 130, 70)));

        public Vector3D CameraLookDirection
        {
            get { return (Vector3D)GetValue(CameraLookDirectionProperty); }
            set { SetValue(CameraLookDirectionProperty, value); }
        }

        public static readonly DependencyProperty CameraLookDirectionProperty =
            DependencyProperty.Register("CameraLookDirection", typeof(Vector3D), typeof(TrajectoryViewer3D), new PropertyMetadata(new Vector3D(-110, -130, -45)));

        public Vector3D CameraUpDirection
        {
            get { return (Vector3D)GetValue(CameraUpDirectionProperty); }
            set { SetValue(CameraUpDirectionProperty, value); }
        }

        public static readonly DependencyProperty CameraUpDirectionProperty =
            DependencyProperty.Register("CameraUpDirection", typeof(Vector3D), typeof(TrajectoryViewer3D), new PropertyMetadata(new Vector3D(0.38, 0.46, 0.81)));

        public string ChartTitle
        {
            get { return (string)GetValue(ChartTitleProperty); }
            set { SetValue(ChartTitleProperty, value); }
        }

        public static readonly DependencyProperty ChartTitleProperty =
            DependencyProperty.Register("ChartTitle", typeof(string), typeof(TrajectoryViewer3D), new PropertyMetadata(""));

        public bool IsShowLegend
        {
            get { return (bool)GetValue(IsShowLegendProperty); }
            set { SetValue(IsShowLegendProperty, value); }
        }

        public static readonly DependencyProperty IsShowLegendProperty =
            DependencyProperty.Register("IsShowLegend", typeof(bool), typeof(TrajectoryViewer3D), new PropertyMetadata(false));

        #endregion

        private static void OnTrajectoriesSourceChanged(
            DependencyObject d,
            DependencyPropertyChangedEventArgs e
        )
        {
            var control = (TrajectoryViewer3D)d;
            control.UpdateTrajectories();

            if (e.NewValue is ObservableCollection<Trajectory3D> newCollection)
            {
                newCollection.CollectionChanged += (s, args) => control.UpdateTrajectories();
            }
        }

        // 阈值框参数变化时的回调方法
        private static void OnThresholdBoxChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (TrajectoryViewer3D)d;
            control.UpdateAllTrajectoriesThresholdBox();
        }

        private void UpdateTrajectories()
        {
            if (TrajectoriesSource == null)
                return;

            // 清除现有轨迹
            foreach (var trajectory in trajectories.Values)
            {
                //viewport.Children.Remove(trajectory);
                sortingVisual3D?.Children.Remove(trajectory);
            }
            trajectories.Clear();

            lock (legendLockObj)
            {
                SetTitleAndLegend();
            }

            // 添加新轨迹
            foreach (var trajectory in TrajectoriesSource)
            {
                AddTrajectory(trajectory);
            }
        }

        private void AddTrajectory(Trajectory3D trajectory)
        {
            if (trajectory == null || string.IsNullOrEmpty(trajectory.Id))
                return;

            var visual = new TrajectoryVisual3D(trajectory);

            // 设置阈值框参数
            visual.SetThresholdBox(
                ThresholdBoxCenter,
                ThresholdBoxHalfLength,
                ThresholdBoxHalfWidth,
                ThresholdBoxHalfHeight
            );

            trajectories[trajectory.Id] = visual;
            //viewport.Children.Add(visual);
            sortingVisual3D?.Children.Add(visual);

            // 如果是动态轨迹，监听数据变化
            if (trajectory.IsDynamic)
            {
                trajectory.Points.CollectionChanged += (s, e) =>
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        visual.UpdateGeometry();
                    });
                };
            }
        }

        // 当阈值框参数变化时更新所有轨迹
        private void UpdateAllTrajectoriesThresholdBox()
        {
            foreach (var visual in trajectories.Values)
            {
                visual.SetThresholdBox(
                    ThresholdBoxCenter,
                    ThresholdBoxHalfLength,
                    ThresholdBoxHalfWidth,
                    ThresholdBoxHalfHeight
                );
            }
        }

        // 新的视口鼠标事件处理方法
        private void OnViewportMouseDown(object sender, MouseButtonEventArgs e)
        {
            var viewport3D = sender as HelixViewport3D;
            if (viewport3D == null)
                return;

            // 获取鼠标位置
            var mousePos = e.GetPosition(viewport3D);

            // 执行命中测试
            var hitResult = VisualTreeHelper.HitTest(viewport3D.Viewport, mousePos);
            if (hitResult != null && hitResult is RayMeshGeometry3DHitTestResult rayHitResult)
            {
                // 向上遍历可视树找到 TrajectoryVisual3D
                var visual = rayHitResult.VisualHit;
                while (visual != null)
                {
                    if (visual is TrajectoryVisual3D trajectoryVisual)
                    {
                        SelectedTrajectory = trajectoryVisual.Trajectory;
                        //TrajectoryClickCommand?.Execute(trajectoryVisual.Trajectory);

                        // 可选：高亮选中的轨迹
                        HighlightTrajectory(trajectoryVisual);
                        break;
                    }

                    // 获取父级 - 使用 VisualTreeHelper
                    if (visual is Visual3D visual3D)
                    {
                        var parent = VisualTreeHelper.GetParent(visual3D);
                        visual = parent as Visual3D;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                // 重置所有轨迹的高亮状态
                foreach (var visual in trajectories.Values)
                {
                    visual.SetHighlight(false);
                }
            }
        }

        // 可选：高亮显示选中的轨迹
        private void HighlightTrajectory(TrajectoryVisual3D selectedVisual)
        {
            // 重置所有轨迹的高亮状态
            foreach (var visual in trajectories.Values)
            {
                visual.SetHighlight(false);
            }

            // 高亮选中的轨迹
            selectedVisual.SetHighlight(true);
        }

        #region 动画控制

        public void StartAnimation()
        {
            foreach (var visual in trajectories.Values)
            {
                visual.ResetAnimation();
            }
            animationTimer.Start();
        }

        public void StopAnimation()
        {
            animationTimer.Stop();
        }

        public void PauseAnimation()
        {
            animationTimer.Stop();
        }

        public void ResumeAnimation()
        {
            animationTimer.Start();
        }

        private void OnAnimationTick(object? sender, EventArgs e)
        {
            foreach (var visual in trajectories.Values)
            {
                visual.AnimateStep();
            }
        }

        #endregion

        #region 视图控制

        public void ResetCamera()
        {
            viewport.ResetCamera();
        }

        public void ZoomExtents()
        {
            viewport.ZoomExtents(500);
        }

        public void SetCameraPosition(
            Point3D position,
            Vector3D lookDirection,
            Vector3D upDirection
        )
        {
            viewport.Camera.Position = position;
            viewport.Camera.LookDirection = lookDirection;
            viewport.Camera.UpDirection = upDirection;
        }

        #endregion
    }

    /// <summary>
    /// 轨迹可视化对象
    /// </summary>
    public class TrajectoryVisual3D : ModelVisual3D
    {
        //private TubeVisual3D? tubeVisual;
        private SphereVisual3D? currentPositionSphere;
        private int animationIndex = 0;
        private Color originalColor;
        private const double HIGHLIGHT_FACTOR = 3;
        private readonly List<TubeVisual3D> segmentVisuals = new();

        // 阈值框参数
        private Point3D thresholdBoxCenter;
        private double thresholdBoxHalfLength;
        private double thresholdBoxHalfWidth;
        private double thresholdBoxHalfHeight;

        public Trajectory3D Trajectory { get; }

        public TrajectoryVisual3D(Trajectory3D trajectory)
        {
            Trajectory = trajectory;
            originalColor = trajectory.Color;
            CreateVisual();
        }

        // 新增：设置阈值框参数
        public void SetThresholdBox(Point3D center, double halfLength, double halfWidth, double halfHeight)
        {
            thresholdBoxCenter = center;
            thresholdBoxHalfLength = halfLength;
            thresholdBoxHalfWidth = halfWidth;
            thresholdBoxHalfHeight = halfHeight;
            UpdateGeometry();
        }

        // 检查点是否在阈值框内
        private bool IsPointInThresholdBox(Point3D point)
        {
            if (thresholdBoxHalfLength == 0 || thresholdBoxHalfWidth == 0 || thresholdBoxHalfHeight == 0)
                return true; // 如果没有设置阈值框，认为所有点都在内部

            return Math.Abs(point.X - thresholdBoxCenter.X) <= thresholdBoxHalfLength &&
                   Math.Abs(point.Y - thresholdBoxCenter.Y) <= thresholdBoxHalfWidth &&
                   Math.Abs(point.Z - thresholdBoxCenter.Z) <= thresholdBoxHalfHeight;
        }

        //private void CreateVisual()
        //{
        //    var points = Trajectory.Points.Select(p => p.Position).ToList();
        //    if (points.Count < 2)
        //        return;

        //    // 创建轨迹管道
        //    tubeVisual = new TubeVisual3D
        //    {
        //        Path = new Point3DCollection(points),
        //        Diameter = Trajectory.Thickness,
        //        Fill = new SolidColorBrush(Trajectory.Color),
        //        ThetaDiv = 20,
        //    };
        //    Children.Add(tubeVisual);

        //    // 创建当前位置指示器（用于动画）
        //    if (Trajectory.ShowAnimation)
        //    {
        //        currentPositionSphere = new SphereVisual3D
        //        {
        //            Center = points[0],
        //            Radius = Trajectory.Thickness * 2,
        //            Fill = new SolidColorBrush(Colors.Red),
        //            Visible = false,
        //        };
        //        Children.Add(currentPositionSphere);
        //    }

        //    // 显示关键点
        //    if (Trajectory.ShowKeyPoints)
        //    {
        //        foreach (var point in Trajectory.Points.Where(p => p.IsKeyPoint))
        //        {
        //            var sphere = new SphereVisual3D
        //            {
        //                Center = point.Position,
        //                Radius = Trajectory.Thickness * 1.5,
        //                Fill = new SolidColorBrush(Colors.Yellow),
        //            };
        //            Children.Add(sphere);

        //            // 添加标签
        //            if (!string.IsNullOrEmpty(point.Label))
        //            {
        //                var text = new BillboardTextVisual3D
        //                {
        //                    Position = point.Position,
        //                    Text = point.Label,
        //                    FontSize = 12,
        //                    Foreground = Brushes.White,
        //                    Background = Brushes.Black,
        //                };
        //                Children.Add(text);
        //            }
        //        }
        //    }
        //}

        private void CreateVisual()
        {
            var points = Trajectory.Points.Select(p => p.Position).ToList();
            if (points.Count < 2)
                return;

            // 清除之前的管道段
            segmentVisuals.Clear();

            // 将轨迹分段，根据是否在阈值框内分配不同颜色
            var segments = new List<(List<Point3D> points, bool isInBox)>();
            var currentSegment = new List<Point3D> { points[0] };
            bool currentInBox = IsPointInThresholdBox(points[0]);

            for (int i = 1; i < points.Count; i++)
            {
                bool pointInBox = IsPointInThresholdBox(points[i]);

                if (pointInBox != currentInBox)
                {
                    // 状态改变，需要创建新段
                    // 添加过渡点以确保段连续
                    currentSegment.Add(points[i]);
                    segments.Add((new List<Point3D>(currentSegment), currentInBox));

                    // 开始新段
                    currentSegment = new List<Point3D> { points[i] };
                    currentInBox = pointInBox;
                }
                else
                {
                    currentSegment.Add(points[i]);
                }
            }

            // 添加最后一段
            if (currentSegment.Count > 0)
            {
                segments.Add((currentSegment, currentInBox));
            }

            // 为每个段创建管道
            foreach (var segment in segments)
            {
                if (segment.points.Count < 2)
                    continue;

                var tubeVisual = new TubeVisual3D
                {
                    Path = new Point3DCollection(segment.points),
                    Diameter = Trajectory.Thickness,
                    Fill = new SolidColorBrush(segment.isInBox ? originalColor : Colors.Red),
                    ThetaDiv = 20,
                };
                segmentVisuals.Add(tubeVisual);
                Children.Add(tubeVisual);
            }

            // 创建当前位置指示器（用于动画）
            if (Trajectory.ShowAnimation)
            {
                currentPositionSphere = new SphereVisual3D
                {
                    Center = points[0],
                    Radius = Trajectory.Thickness * 2,
                    Fill = new SolidColorBrush(Colors.Yellow),
                    Visible = false,
                };
                Children.Add(currentPositionSphere);
            }

            // 显示关键点
            if (Trajectory.ShowKeyPoints)
            {
                foreach (var point in Trajectory.Points.Where(p => p.IsKeyPoint))
                {
                    var sphere = new SphereVisual3D
                    {
                        Center = point.Position,
                        Radius = Trajectory.Thickness * 1.5,
                        Fill = new SolidColorBrush(Colors.Yellow),
                    };
                    Children.Add(sphere);

                    // 添加标签
                    if (!string.IsNullOrEmpty(point.Label))
                    {
                        var text = new BillboardTextVisual3D
                        {
                            Position = point.Position,
                            Text = point.Label,
                            FontSize = 12,
                            Foreground = Brushes.White,
                            Background = Brushes.Black,
                        };
                        Children.Add(text);
                    }
                }
            }
        }

        public void UpdateGeometry()
        {
            Children.Clear();
            CreateVisual();
        }

        public void AnimateStep()
        {
            if (!Trajectory.ShowAnimation || currentPositionSphere == null)
                return;

            var points = Trajectory.Points.ToList();
            if (points.Count == 0)
                return;

            currentPositionSphere.Visible = true;
            currentPositionSphere.Center = points[animationIndex % points.Count].Position;
            animationIndex++;
        }

        public void ResetAnimation()
        {
            animationIndex = 0;
            if (currentPositionSphere != null)
            {
                currentPositionSphere.Visible = false;
            }
        }

        //// 设置高亮状态
        //public void SetHighlight(bool isHighlighted)
        //{
        //    if (tubeVisual != null)
        //    {
        //        if (isHighlighted)
        //        {
        //            // 增加亮度或改变颜色来表示高亮
        //            //var highlightColor = Color.FromRgb(
        //            //    (byte)Math.Min(255, originalColor.R * HIGHLIGHT_FACTOR),
        //            //    (byte)Math.Min(255, originalColor.G * HIGHLIGHT_FACTOR),
        //            //    (byte)Math.Min(255, originalColor.B * HIGHLIGHT_FACTOR)
        //            //);
        //            var highlightColor = Color.FromRgb(255, 255, 0);
        //            tubeVisual.Fill = new SolidColorBrush(highlightColor);
        //            tubeVisual.Diameter = Trajectory.Thickness * 1.2; // 稍微增加粗细
        //        }
        //        else
        //        {
        //            // 恢复原始状态
        //            tubeVisual.Fill = new SolidColorBrush(originalColor);
        //            tubeVisual.Diameter = Trajectory.Thickness;
        //        }
        //    }
        //}

        // 设置高亮状态
        public void SetHighlight(bool isHighlighted)
        {
            foreach (var tubeVisual in segmentVisuals)
            {
                if (isHighlighted)
                {
                    // 高亮时保持原有的颜色逻辑，但增加亮度
                    var currentBrush = tubeVisual.Fill as SolidColorBrush;
                    if (currentBrush != null)
                    {
                        var currentColor = currentBrush.Color;
                        var highlightColor = currentColor == Colors.Red ?
                            Colors.OrangeRed : // 红色段高亮为橙红色
                            Color.FromRgb(255, 255, 0); // 原色段高亮为黄色
                        tubeVisual.Fill = new SolidColorBrush(highlightColor);
                    }
                    tubeVisual.Diameter = Trajectory.Thickness * 1.2;
                }
                else
                {
                    // 恢复原始状态，重新判断是否在阈值框内
                    // 这里简化处理，直接重建
                    UpdateGeometry();
                    break;
                }
            }
        }
    }
}
