using HelixToolkit.Wpf;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace SealPinTwinDemo;

public partial class MainWindow : Window
{
    private const double InfeedDoorX = -9.6;
    private const double OutfeedDoorX = 9.6;
    private static readonly double[] StationX = [-9.2, -5.2, 0, 0, 0, 0, 9.2];
    private static readonly BatteryModelSpec CellSpec = BatteryModelSpec.HithiumInfinityCell1175Ah;
    private readonly DispatcherTimer _timer;
    private readonly Stopwatch _simulationClock = Stopwatch.StartNew();
    private readonly Random _random = new(20260727);
    private readonly ModelVisual3D _movingCell = new();
    private readonly ModelVisual3D _movingCell2 = new();
    private readonly ModelVisual3D _shuttle = new();
    private readonly ModelVisual3D _liftTable = new();
    private readonly ModelVisual3D _infeedDoor = new();
    private readonly ModelVisual3D _outfeedDoor = new();
    private readonly BillboardTextVisual3D _cellLabel = new();
    private readonly ModelVisual3D _leftClamp = new();
    private readonly ModelVisual3D _rightClamp = new();
    private readonly ModelVisual3D _cameraHead = new();
    private readonly ModelVisual3D _cleaningHead = new();
    private readonly ModelVisual3D _laserAssembly = new();
    private readonly ModelVisual3D _ccdScanPlane = new();
    private readonly ModelVisual3D _pinTool = new();
    private readonly ModelVisual3D _weldRing = new();
    private readonly ModelVisual3D _weldRing2 = new();
    private readonly ModelVisual3D _hPress = new();
    private readonly ModelVisual3D _gantryCarriage = new();
    private readonly ModelVisual3D _dustHood = new();
    private readonly ModelVisual3D _inspectionHead = new();
    private readonly List<BillboardTextVisual3D> _sceneLabels = [];
    private bool _running = true;
    private bool _fault;
    private bool _followCell = true;
    private bool _updatingSelection;
    private bool _showSceneLabels;
    private double _elapsed;
    private double _lastWallSeconds;
    private int _lastStep = -1;
    private int _output = 2418;

    public ObservableCollection<ProcessStep> Steps { get; } = [];
    public ObservableCollection<EventRow> Events { get; } = [];

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        SeedProcess();
        BuildScene();

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
        _timer.Tick += OnSimulationTick;
        _timer.Start();

        Loaded += (_, _) =>
        {
            SetHomeCamera();
            _updatingSelection = true;
            StepList.SelectedIndex = 0;
            _updatingSelection = false;
            AddEvent("系统", "密封钉焊接三维工作岛加载完成，模拟 PLC 已连接");
        };
    }

    private void SeedProcess()
    {
        Steps.Add(new ProcessStep
        {
            Code = "01", Name = "电芯进机移载", Device = "升降门 / 伺服滑台",
            FocusPoint = new Point3D(-4.5, 0, 1.5)
        });
        Steps.Add(new ProcessStep
        {
            Code = "02", Name = "定位夹紧与测高", Device = "对中夹具 / 位移传感器",
            FocusPoint = new Point3D(0, 0, 1.8)
        });
        Steps.Add(new ProcessStep
        {
            Code = "03", Name = "注液孔激光清洗", Device = "清洗激光器 / 除尘罩",
            FocusPoint = new Point3D(0, 0, 2.2)
        });
        Steps.Add(new ProcessStep
        {
            Code = "04", Name = "密封钉取放压装", Device = "振动盘 / 真空吸嘴",
            FocusPoint = new Point3D(0, 0, 2.2)
        });
        Steps.Add(new ProcessStep
        {
            Code = "05", Name = "预焊与环形满焊", Device = "振镜焊接头 / 保护气",
            FocusPoint = new Point3D(0, 0, 2.3)
        });
        Steps.Add(new ProcessStep
        {
            Code = "06", Name = "焊缝视觉检测", Device = "同轴视觉 / 焊缝算法",
            FocusPoint = new Point3D(0, 0, 2.2)
        });
        Steps.Add(new ProcessStep
        {
            Code = "07", Name = "松夹与电芯出机", Device = "伺服滑台 / 出料门",
            FocusPoint = new Point3D(4.5, 0, 1.5)
        });
    }

    private void BuildScene()
    {
        Viewport.Children.Clear();
        Viewport.Children.Add(new SunLight());

        var floorMaterial = Mat(32, 45, 58);
        Viewport.Children.Add(Box(new Point3D(0, 0, -0.16), 24, 14, 0.3, floorMaterial));
        Viewport.Children.Add(new GridLinesVisual3D
        {
            Center = new Point3D(0, 0, 0),
            Width = 24,
            Length = 14,
            MinorDistance = 1,
            MajorDistance = 5,
            Thickness = 0.01,
            Fill = new SolidColorBrush(Color.FromRgb(58, 78, 98))
        });

        AddMachineCabinet();
        AddInternalShuttle();
        AddCentralFixture();
        AddInternalTooling();
        AddDetailedMechanisms();
        AddProductionUtilities();
        AddMovingCell();
        AddInternalLabels();
    }

    private void AddMachineCabinet()
    {
        var root = new ModelVisual3D();
        var baseMaterial = Mat(75, 91, 108);
        var frame = Mat(205, 214, 222);
        var warning = Mat(245, 158, 11);
        var glass = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(48, 56, 189, 248)));

        root.Children.Add(Box(new Point3D(0, 0, 0.43), 20.2, 8.4, 0.82, baseMaterial));
        // 教学剖视结构：顶部只保留框架梁，避免遮挡内部动作。
        root.Children.Add(Box(new Point3D(0, -3.85, 8.25), 20.0, 0.22, 0.28, frame));
        root.Children.Add(Box(new Point3D(0, 3.85, 8.25), 20.0, 0.22, 0.28, frame));
        root.Children.Add(Box(new Point3D(-9.85, 0, 8.25), 0.22, 7.7, 0.28, frame));
        root.Children.Add(Box(new Point3D(9.85, 0, 8.25), 0.22, 7.7, 0.28, frame));
        foreach (double x in new[] { -9.85, 9.85 })
        foreach (double y in new[] { -3.85, 3.85 })
            root.Children.Add(Box(new Point3D(x, y, 4.55), 0.22, 0.22, 7.4, frame));

        // 中间立柱仅保留在后侧，前侧作为教学剖视观察口。
        foreach (double x in new[] { -6.6, -3.3, 0.0, 3.3, 6.6 })
            root.Children.Add(Box(new Point3D(x, 3.85, 4.55), 0.16, 0.16, 7.4, frame));

        // 仅保留后侧玻璃；朝向相机的一面开放，避免 WPF 透明面遮挡内部模型。
        root.Children.Add(Box(new Point3D(0, 3.88, 4.55), 19.4, 0.04, 7.15, glass));
        root.Children.Add(Box(new Point3D(0, 3.92, 0.88), 19.6, 0.1, 0.12, warning));
        root.Children.Add(Box(new Point3D(0, -3.92, 0.88), 19.6, 0.1, 0.12, warning));

        // 两侧自动升降门，留下电芯进出通道。
        foreach (double x in new[] { InfeedDoorX, OutfeedDoorX })
        {
            root.Children.Add(Box(new Point3D(x, -2.45, 4.2), 0.1, 2.3, 6.6, glass));
            root.Children.Add(Box(new Point3D(x, 2.45, 4.2), 0.1, 2.3, 6.6, glass));
            root.Children.Add(Box(new Point3D(x, 0, 7.25), 0.22, 2.65, 0.2, warning));
        }

        var doorGlass = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(115, 34, 211, 238)));
        _infeedDoor.Children.Add(Box(new Point3D(0, 0, 0), 0.1, 2.6, 4.2, doorGlass));
        _outfeedDoor.Children.Add(Box(new Point3D(0, 0, 0), 0.1, 2.6, 4.2, doorGlass));
        Viewport.Children.Add(_infeedDoor);
        Viewport.Children.Add(_outfeedDoor);

        // 右前方控制柜与三色灯。
        root.Children.Add(Box(new Point3D(7.7, -3.45, 3.25), 2.55, 0.65, 4.8, baseMaterial));
        root.Children.Add(Box(new Point3D(7.7, -3.82, 3.55), 1.45, 0.09, 0.95, Mat(24, 45, 64)));
        root.Children.Add(Cylinder(new Point3D(8.65, -3.45, 5.65), new Point3D(8.65, -3.45, 6.25), 0.1, frame));
        root.Children.Add(Cylinder(new Point3D(8.65, -3.45, 6.25), new Point3D(8.65, -3.45, 6.55), 0.22, Mat(34, 197, 94)));
        Viewport.Children.Add(root);
    }

    private void AddInternalShuttle()
    {
        var root = new ModelVisual3D();
        var rail = Mat(122, 139, 155);
        var dark = Mat(34, 46, 59);
        root.Children.Add(Box(new Point3D(0, -0.72, 0.82), 19.2, 0.16, 0.2, rail));
        root.Children.Add(Box(new Point3D(0, 0.72, 0.82), 19.2, 0.16, 0.2, rail));
        root.Children.Add(Box(new Point3D(0, 0, 0.66), 19.2, 0.7, 0.16, dark));
        for (double x = -9.2; x <= 9.2; x += 0.8)
            root.Children.Add(Box(new Point3D(x, 0, 0.92), 0.05, 1.55, 0.04, Mat(73, 91, 108)));
        Viewport.Children.Add(root);

        _shuttle.Children.Add(Box(new Point3D(0, 0, 0), 4.35, 1.72, 0.2, Mat(96, 165, 250)));
        _shuttle.Children.Add(Box(new Point3D(0, 0, 0.14), 4.12, 1.5, 0.1, Mat(165, 180, 194)));
        foreach (double y in new[] { -0.74, 0.74 })
            _shuttle.Children.Add(Box(new Point3D(0, y, 0.28), 4.0, 0.1, 0.24, Mat(249, 115, 22)));
        Viewport.Children.Add(_shuttle);
    }

    private void AddCentralFixture()
    {
        var root = new ModelVisual3D();
        var steel = Mat(164, 180, 194);
        var dark = Mat(48, 62, 76);
        root.Children.Add(Box(new Point3D(0, 0, 0.9), 4.85, 1.9, 0.18, dark));
        root.Children.Add(Box(new Point3D(-2.28, 0, 1.2), 0.18, 1.62, 0.62, steel));
        root.Children.Add(Box(new Point3D(2.28, 0, 1.2), 0.18, 1.62, 0.62, steel));

        _leftClamp.Children.Add(Box(new Point3D(0, 0, 0), 0.7, 0.24, 0.48, Mat(249, 115, 22)));
        _rightClamp.Children.Add(Box(new Point3D(0, 0, 0), 0.7, 0.24, 0.48, Mat(249, 115, 22)));
        Viewport.Children.Add(root);
        Viewport.Children.Add(_leftClamp);
        Viewport.Children.Add(_rightClamp);
    }

    private void AddInternalTooling()
    {
        var root = new ModelVisual3D();
        var steel = Mat(190, 201, 211);
        var dark = Mat(37, 50, 64);

        foreach (double x in new[] { -3.0, 3.0 })
        foreach (double y in new[] { -2.3, 2.3 })
            root.Children.Add(Box(new Point3D(x, y, 3.25), 0.2, 0.2, 4.8, steel));
        root.Children.Add(Box(new Point3D(0, -2.3, 5.58), 6.3, 0.28, 0.3, steel));
        root.Children.Add(Box(new Point3D(0, 2.3, 5.58), 6.3, 0.28, 0.3, steel));
        root.Children.Add(Box(new Point3D(0, 0, 5.58), 0.7, 4.85, 0.38, Mat(249, 115, 22)));

        _cameraHead.Children.Add(Box(new Point3D(0, 0, 0), 0.5, 0.5, 0.55, dark));
        _cameraHead.Children.Add(Cylinder(new Point3D(0, 0, -0.25), new Point3D(0, 0, -0.48), 0.16, Mat(38, 217, 199)));
        for (var i = 0; i < 12; i++)
        {
            var angle = i * Math.PI * 2 / 12;
            _cameraHead.Children.Add(Sphere(new Point3D(
                Math.Cos(angle) * 0.22,
                Math.Sin(angle) * 0.22,
                -0.49), 0.035, Mat(248, 250, 252)));
        }
        Viewport.Children.Add(_cameraHead);

        _cleaningHead.Children.Add(Box(new Point3D(0, 0, 0), 0.62, 0.62, 0.55, Mat(96, 165, 250)));
        _cleaningHead.Children.Add(Cylinder(new Point3D(0, 0, -0.25), new Point3D(0, 0, -0.72), 0.12, dark));
        Viewport.Children.Add(_cleaningHead);

        _pinTool.Children.Add(Box(new Point3D(0, 0, 0), 0.54, 0.54, 0.42, Mat(249, 115, 22)));
        _pinTool.Children.Add(Cylinder(new Point3D(0, 0, -0.2), new Point3D(0, 0, -1.0), 0.12, steel));
        _pinTool.Children.Add(Cylinder(new Point3D(0, 0, -1.0), new Point3D(0, 0, -1.14), 0.2, Mat(220, 227, 233)));
        Viewport.Children.Add(_pinTool);

        _laserAssembly.Children.Add(Box(new Point3D(0, 0, 0), 0.65, 0.65, 0.6, dark));
        _laserAssembly.Children.Add(Cylinder(new Point3D(0, 0, -0.28), new Point3D(0, 0, -0.62), 0.16, Mat(239, 68, 68)));
        var beam = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(215, 255, 55, 40)));
        _laserAssembly.Children.Add(Cylinder(new Point3D(0, 0, -0.62), new Point3D(0, 0, -1.42), 0.035, beam));
        Viewport.Children.Add(_laserAssembly);

        var scanMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(90, 34, 211, 238)));
        _ccdScanPlane.Children.Add(Box(new Point3D(0, 0, 0), 1.15, 0.9, 0.025, scanMaterial));
        Viewport.Children.Add(_ccdScanPlane);

        AddInternalWeldRing();
        Viewport.Children.Add(root);
    }

    private void AddDetailedMechanisms()
    {
        var root = new ModelVisual3D();
        var steel = Mat(174, 188, 201);
        var dark = Mat(43, 56, 70);
        var rail = Mat(87, 104, 121);
        var orange = Mat(249, 115, 22);
        var cyan = Mat(34, 211, 238);
        var brass = Mat(202, 138, 62);

        // 中央顶升定位台：四根导柱、顶升气缸、定位销和承载板。
        _liftTable.Children.Add(Box(new Point3D(0, 0, 0.16), 4.92, 1.82, 0.18, dark));
        foreach (double x in new[] { -2.02, 2.02 })
        foreach (double y in new[] { -0.66, 0.66 })
        {
            _liftTable.Children.Add(Cylinder(new Point3D(x, y, 0.1),
                new Point3D(x, y, 0.72), 0.1, steel));
            _liftTable.Children.Add(Cylinder(new Point3D(x, y, 0.72),
                new Point3D(x, y, 0.83), 0.17, brass));
        }
        _liftTable.Children.Add(Cylinder(new Point3D(0, 0, -0.18),
            new Point3D(0, 0, 0.55), 0.42, rail));
        _liftTable.Children.Add(Box(new Point3D(0, 0, 0.76), 4.68, 1.66, 0.14, steel));
        Viewport.Children.Add(_liftTable);

        // 专利中的 H 形翻转压钉组件：两端压块带激光避让孔，中部由转轴连接。
        _hPress.Children.Add(Box(new Point3D(0, -0.55, 0), 1.05, 0.2, 0.13, orange));
        _hPress.Children.Add(Box(new Point3D(0, 0.55, 0), 1.05, 0.2, 0.13, orange));
        _hPress.Children.Add(Box(new Point3D(0, 0, 0), 0.18, 1.3, 0.13, orange));
        foreach (double y in new[] { -0.55, 0.55 })
        {
            // 压块上的镂空位用深色环表示，预焊光束从中央穿过。
            _hPress.Children.Add(Cylinder(new Point3D(0, y, -0.08),
                new Point3D(0, y, 0.08), 0.29, dark));
            _hPress.Children.Add(Cylinder(new Point3D(0, y, -0.09),
                new Point3D(0, y, 0.09), 0.15, cyan));
        }
        _hPress.Children.Add(Cylinder(new Point3D(-0.66, -0.7, 0),
            new Point3D(-0.66, 0.7, 0), 0.11, steel));
        Viewport.Children.Add(_hPress);

        // 龙门 X 轴滑座、Z 轴模组、伺服电机和滑块。
        _gantryCarriage.Children.Add(Box(new Point3D(0, 0, 0), 0.72, 3.05, 0.32, orange));
        _gantryCarriage.Children.Add(Box(new Point3D(0, 0, -0.5), 0.62, 0.62, 1.25, rail));
        _gantryCarriage.Children.Add(Box(new Point3D(0, -0.38, -0.52), 0.42, 0.18, 1.05, steel));
        _gantryCarriage.Children.Add(Box(new Point3D(0, 0.38, -0.52), 0.42, 0.18, 1.05, steel));
        _gantryCarriage.Children.Add(Cylinder(new Point3D(0, 0, 0.18),
            new Point3D(0, 0, 0.62), 0.28, dark));
        _gantryCarriage.Children.Add(Cylinder(new Point3D(0, 0, 0.62),
            new Point3D(0, 0, 0.86), 0.22, Mat(96, 165, 250)));
        Viewport.Children.Add(_gantryCarriage);

        // 顶部直线导轨、丝杆和拖链。
        root.Children.Add(Box(new Point3D(0, -2.05, 5.43), 5.8, 0.14, 0.16, dark));
        root.Children.Add(Box(new Point3D(0, 2.05, 5.43), 5.8, 0.14, 0.16, dark));
        root.Children.Add(Cylinder(new Point3D(-2.75, 0, 5.46),
            new Point3D(2.75, 0, 5.46), 0.09, steel));
        root.Children.Add(Cylinder(new Point3D(2.77, 0, 5.46),
            new Point3D(3.28, 0, 5.46), 0.38, Mat(96, 165, 250)));
        for (var i = 0; i < 16; i++)
        {
            root.Children.Add(Box(new Point3D(-1.55 + i * 0.19, 2.35, 5.72),
                0.14, 0.32, 0.12, dark));
        }

        // 密封钉振动盘、直振轨道和取钉等待位。
        root.Children.Add(Cylinder(new Point3D(-3.55, 1.25, 1.02),
            new Point3D(-3.55, 1.25, 1.48), 1.25, rail));
        root.Children.Add(Cylinder(new Point3D(-3.55, 1.25, 1.48),
            new Point3D(-3.55, 1.25, 1.58), 1.48, steel));
        root.Children.Add(Cylinder(new Point3D(-3.55, 1.25, 1.58),
            new Point3D(-3.55, 1.25, 1.65), 1.05, dark));
        root.Children.Add(Box(new Point3D(-2.0, 1.15, 1.58), 2.15, 0.34, 0.14, steel));
        root.Children.Add(Box(new Point3D(-2.0, 1.15, 1.67), 2.15, 0.18, 0.06, dark));
        for (var i = 0; i < 7; i++)
        {
            root.Children.Add(Cylinder(new Point3D(-2.75 + i * 0.27, 1.15, 1.69),
                new Point3D(-2.75 + i * 0.27, 1.15, 1.76), 0.13, brass));
        }
        root.Children.Add(Box(new Point3D(-0.78, 1.15, 1.58), 0.42, 0.46, 0.18, orange));

        // 除尘罩、抽气管和保护气喷嘴。
        _dustHood.Children.Add(Box(new Point3D(0, 0, 0), 1.45, 1.55, 0.12, cyan));
        foreach (double x in new[] { -0.66, 0.66 })
        foreach (double y in new[] { -0.7, 0.7 })
            _dustHood.Children.Add(Box(new Point3D(x, y, -0.22), 0.07, 0.07, 0.5, cyan));
        _dustHood.Children.Add(Cylinder(new Point3D(0.68, 0.68, 0),
            new Point3D(1.32, 1.3, 0.55), 0.16, dark));
        Viewport.Children.Add(_dustHood);
        root.Children.Add(Cylinder(new Point3D(1.3, 1.3, 3.05),
            new Point3D(2.0, 1.75, 3.65), 0.18, dark));
        root.Children.Add(Cylinder(new Point3D(2.0, 1.75, 3.65),
            new Point3D(4.4, 1.75, 3.65), 0.18, dark));
        root.Children.Add(Cylinder(new Point3D(-0.34, -0.38, 2.7),
            new Point3D(-0.12, -0.18, 2.48), 0.07, brass));

        // 独立焊后检测头：2D 相机、3D 轮廓传感器和双侧条形光。
        _inspectionHead.Children.Add(Box(new Point3D(0, 0, 0), 0.72, 0.52, 0.7, dark));
        _inspectionHead.Children.Add(Cylinder(new Point3D(-0.18, 0, -0.34),
            new Point3D(-0.18, 0, -0.55), 0.13, cyan));
        _inspectionHead.Children.Add(Box(new Point3D(0.22, 0, -0.4), 0.2, 0.35, 0.23, Mat(96, 165, 250)));
        _inspectionHead.Children.Add(Box(new Point3D(0, -0.38, -0.35), 0.72, 0.08, 0.12, Mat(248, 250, 252)));
        _inspectionHead.Children.Add(Box(new Point3D(0, 0.38, -0.35), 0.72, 0.08, 0.12, Mat(248, 250, 252)));
        Viewport.Children.Add(_inspectionHead);

        // 对射传感器、限位开关、气动阀岛和线缆。
        foreach (double x in new[] { -2.65, 2.65 })
        {
            root.Children.Add(Box(new Point3D(x, -0.95, 1.25), 0.16, 0.22, 0.28, dark));
            root.Children.Add(Box(new Point3D(x, 0.95, 1.25), 0.16, 0.22, 0.28, dark));
            root.Children.Add(Cylinder(new Point3D(x, -0.84, 1.25),
                new Point3D(x, 0.84, 1.25), 0.025, Mat(239, 68, 68)));
        }
        root.Children.Add(Box(new Point3D(3.65, 1.55, 1.75), 1.2, 0.38, 1.0, dark));
        for (var i = 0; i < 5; i++)
            root.Children.Add(Box(new Point3D(3.3 + i * 0.18, 1.34, 1.82), 0.12, 0.08, 0.45, Mat(96, 165, 250)));

        // 丝杆安装座、直线滑块和可见紧固件。
        foreach (double x in new[] { -2.8, -1.4, 0.0, 1.4, 2.8 })
        foreach (double y in new[] { -0.72, 0.72 })
        {
            root.Children.Add(Box(new Point3D(x, y, 0.94), 0.28, 0.28, 0.12, dark));
            root.Children.Add(Cylinder(new Point3D(x, y, 1.0),
                new Point3D(x, y, 1.08), 0.07, brass));
        }

        Viewport.Children.Add(root);
    }

    private void AddProductionUtilities()
    {
        var root = new ModelVisual3D();
        var cabinet = Mat(64, 78, 93);
        var panel = Mat(31, 45, 60);
        var steel = Mat(177, 190, 202);
        var cyan = Mat(34, 211, 238);
        var orange = Mat(249, 115, 22);
        var dark = Mat(29, 39, 51);

        // 后侧激光器主机和水冷机，量产设备不会把这些单元省略在机架之外。
        root.Children.Add(Box(new Point3D(5.9, 2.65, 2.85), 2.35, 1.65, 4.15, cabinet));
        root.Children.Add(Box(new Point3D(5.9, 1.8, 3.25), 1.45, 0.08, 0.9, panel));
        root.Children.Add(Box(new Point3D(5.9, 1.79, 1.55), 1.55, 0.08, 1.05, dark));
        for (var i = 0; i < 7; i++)
            root.Children.Add(Box(new Point3D(5.35 + i * 0.18, 1.74, 1.55), 0.08, 0.04, 0.85, steel));

        root.Children.Add(Box(new Point3D(-7.35, 2.55, 2.65), 2.7, 1.85, 3.8, cabinet));
        root.Children.Add(Box(new Point3D(-7.35, 1.59, 2.9), 1.55, 0.08, 1.25, panel));
        for (var i = 0; i < 6; i++)
            root.Children.Add(Box(new Point3D(-7.95 + i * 0.24, 1.54, 2.9), 0.12, 0.04, 1.05, cyan));
        root.Children.Add(Cylinder(new Point3D(-7.9, 2.55, 4.55),
            new Point3D(-7.9, 2.55, 5.15), 0.22, dark));

        // 焊烟净化主机、过滤筒和连接至焊接室的抽风管。
        root.Children.Add(Box(new Point3D(8.25, 2.35, 2.55), 1.8, 1.65, 3.55, cabinet));
        root.Children.Add(Cylinder(new Point3D(7.85, 2.35, 4.35),
            new Point3D(7.85, 2.35, 5.35), 0.42, steel));
        root.Children.Add(Cylinder(new Point3D(8.55, 2.35, 4.35),
            new Point3D(8.55, 2.35, 5.35), 0.42, steel));
        root.Children.Add(Cylinder(new Point3D(4.4, 1.75, 3.65),
            new Point3D(7.85, 2.35, 5.35), 0.22, dark));

        // 后侧成排电控柜、驱动器和安全继电器。
        foreach (double x in new[] { -4.8, -2.8, -0.8, 1.2, 3.2 })
        {
            root.Children.Add(Box(new Point3D(x, 3.2, 2.45), 1.72, 0.8, 3.75, cabinet));
            root.Children.Add(Box(new Point3D(x, 2.78, 2.65), 1.25, 0.05, 0.72, panel));
            root.Children.Add(Cylinder(new Point3D(x + 0.58, 2.72, 2.7),
                new Point3D(x + 0.58, 2.67, 2.7), 0.1, orange));
        }

        // 前方悬臂操作屏、按钮盒和急停。
        root.Children.Add(Cylinder(new Point3D(6.15, -3.3, 0.85),
            new Point3D(6.15, -3.3, 4.45), 0.13, steel));
        root.Children.Add(Cylinder(new Point3D(6.15, -3.3, 4.45),
            new Point3D(5.35, -3.85, 4.65), 0.12, steel));
        root.Children.Add(Box(new Point3D(4.9, -4.05, 4.65), 1.65, 0.22, 1.15, cabinet));
        root.Children.Add(Box(new Point3D(4.9, -4.18, 4.72), 1.3, 0.05, 0.82, Mat(21, 72, 100)));
        root.Children.Add(Box(new Point3D(5.45, -4.18, 4.08), 0.28, 0.05, 0.22, orange));

        // 进出料口安全光栅和红外检测束。
        foreach (double x in new[] { -9.25, 9.25 })
        {
            foreach (double y in new[] { -1.65, 1.65 })
            {
                root.Children.Add(Box(new Point3D(x, y, 3.35), 0.16, 0.16, 4.9, dark));
                for (var i = 0; i < 7; i++)
                    root.Children.Add(Sphere(new Point3D(x, y, 1.35 + i * 0.64), 0.055, Mat(239, 68, 68)));
            }
            for (var i = 0; i < 7; i++)
                root.Children.Add(Cylinder(new Point3D(x, -1.62, 1.35 + i * 0.64),
                    new Point3D(x, 1.62, 1.35 + i * 0.64), 0.018, Mat(239, 68, 68)));
        }

        // 顶部桥架、气管总管和检修照明。
        root.Children.Add(Box(new Point3D(0, 3.15, 7.75), 17.5, 0.55, 0.18, dark));
        for (var i = 0; i < 28; i++)
            root.Children.Add(Box(new Point3D(-8.4 + i * 0.62, 3.15, 7.88), 0.38, 0.48, 0.08, steel));
        root.Children.Add(Cylinder(new Point3D(-8.4, 2.65, 7.45),
            new Point3D(8.4, 2.65, 7.45), 0.1, cyan));
        foreach (double x in new[] { -6.0, -2.0, 2.0, 6.0 })
            root.Children.Add(Box(new Point3D(x, -3.45, 7.15), 2.2, 0.16, 0.12, Mat(248, 250, 252)));

        // 气源处理三联件与主气阀。
        root.Children.Add(Box(new Point3D(-8.15, -3.25, 2.35), 1.15, 0.45, 1.9, cabinet));
        for (var i = 0; i < 3; i++)
        {
            root.Children.Add(Cylinder(new Point3D(-8.48 + i * 0.34, -3.5, 2.1),
                new Point3D(-8.48 + i * 0.34, -3.5, 2.85), 0.18, steel));
        }

        Viewport.Children.Add(root);
    }

    private void AddInternalWeldRing()
    {
        var weld = Mat(255, 158, 11);
        const int count = 30;
        const double radius = 0.24;
        for (var i = 0; i < count; i++)
        {
            var a1 = i * Math.PI * 2 / count;
            var a2 = (i + 1) * Math.PI * 2 / count;
            var p1 = new Point3D(Math.Cos(a1) * radius, Math.Sin(a1) * radius, 0);
            var p2 = new Point3D(Math.Cos(a2) * radius, Math.Sin(a2) * radius, 0);
            _weldRing.Children.Add(Cylinder(p1, p2, 0.035, weld));
            _weldRing2.Children.Add(Cylinder(p1, p2, 0.035, weld));
        }
        Viewport.Children.Add(_weldRing);
        Viewport.Children.Add(_weldRing2);
    }

    private void AddInternalLabels()
    {
        var labels = new[]
        {
            ("进机口", new Point3D(InfeedDoorX, -2.2, 6.4)),
            ("伺服移载滑台", new Point3D(-5.2, -2.0, 1.35)),
            ("定位焊接工位", new Point3D(0, -2.8, 6.0)),
            ("出机口", new Point3D(OutfeedDoorX, -2.2, 6.4))
        };
        foreach (var (text, position) in labels)
        {
            _sceneLabels.Add(new BillboardTextVisual3D
            {
                Text = text,
                Position = position,
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromArgb(185, 11, 28, 44)),
                FontSize = 10,
                Padding = new Thickness(5, 2, 5, 2)
            });
        }
    }

    private void AddConveyor()
    {
        var root = new ModelVisual3D();
        var steel = Mat(116, 132, 148);
        var belt = Mat(32, 95, 112);
        var dark = Mat(27, 37, 48);

        root.Children.Add(Box(new Point3D(-0.6, 0, 0.85), 23.6, 1.8, 0.18, belt));
        root.Children.Add(Box(new Point3D(-0.6, -0.98, 0.76), 23.8, 0.13, 0.35, steel));
        root.Children.Add(Box(new Point3D(-0.6, 0.98, 0.76), 23.8, 0.13, 0.35, steel));
        for (double x = -12; x <= 10.8; x += 1.0)
        {
            root.Children.Add(Cylinder(new Point3D(x, -0.82, 0.96), new Point3D(x, 0.82, 0.96), 0.1, dark));
        }
        for (double x = -11.7; x <= 10.5; x += 3.7)
        {
            root.Children.Add(Box(new Point3D(x, -0.78, 0.36), 0.15, 0.15, 0.8, steel));
            root.Children.Add(Box(new Point3D(x, 0.78, 0.36), 0.15, 0.15, 0.8, steel));
        }
        Viewport.Children.Add(root);
    }

    private void AddProtectiveEnclosure()
    {
        var root = new ModelVisual3D();
        var frame = Mat(230, 178, 22);
        var glass = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(35, 56, 189, 248)));
        foreach (double x in new[] { -8.8, -2.25, 4.35, 7.7 })
        foreach (double y in new[] { -2.1, 2.1 })
            root.Children.Add(Box(new Point3D(x, y, 1.75), 0.12, 0.12, 3.5, frame));

        foreach (double y in new[] { -2.1, 2.1 })
        {
            root.Children.Add(Box(new Point3D(-5.5, y, 3.45), 6.6, 0.1, 0.12, frame));
            root.Children.Add(Box(new Point3D(1.05, y, 3.45), 6.6, 0.1, 0.12, frame));
            root.Children.Add(Box(new Point3D(6.0, y, 3.45), 3.4, 0.1, 0.12, frame));
            root.Children.Add(Box(new Point3D(-5.5, y, 2.25), 6.4, 0.04, 2.1, glass));
            root.Children.Add(Box(new Point3D(1.05, y, 2.25), 6.4, 0.04, 2.1, glass));
            root.Children.Add(Box(new Point3D(6.0, y, 2.25), 3.2, 0.04, 2.1, glass));
        }
        Viewport.Children.Add(root);
    }

    private void AddLoadFixture()
    {
        var root = new ModelVisual3D();
        var steel = Mat(130, 148, 165);
        var clamp = Mat(249, 115, 22);
        root.Children.Add(Box(new Point3D(StationX[0], 0, 1.08), 2.1, 1.7, 0.18, steel));
        foreach (double y in new[] { -0.72, 0.72 })
            root.Children.Add(Box(new Point3D(StationX[0], y, 1.37), 1.2, 0.18, 0.5, clamp));
        Viewport.Children.Add(root);
    }

    private void AddCcdStation(double x, string label)
    {
        var root = new ModelVisual3D();
        var steel = Mat(172, 184, 196);
        var dark = Mat(30, 42, 55);
        var lens = Mat(38, 217, 199);
        foreach (double y in new[] { -1.45, 1.45 })
            root.Children.Add(Box(new Point3D(x, y, 2.0), 0.16, 0.16, 2.7, steel));
        root.Children.Add(Box(new Point3D(x, 0, 3.28), 0.24, 3.05, 0.2, steel));
        root.Children.Add(Box(new Point3D(x, 0, 2.84), 0.55, 0.55, 0.55, dark));
        root.Children.Add(Cylinder(new Point3D(x, 0, 2.56), new Point3D(x, 0, 2.35), 0.17, lens));
        Viewport.Children.Add(root);

        if (label.StartsWith("CCD"))
        {
            var scanMaterial = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(80, 34, 211, 238)));
            _ccdScanPlane.Children.Add(Box(new Point3D(0, 0, 0), 1.3, 1.1, 0.025, scanMaterial));
            Viewport.Children.Add(_ccdScanPlane);
        }
    }

    private void AddCleaningStation()
    {
        var root = new ModelVisual3D();
        var steel = Mat(175, 187, 198);
        var blue = Mat(96, 165, 250);
        var hose = Mat(52, 65, 80);
        root.Children.Add(Box(new Point3D(StationX[2], -1.25, 2.15), 0.22, 0.22, 2.5, steel));
        root.Children.Add(Box(new Point3D(StationX[2], 0, 3.25), 0.35, 2.7, 0.24, steel));
        root.Children.Add(Box(new Point3D(StationX[2], 0, 2.68), 0.65, 0.65, 0.65, blue));
        root.Children.Add(Cylinder(new Point3D(StationX[2], 0, 2.38), new Point3D(StationX[2], 0, 1.75), 0.12, hose));
        root.Children.Add(Cylinder(new Point3D(StationX[2] + 0.35, 0.45, 2.6),
            new Point3D(StationX[2] + 0.7, 1.55, 2.8), 0.18, hose));
        Viewport.Children.Add(root);
    }

    private void AddPinPlacementStation()
    {
        var root = new ModelVisual3D();
        var steel = Mat(186, 198, 208);
        var orange = Mat(249, 115, 22);
        var feeder = Mat(68, 84, 102);
        root.Children.Add(Cylinder(new Point3D(StationX[3] - 0.9, 1.1, 1.0),
            new Point3D(StationX[3] - 0.9, 1.1, 1.8), 1.25, feeder));
        root.Children.Add(Box(new Point3D(StationX[3], 0, 3.25), 3.0, 0.26, 0.25, steel));
        root.Children.Add(Box(new Point3D(StationX[3], 0, 2.9), 0.55, 0.6, 0.42, orange));
        _pinTool.Children.Add(Cylinder(new Point3D(0, 0, 0), new Point3D(0, 0, -1.15), 0.14, steel));
        _pinTool.Children.Add(Cylinder(new Point3D(0, 0, -1.15), new Point3D(0, 0, -1.28), 0.22, orange));
        root.Children.Add(_pinTool);
        Viewport.Children.Add(root);
    }

    private void AddWeldingStation()
    {
        var root = new ModelVisual3D();
        var steel = Mat(197, 207, 216);
        var red = Mat(239, 68, 68);
        var dark = Mat(40, 52, 66);
        foreach (double y in new[] { -1.45, 1.45 })
            root.Children.Add(Box(new Point3D(StationX[4], y, 2.0), 0.18, 0.18, 2.7, steel));
        root.Children.Add(Box(new Point3D(StationX[4], 0, 3.28), 0.35, 3.1, 0.28, steel));
        root.Children.Add(Box(new Point3D(StationX[4], 0, 2.78), 0.75, 0.75, 0.72, dark));
        root.Children.Add(Cylinder(new Point3D(StationX[4], 0, 2.43),
            new Point3D(StationX[4], 0, 2.15), 0.2, red));

        var beam = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(210, 255, 59, 48)));
        _laserAssembly.Children.Add(Cylinder(new Point3D(0, 0, 0), new Point3D(0, 0, -1.15), 0.035, beam));
        _laserAssembly.Children.Add(Sphere(new Point3D(0, 0, -1.15), 0.09, Mat(255, 186, 34)));
        root.Children.Add(_laserAssembly);
        AddWeldRing(root);
        Viewport.Children.Add(root);
    }

    private void AddWeldRing(ModelVisual3D root)
    {
        var weld = Mat(255, 158, 11);
        const int count = 30;
        const double radius = 0.24;
        for (var i = 0; i < count; i++)
        {
            var a1 = i * Math.PI * 2 / count;
            var a2 = (i + 1) * Math.PI * 2 / count;
            var p1 = new Point3D(StationX[4] + Math.Cos(a1) * radius, Math.Sin(a1) * radius, 1.88);
            var p2 = new Point3D(StationX[4] + Math.Cos(a2) * radius, Math.Sin(a2) * radius, 1.88);
            root.Children.Add(Cylinder(p1, p2, 0.035, weld));
        }
    }

    private void AddSortingStation()
    {
        var root = new ModelVisual3D();
        var steel = Mat(135, 151, 167);
        var green = Mat(34, 197, 94);
        var red = Mat(239, 68, 68);
        root.Children.Add(Box(new Point3D(StationX[6], 0, 1.05), 2.0, 1.8, 0.18, steel));
        root.Children.Add(Box(new Point3D(StationX[6] + 0.6, -1.35, 0.92), 2.5, 0.7, 0.16, green));
        root.Children.Add(Box(new Point3D(StationX[6] + 0.6, 1.35, 0.92), 2.5, 0.7, 0.16, red));
        Viewport.Children.Add(root);
    }

    private void AddMovingCell()
    {
        var aluminum = Mat(14, 105, 166);
        var top = Mat(48, 60, 72);
        var copper = Mat(202, 138, 62);
        var blue = Mat(152, 166, 179);
        BuildCellGeometry(_movingCell, aluminum, top, copper, blue);
        BuildCellGeometry(_movingCell2, aluminum, top, copper, blue);
        Viewport.Children.Add(_movingCell);
        Viewport.Children.Add(_movingCell2);

        _cellLabel.Text = "▼ 双电芯托盘";
        _cellLabel.Foreground = new SolidColorBrush(Color.FromRgb(253, 224, 71));
        _cellLabel.Background = new SolidColorBrush(Color.FromArgb(190, 55, 38, 10));
        _cellLabel.FontSize = 11;
        _cellLabel.Padding = new Thickness(6, 2, 6, 2);
        _sceneLabels.Add(_cellLabel);
    }

    private static void BuildCellGeometry(ModelVisual3D cell, Material aluminum, Material top,
        Material copper, Material blue)
    {
        var length = CellSpec.SceneLength;
        var width = CellSpec.SceneWidth;
        var height = CellSpec.SceneHeight;
        var white = Mat(236, 244, 250);
        var silver = Mat(188, 201, 212);
        var vent = Mat(112, 126, 140);

        // 海辰千安时级电芯的核心特征是超长、超薄的方壳比例。
        cell.Children.Add(Box(new Point3D(0, 0, height / 2), length, width, height, aluminum));
        cell.Children.Add(Box(new Point3D(0, 0, height + 0.04), length + 0.04, width + 0.07, 0.08, top));
        cell.Children.Add(Box(new Point3D(-length / 2 + 0.045, 0, height / 2),
            0.06, width + 0.025, height - 0.08, silver));
        cell.Children.Add(Box(new Point3D(length / 2 - 0.045, 0, height / 2),
            0.06, width + 0.025, height - 0.08, silver));

        var terminalX = length * 0.38;
        foreach (var (x, material) in new[] { (-terminalX, copper), (terminalX, blue) })
        {
            cell.Children.Add(Cylinder(new Point3D(x, 0, height + 0.075),
                new Point3D(x, 0, height + 0.22), 0.22, material));
            cell.Children.Add(Cylinder(new Point3D(x, 0, height + 0.055),
                new Point3D(x, 0, height + 0.08), 0.32, silver));
        }

        // 中央密封钉、偏置防爆阀和三组可见气道加强筋。
        cell.Children.Add(Cylinder(new Point3D(0, 0, height + 0.075),
            new Point3D(0, 0, height + 0.115), 0.19, silver));
        cell.Children.Add(Box(new Point3D(length * 0.14, 0, height + 0.09),
            0.5, width * 0.52, 0.045, vent));
        foreach (double x in new[] { -0.62, 0.0, 0.62 })
            cell.Children.Add(Box(new Point3D(x, 0, height + 0.095),
                0.38, width * 0.28, 0.035, Mat(91, 105, 119)));

        // 侧面的海辰蓝色包膜和简化 ∞Cell 识别图形。
        var faceY = -width / 2 - 0.012;
        cell.Children.Add(Cylinder(new Point3D(-0.2, faceY, height * 0.58),
            new Point3D(-0.2, faceY - 0.018, height * 0.58), 0.22, white));
        cell.Children.Add(Cylinder(new Point3D(0.2, faceY, height * 0.58),
            new Point3D(0.2, faceY - 0.018, height * 0.58), 0.22, white));
        cell.Children.Add(Box(new Point3D(0, faceY - 0.01, height * 0.58),
            0.28, 0.025, 0.07, white));
        cell.Children.Add(Box(new Point3D(0, faceY - 0.01, height * 0.32),
            length * 0.42, 0.025, 0.04, white));
    }

    private void AddLabels()
    {
        string[] labels = ["上料定位", "CCD定位", "激光清洗", "密封钉装配", "激光焊接", "焊后检测", "OK / NG"];
        for (var i = 0; i < labels.Length; i++)
        {
            Viewport.Children.Add(new BillboardTextVisual3D
            {
                Text = labels[i],
                Position = new Point3D(StationX[i], -1.7, i is 1 or 2 or 4 or 5 ? 3.7 : 2.25),
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromArgb(205, 11, 28, 44)),
                FontSize = 13,
                Padding = new Thickness(7, 3, 7, 3)
            });
        }
    }

    private void OnSimulationTick(object? sender, EventArgs e)
    {
        ClockText.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");
        var wallSeconds = _simulationClock.Elapsed.TotalSeconds;
        var deltaSeconds = Math.Clamp(wallSeconds - _lastWallSeconds, 0, 0.5);
        _lastWallSeconds = wallSeconds;
        if (!_running || _fault) return;

        _elapsed += deltaSeconds;
        const double secondsPerStep = 5.0;
        var stepIndex = (int)(_elapsed / secondsPerStep) % Steps.Count;
        var local = (_elapsed % secondsPerStep) / secondsPerStep;
        UpdateStepState(stepIndex);
        AnimateCell(stepIndex, local);
        AnimateTools(stepIndex, local);
        UpdateTelemetry(stepIndex);

        if (stepIndex != _lastStep)
        {
            _lastStep = stepIndex;
            CurrentStepText.Text = Steps[stepIndex].Name;
            AddEvent("工序", $"{Steps[stepIndex].Code} {Steps[stepIndex].Name} 开始执行");
            if (stepIndex == 0 && _elapsed > 1)
            {
                _output++;
                OutputText.Text = _output.ToString("N0");
                AddEvent("质量", "上一电芯焊缝视觉检测完成，判定 OK");
            }
        }
    }

    private void UpdateStepState(int activeIndex)
    {
        for (var i = 0; i < Steps.Count; i++)
        {
            Steps[i].State = i == activeIndex ? StepState.Working :
                i < activeIndex ? StepState.Done : StepState.Waiting;
        }
        if (StepList.SelectedIndex != activeIndex)
        {
            _updatingSelection = true;
            StepList.SelectedIndex = activeIndex;
            _updatingSelection = false;
        }
    }

    private void AnimateCell(int index, double local)
    {
        double x;
        if (index == 0)
            x = -11.5 + 6.3 * SmoothStep(local);
        else if (index == 1)
            x = -5.2 + 5.2 * SmoothStep(Math.Min(1, local / 0.55));
        else if (index == Steps.Count - 1)
            x = 11.2 * SmoothStep(local);
        else
            x = 0;

        _movingCell.Transform = new TranslateTransform3D(x, -0.55, 1.25);
        _movingCell2.Transform = new TranslateTransform3D(x, 0.55, 1.25);
        _shuttle.Transform = new TranslateTransform3D(x, 0, 1.02);
        _cellLabel.Position = new Point3D(x, -1.05, 3.10);

        var infeedLift = index == 0 ? 5.0 : 0;
        var outfeedLift = index == Steps.Count - 1 ? 5.0 : 0;
        _infeedDoor.Transform = new TranslateTransform3D(InfeedDoorX, 0, 4.2 + infeedLift);
        _outfeedDoor.Transform = new TranslateTransform3D(OutfeedDoorX, 0, 4.2 + outfeedLift);

        if (_followCell)
        {
            Viewport.Camera = new PerspectiveCamera(
                new Point3D(x + 14, -23, 12),
                new Vector3D(-14, 23, -8.6),
                new Vector3D(0, 0, 1),
                52);
        }
    }

    private void AnimateTools(int index, double local)
    {
        var clampClosed = index is >= 1 and <= 5
            ? SmoothStep(Math.Min(1, local / 0.25))
            : 0;
        var clampY = 1.12 - clampClosed * 0.24;
        _leftClamp.Transform = new TranslateTransform3D(0, -clampY, 1.75);
        _rightClamp.Transform = new TranslateTransform3D(0, clampY, 1.75);
        _liftTable.Transform = new TranslateTransform3D(0, 0,
            index is >= 1 and <= 5 ? 0.16 * SmoothStep(Math.Min(1, local / 0.22)) : 0);

        var locating = index == 1;
        var scanSweep = Math.Sin(local * Math.PI * 4) * 0.62;
        _cameraHead.Transform = locating
            ? new TranslateTransform3D(0, scanSweep, 3.35)
            : new TranslateTransform3D(-1.15, 0, 3.55);
        _ccdScanPlane.Transform = locating
            ? new TranslateTransform3D(0, scanSweep, 2.82)
            : new TranslateTransform3D(0, 0, -100);

        var cleaningActive = index == 2;
        _cleaningHead.Transform = cleaningActive
            ? new TranslateTransform3D(0, Math.Sin(local * Math.PI * 6) * 0.62, 3.53)
            : new TranslateTransform3D(-0.75, 0.65, 3.60);
        _dustHood.Transform = cleaningActive
            ? new TranslateTransform3D(0, 0, 3.08)
            : new TranslateTransform3D(0, 0, -100);

        var pinHalf = (local * 2) % 1;
        var pinY = local < 0.5 ? -0.55 : 0.55;
        var pinPress = index == 3 ? Math.Sin(pinHalf * Math.PI) * 0.48 : 0;
        _pinTool.Transform = index == 3
            ? new TranslateTransform3D(0, pinY, 4.43 - pinPress)
            : new TranslateTransform3D(0.75, 0.65, 3.65);

        if (index == 4)
        {
            var weldHalf = (local * 2) % 1;
            var weldY = local < 0.5 ? -0.55 : 0.55;
            var tackPhase = weldHalf < 0.34;
            var angle = tackPhase
                ? Math.Floor(weldHalf / 0.11) * Math.PI * 2 / 3
                : ((weldHalf - 0.34) / 0.60) * Math.PI * 2;
            _laserAssembly.Transform = new TranslateTransform3D(
                Math.Cos(angle) * 0.24,
                weldY + Math.Sin(angle) * 0.24,
                4.24);
            _weldRing.Transform = local >= 0.17
                ? new TranslateTransform3D(0, -0.55, 2.82)
                : new TranslateTransform3D(0, 0, -100);
            _weldRing2.Transform = local >= 0.67
                ? new TranslateTransform3D(0, 0.55, 2.82)
                : new TranslateTransform3D(0, 0, -100);

            // 预焊时 H 形压块翻下，完成三点固定后立即翻起，为满焊轨迹避让。
            var pressCycle = weldHalf;
            var pressDown = pressCycle < 0.34
                ? SmoothStep(Math.Min(1, pressCycle / 0.12))
                : 1 - SmoothStep(Math.Min(1, (pressCycle - 0.34) / 0.12));
            SetHPressTransform(-82 + pressDown * 82, 3.02 - pressDown * 0.18);
        }
        else
        {
            _laserAssembly.Transform = new TranslateTransform3D(1.15, 0, 3.65);
            _weldRing.Transform = new TranslateTransform3D(0, 0, -100);
            _weldRing2.Transform = new TranslateTransform3D(0, 0, -100);
            SetHPressTransform(-82, 3.12);
        }

        var inspecting = index == 5;
        _inspectionHead.Transform = inspecting
            ? new TranslateTransform3D(0, Math.Sin(local * Math.PI * 5) * 0.62, 3.48)
            : new TranslateTransform3D(1.15, -0.65, 3.58);
        if (inspecting)
            _ccdScanPlane.Transform = new TranslateTransform3D(0, Math.Sin(local * Math.PI * 5) * 0.62, 2.82);

        var carriageX = index switch
        {
            1 => -0.85,
            2 => -0.45,
            3 => 0,
            4 => 0.48,
            5 => 0.85,
            _ => -1.35
        };
        _gantryCarriage.Transform = new TranslateTransform3D(carriageX, 0, 5.5);
    }

    private void SetHPressTransform(double angle, double z)
    {
        var group = new Transform3DGroup();
        group.Children.Add(new RotateTransform3D(
            new AxisAngleRotation3D(new Vector3D(1, 0, 0), angle)));
        group.Children.Add(new TranslateTransform3D(0, 0, z));
        _hPress.Transform = group;
    }

    private void UpdateTelemetry(int stepIndex)
    {
        PowerText.Text = stepIndex == 4 ? $"{2000 + _random.Next(-24, 25)} W" : "0 W";
        SpeedText.Text = stepIndex == 4 ? $"{85 + _random.Next(-2, 3)} mm/s" : "0 mm/s";
        GasText.Text = $"{18 + (_random.NextDouble() - 0.5) * 0.4:F1} L/min";
        FocusText.Text = $"{(_random.NextDouble() - 0.5) * 0.06:+0.00;-0.00;0.00} mm";
        var seam = 99.4 + _random.NextDouble() * 0.5;
        SeamText.Text = $"{seam:F1}%";
        SeamBar.Value = seam;
        VisionOffsetText.Text = $"{0.012 + _random.NextDouble() * 0.012:F3} mm";
    }

    private void RunButton_Click(object sender, RoutedEventArgs e)
    {
        if (_fault) return;
        _running = !_running;
        RunButton.Content = _running ? "暂停产线" : "继续运行";
        ConnectionDot.Fill = _running ? Brushes.Turquoise : Brushes.Gold;
        ConnectionText.Text = _running ? "模拟 PLC 已连接" : "产线已暂停";
        AddEvent("操作", _running ? "产线继续运行" : "产线已人工暂停");
    }

    private void FaultButton_Click(object sender, RoutedEventArgs e)
    {
        _fault = true;
        _running = false;
        var active = Math.Max(0, _lastStep);
        Steps[active].State = StepState.Alarm;
        ConnectionDot.Fill = Brushes.OrangeRed;
        ConnectionText.Text = "焊接质量报警";
        JudgeBadge.Background = new SolidColorBrush(Color.FromRgb(127, 29, 29));
        JudgeText.Text = "NG";
        JudgeText.Foreground = new SolidColorBrush(Color.FromRgb(254, 202, 202));
        SeamText.Text = "82.4%";
        SeamBar.Value = 82.4;
        PowerText.Text = "864 W";
        AddEvent("报警", "ALM-504 激光功率偏差超限，产线安全停机");
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        _fault = false;
        _running = true;
        _elapsed = 0;
        _lastStep = -1;
        RunButton.Content = "暂停产线";
        ConnectionDot.Fill = Brushes.Turquoise;
        ConnectionText.Text = "模拟 PLC 已连接";
        JudgeBadge.Background = new SolidColorBrush(Color.FromRgb(20, 83, 45));
        JudgeText.Text = "OK";
        JudgeText.Foreground = new SolidColorBrush(Color.FromRgb(134, 239, 172));
        foreach (var step in Steps) step.State = StepState.Waiting;
        AddEvent("系统", "报警已清除，工艺循环复位");
    }

    private void StepList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (_updatingSelection) return;
        if (StepList.SelectedItem is not ProcessStep step) return;
        SceneTitle.Text = step.Name;
        _followCell = false;
        FollowButton.Content = "跟随电芯";
        Viewport.Camera?.LookAt(step.FocusPoint, 400);
    }

    private void FollowCellButton_Click(object sender, RoutedEventArgs e)
    {
        _followCell = !_followCell;
        FollowButton.Content = _followCell ? "停止跟随" : "跟随电芯";
        AddEvent("视图", _followCell ? "相机开始跟随当前流转电芯" : "相机停止跟随");
    }

    private void HomeViewButton_Click(object sender, RoutedEventArgs e)
    {
        _followCell = false;
        FollowButton.Content = "跟随电芯";
        SetHomeCamera();
    }

    private void WeldCloseViewButton_Click(object sender, RoutedEventArgs e)
    {
        _followCell = false;
        FollowButton.Content = "跟随电芯";
        Viewport.Camera = new PerspectiveCamera(
            new Point3D(9.5, -15.5, 8.5),
            new Vector3D(-9.5, 15.5, -5.7),
            new Vector3D(0, 0, 1),
            43);
        AddEvent("视图", "切换到密封钉压装与焊接机构特写");
    }

    private void ToggleLabelsButton_Click(object sender, RoutedEventArgs e)
    {
        _showSceneLabels = !_showSceneLabels;
        if (_showSceneLabels)
        {
            foreach (var label in _sceneLabels)
            {
                if (!Viewport.Children.Contains(label))
                    Viewport.Children.Add(label);
            }
        }
        else
        {
            foreach (var label in _sceneLabels)
                Viewport.Children.Remove(label);
        }

        LabelButton.Content = _showSceneLabels ? "隐藏标注" : "显示标注";
        AddEvent("视图", _showSceneLabels ? "已显示三维设备名称标注" : "已隐藏三维设备名称标注");
    }

    private void TopViewButton_Click(object sender, RoutedEventArgs e)
    {
        _followCell = false;
        FollowButton.Content = "跟随电芯";
        Viewport.Camera = new PerspectiveCamera(new Point3D(0, -0.01, 30),
            new Vector3D(0, 0, -30), new Vector3D(0, 1, 0), 50);
    }

    private void FrontViewButton_Click(object sender, RoutedEventArgs e)
    {
        _followCell = false;
        FollowButton.Content = "跟随电芯";
        Viewport.Camera = new PerspectiveCamera(new Point3D(0, -34, 11),
            new Vector3D(0, 34, -7.2), new Vector3D(0, 0, 1), 50);
    }

    private void SetHomeCamera() =>
        Viewport.Camera = new PerspectiveCamera(new Point3D(23, -35, 18),
            new Vector3D(-23, 35, -13.5), new Vector3D(0, 0, 1), 52);

    private void AddEvent(string level, string message)
    {
        Events.Insert(0, new EventRow(DateTime.Now.ToString("HH:mm:ss"), level, message));
        while (Events.Count > 5) Events.RemoveAt(Events.Count - 1);
    }

    private static double SmoothStep(double value)
    {
        value = Math.Clamp(value, 0, 1);
        return value * value * (3 - 2 * value);
    }

    private static Material Mat(byte r, byte g, byte b) =>
        new DiffuseMaterial(new SolidColorBrush(Color.FromRgb(r, g, b)));

    private static ModelVisual3D Box(Point3D center, double x, double y, double z, Material material)
    {
        var mesh = new MeshBuilder(false, false);
        mesh.AddBox(center, x, y, z);
        return Visual(mesh, material);
    }

    private static ModelVisual3D Cylinder(Point3D p1, Point3D p2, double diameter, Material material)
    {
        var mesh = new MeshBuilder(false, false);
        mesh.AddCylinder(p1, p2, diameter, 24);
        return Visual(mesh, material);
    }

    private static ModelVisual3D Sphere(Point3D center, double radius, Material material)
    {
        var mesh = new MeshBuilder(false, false);
        mesh.AddSphere(center, radius, 20, 12);
        return Visual(mesh, material);
    }

    private static ModelVisual3D Visual(MeshBuilder mesh, Material material)
    {
        var model = new GeometryModel3D(mesh.ToMesh(), material) { BackMaterial = material };
        return new ModelVisual3D { Content = model };
    }
}

public sealed record EventRow(string Time, string Level, string Message);
