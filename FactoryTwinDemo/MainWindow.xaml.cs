using FactoryTwinDemo.Models;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;

namespace FactoryTwinDemo;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private readonly DispatcherTimer _timer;
    private readonly Random _random = new(2026);
    private readonly Dictionary<string, Material> _normalMaterials = [];
    private readonly List<ModelVisual3D> _products = [];
    private readonly List<double> _productPositions = [];
    private ModelVisual3D? _robotCarriage;
    private ModelVisual3D? _sealerHead;
    private bool _isRunning = true;
    private double _simulationTime;
    private int _totalOutput = 1248;
    private EquipmentItem? _selectedEquipment;

    public ObservableCollection<EquipmentItem> Equipment { get; } = [];
    public ObservableCollection<EventLogItem> EventLogs { get; } = [];

    public EquipmentItem? SelectedEquipment
    {
        get => _selectedEquipment;
        set
        {
            _selectedEquipment = value;
            OnPropertyChanged();
        }
    }

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
        BuildFactoryScene();
        SeedTelemetry();

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(100) };
        _timer.Tick += SimulationTick;
        _timer.Start();

        Loaded += (_, _) =>
        {
            SetHomeCamera();
            AddLog("系统", "三维产线模型加载完成，模拟 PLC 已连接");
        };
    }

    private void BuildFactoryScene()
    {
        Viewport.Children.Clear();
        Viewport.Children.Add(new SunLight());

        // 车间地面
        var floor = new BoxVisual3D
        {
            Center = new Point3D(0, 0, -0.18),
            Width = 32,
            Length = 15,
            Height = 0.3,
            Material = CreateMaterial(Color.FromRgb(35, 47, 61))
        };
        Viewport.Children.Add(floor);

        var grid = new GridLinesVisual3D
        {
            Center = new Point3D(0, 0, 0),
            Width = 32,
            Length = 15,
            MinorDistance = 1,
            MajorDistance = 5,
            Thickness = 0.01,
            Fill = new SolidColorBrush(Color.FromRgb(63, 82, 102))
        };
        Viewport.Children.Add(grid);

        AddSafetyFence();
        AddConveyor("CV-101", "上料输送机", -10.5, 5.5, Color.FromRgb(40, 130, 150));
        AddVisionStation();
        AddPickPlaceRobot();
        AddPackagingStation();
        AddConveyor("CV-102", "出料输送机", 7.5, 7.0, Color.FromRgb(40, 130, 150));
        AddPalletArea();
        AddProducts();
        AddLabels();
    }

    private void AddConveyor(string id, string name, double centerX, double length, Color color)
    {
        var equipment = new EquipmentItem
        {
            Id = id,
            Name = name,
            Category = "皮带输送设备",
            FocusPoint = new Point3D(centerX, 0, 1.2),
            Status = EquipmentStatus.Running
        };

        var root = new ModelVisual3D();
        var frameMaterial = CreateMaterial(Color.FromRgb(105, 120, 135));
        var beltMaterial = CreateMaterial(color);
        var darkMaterial = CreateMaterial(Color.FromRgb(30, 40, 50));

        root.Children.Add(CreateBox(new Point3D(centerX, 0, 1.05), length, 2.15, 0.18, beltMaterial, equipment));
        root.Children.Add(CreateBox(new Point3D(centerX, -1.08, 0.96), length, 0.14, 0.36, frameMaterial));
        root.Children.Add(CreateBox(new Point3D(centerX, 1.08, 0.96), length, 0.14, 0.36, frameMaterial));

        for (double x = centerX - length / 2 + 0.45; x <= centerX + length / 2; x += 1.1)
        {
            root.Children.Add(CreateCylinder(new Point3D(x, -0.96, 1.15), new Point3D(x, 0.96, 1.15),
                0.13, darkMaterial));
        }

        foreach (double x in new[] { centerX - length / 2 + 0.4, centerX + length / 2 - 0.4 })
        {
            foreach (double y in new[] { -0.88, 0.88 })
            {
                root.Children.Add(CreateBox(new Point3D(x, y, 0.5), 0.16, 0.16, 1.0, frameMaterial));
                root.Children.Add(CreateBox(new Point3D(x, y, 0.05), 0.38, 0.38, 0.08, darkMaterial));
            }
        }

        Viewport.Children.Add(root);
        Equipment.Add(equipment);
        _normalMaterials[id] = beltMaterial;
    }

    private void AddVisionStation()
    {
        var equipment = new EquipmentItem
        {
            Id = "VS-201",
            Name = "视觉检测工位",
            Category = "机器视觉检测",
            FocusPoint = new Point3D(-6.2, 0, 2.2),
            Status = EquipmentStatus.Running
        };
        var root = new ModelVisual3D();
        var metal = CreateMaterial(Color.FromRgb(125, 140, 155));
        var stateMaterial = CreateMaterial(Color.FromRgb(45, 212, 191));
        var cameraMaterial = CreateMaterial(Color.FromRgb(32, 43, 58));

        foreach (double y in new[] { -1.25, 1.25 })
            root.Children.Add(CreateBox(new Point3D(-6.2, y, 2.1), 0.18, 0.18, 2.2, metal));
        root.Children.Add(CreateBox(new Point3D(-6.2, 0, 3.15), 0.24, 2.7, 0.22, metal));
        root.Children.Add(CreateBox(new Point3D(-6.2, 0, 2.72), 0.55, 0.55, 0.5, cameraMaterial));
        root.Children.Add(CreateCylinder(new Point3D(-6.2, 0, 2.5), new Point3D(-6.2, 0, 2.2),
            0.16, stateMaterial, equipment));

        Viewport.Children.Add(root);
        Equipment.Add(equipment);
        _normalMaterials[equipment.Id] = stateMaterial;
    }

    private void AddPickPlaceRobot()
    {
        var equipment = new EquipmentItem
        {
            Id = "RB-301",
            Name = "高速装箱机器人",
            Category = "三轴拾放机器人",
            FocusPoint = new Point3D(-1.8, 0, 2.2),
            Status = EquipmentStatus.Running
        };
        var root = new ModelVisual3D();
        var frame = CreateMaterial(Color.FromRgb(230, 232, 235));
        var accent = CreateMaterial(Color.FromRgb(249, 115, 22));
        var joint = CreateMaterial(Color.FromRgb(45, 55, 68));

        foreach (double x in new[] { -3.5, 0.0 })
        foreach (double y in new[] { -1.65, 1.65 })
            root.Children.Add(CreateBox(new Point3D(x, y, 1.85), 0.22, 0.22, 3.7, frame));

        root.Children.Add(CreateBox(new Point3D(-1.75, -1.65, 3.65), 3.8, 0.25, 0.28, frame));
        root.Children.Add(CreateBox(new Point3D(-1.75, 1.65, 3.65), 3.8, 0.25, 0.28, frame));
        root.Children.Add(CreateBox(new Point3D(-1.75, 0, 3.65), 0.5, 3.55, 0.34, accent, equipment));

        _robotCarriage = new ModelVisual3D();
        _robotCarriage.Children.Add(CreateBox(new Point3D(0, 0, 0), 0.72, 0.72, 0.4, joint));
        _robotCarriage.Children.Add(CreateCylinder(new Point3D(0, 0, -0.1), new Point3D(0, 0, -1.6),
            0.18, accent));
        _robotCarriage.Children.Add(CreateBox(new Point3D(0, 0, -1.7), 0.9, 0.7, 0.16, joint));
        root.Children.Add(_robotCarriage);

        Viewport.Children.Add(root);
        Equipment.Add(equipment);
        _normalMaterials[equipment.Id] = accent;
    }

    private void AddPackagingStation()
    {
        var equipment = new EquipmentItem
        {
            Id = "PK-401",
            Name = "自动封装机",
            Category = "食品包装设备",
            FocusPoint = new Point3D(3.0, 0, 2.0),
            Status = EquipmentStatus.Running
        };
        var root = new ModelVisual3D();
        var metal = CreateMaterial(Color.FromRgb(190, 200, 210));
        var glass = new DiffuseMaterial(new SolidColorBrush(Color.FromArgb(95, 56, 189, 248)));
        var accent = CreateMaterial(Color.FromRgb(45, 212, 191));

        root.Children.Add(CreateBox(new Point3D(3.0, 0, 0.75), 3.7, 3.2, 0.25, metal));
        foreach (double x in new[] { 1.35, 4.65 })
        foreach (double y in new[] { -1.42, 1.42 })
            root.Children.Add(CreateBox(new Point3D(x, y, 2.0), 0.16, 0.16, 2.6, metal));
        root.Children.Add(CreateBox(new Point3D(3.0, -1.48, 2.8), 3.55, 0.08, 1.55, glass));
        root.Children.Add(CreateBox(new Point3D(3.0, 1.48, 2.8), 3.55, 0.08, 1.55, glass));
        root.Children.Add(CreateBox(new Point3D(1.3, 0, 2.8), 0.08, 2.9, 1.55, glass));
        root.Children.Add(CreateBox(new Point3D(4.7, 0, 2.8), 0.08, 2.9, 1.55, glass));
        root.Children.Add(CreateBox(new Point3D(3.0, 0, 3.65), 3.7, 3.2, 0.18, metal));
        root.Children.Add(CreateBox(new Point3D(4.78, -1.15, 2.4), 0.18, 0.65, 0.75, accent, equipment));

        _sealerHead = new ModelVisual3D();
        _sealerHead.Children.Add(CreateBox(new Point3D(0, 0, 0), 1.15, 1.7, 0.25, accent));
        root.Children.Add(_sealerHead);

        Viewport.Children.Add(root);
        Equipment.Add(equipment);
        _normalMaterials[equipment.Id] = accent;
    }

    private void AddPalletArea()
    {
        var equipment = new EquipmentItem
        {
            Id = "PL-501",
            Name = "成品缓存区",
            Category = "码垛与物流",
            FocusPoint = new Point3D(12, 0, 1),
            Status = EquipmentStatus.Standby
        };
        var root = new ModelVisual3D();
        var wood = CreateMaterial(Color.FromRgb(153, 101, 55));
        var box = CreateMaterial(Color.FromRgb(202, 153, 93));

        for (var row = 0; row < 2; row++)
        for (var col = 0; col < 3; col++)
        {
            root.Children.Add(CreateBox(new Point3D(11.5 + col * 0.85, -0.55 + row * 1.1, 0.25),
                0.75, 0.9, 0.12, wood));
            root.Children.Add(CreateBox(new Point3D(11.5 + col * 0.85, -0.55 + row * 1.1, 0.75),
                0.7, 0.82, 0.82, box, equipment));
        }

        Viewport.Children.Add(root);
        Equipment.Add(equipment);
        _normalMaterials[equipment.Id] = box;
    }

    private void AddProducts()
    {
        for (var i = 0; i < 9; i++)
        {
            var product = new ModelVisual3D();
            var material = i % 2 == 0
                ? CreateMaterial(Color.FromRgb(250, 204, 21))
                : CreateMaterial(Color.FromRgb(74, 222, 128));
            product.Children.Add(CreateBox(new Point3D(0, 0, 0), 0.65, 0.8, 0.52, material));
            _products.Add(product);
            _productPositions.Add(-13.0 + i * 2.55);
            Viewport.Children.Add(product);
        }
    }

    private void AddSafetyFence()
    {
        var yellow = CreateMaterial(Color.FromRgb(234, 179, 8));
        var fence = new ModelVisual3D();
        foreach (double x in new[] { -4.0, 0.5, 5.5 })
        foreach (double y in new[] { -2.35, 2.35 })
            fence.Children.Add(CreateBox(new Point3D(x, y, 1.0), 0.1, 0.1, 2.0, yellow));

        foreach (double y in new[] { -2.35, 2.35 })
        {
            fence.Children.Add(CreateBox(new Point3D(-1.75, y, 1.75), 4.5, 0.06, 0.08, yellow));
            fence.Children.Add(CreateBox(new Point3D(3.0, y, 1.75), 5.0, 0.06, 0.08, yellow));
        }
        Viewport.Children.Add(fence);
    }

    private void AddLabels()
    {
        var labels = new[]
        {
            ("上料", new Point3D(-10.5, -1.7, 1.9)),
            ("视觉检测", new Point3D(-6.2, -1.7, 3.35)),
            ("机器人装箱", new Point3D(-1.8, -2.0, 4.05)),
            ("自动封装", new Point3D(3.0, -1.9, 4.05)),
            ("成品输出", new Point3D(8.0, -1.7, 1.9))
        };

        foreach (var (text, position) in labels)
        {
            Viewport.Children.Add(new BillboardTextVisual3D
            {
                Text = text,
                Position = position,
                Foreground = Brushes.White,
                Background = new SolidColorBrush(Color.FromArgb(190, 15, 35, 52)),
                FontSize = 14,
                Padding = new Thickness(7, 3, 7, 3)
            });
        }
    }

    private void SeedTelemetry()
    {
        foreach (var item in Equipment)
        {
            item.Speed = item.Category.Contains("输送") ? 12.5 : 6.0;
            item.Temperature = 30 + _random.NextDouble() * 8;
            item.Processed = _random.Next(900, 1600);
        }

        SelectedEquipment = Equipment.FirstOrDefault();
        EquipmentList.SelectedIndex = 0;
    }

    private void SimulationTick(object? sender, EventArgs e)
    {
        ClockText.Text = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss");
        if (!_isRunning) return;

        _simulationTime += 0.1;
        var lineAlarm = Equipment.Any(x => x.Status == EquipmentStatus.Alarm);
        var lineSpeed = lineAlarm ? 0.0 : 1.0;

        // 产品沿 X 方向循环运动
        for (var i = 0; i < _products.Count; i++)
        {
            _productPositions[i] += 0.065 * lineSpeed;
            if (_productPositions[i] > 10.8)
            {
                _productPositions[i] = -13.2;
                _totalOutput++;
                if (_totalOutput % 8 == 0)
                    AddLog("生产", $"成品累计产量达到 {_totalOutput:N0} 件");
            }

            _products[i].Transform = new TranslateTransform3D(_productPositions[i], 0, 1.52);
        }

        if (_robotCarriage is not null)
        {
            var x = -1.75 + Math.Sin(_simulationTime * 1.35) * 1.2 * lineSpeed;
            var lift = 3.7 + Math.Abs(Math.Cos(_simulationTime * 1.35)) * 0.35;
            _robotCarriage.Transform = new TranslateTransform3D(x, 0, lift);
        }

        if (_sealerHead is not null)
        {
            var z = 2.55 + Math.Abs(Math.Sin(_simulationTime * 1.8)) * 0.5 * lineSpeed;
            _sealerHead.Transform = new TranslateTransform3D(3.0, 0, z);
        }

        foreach (var item in Equipment)
        {
            if (item.Status == EquipmentStatus.Alarm)
            {
                item.Speed = 0;
                item.Temperature = Math.Min(78, item.Temperature + 0.04);
            }
            else
            {
                item.Speed = item.Status == EquipmentStatus.Running
                    ? Math.Max(2, item.Speed + (_random.NextDouble() - 0.5) * 0.18)
                    : 0;
                item.Temperature += (_random.NextDouble() - 0.48) * 0.08;
                if (_random.NextDouble() < 0.035) item.Processed++;
            }
        }

        OutputText.Text = _totalOutput.ToString("N0");
        OeeText.Text = lineAlarm ? "62.4%" : $"{87.4 + Math.Sin(_simulationTime / 8) * 1.2:F1}%";
        CycleText.Text = lineAlarm ? "--" : $"{3.15 + Math.Sin(_simulationTime / 4) * 0.12:F2} s";
        AlarmCountText.Text = Equipment.Count(x => x.Status == EquipmentStatus.Alarm).ToString();
        DataQualityBar.Value = 97.5 + Math.Sin(_simulationTime / 3) * 1.2;
    }

    private void SetEquipmentVisualState(EquipmentItem item)
    {
        Material material = item.Status switch
        {
            EquipmentStatus.Alarm => CreateMaterial(Color.FromRgb(239, 68, 68)),
            EquipmentStatus.Standby => CreateMaterial(Color.FromRgb(250, 204, 21)),
            EquipmentStatus.Offline => CreateMaterial(Color.FromRgb(100, 116, 139)),
            _ => _normalMaterials.GetValueOrDefault(item.Id, CreateMaterial(Color.FromRgb(45, 212, 191)))
        };

        foreach (var model in item.StateModels)
        {
            model.Material = material;
            model.BackMaterial = material;
        }
    }

    private void RunButton_Click(object sender, RoutedEventArgs e)
    {
        _isRunning = !_isRunning;
        RunButton.Content = _isRunning ? "暂停" : "继续";
        AddLog("操作", _isRunning ? "产线仿真已继续" : "产线仿真已暂停");
    }

    private void InjectFaultButton_Click(object sender, RoutedEventArgs e)
    {
        var target = Equipment.First(x => x.Id == "PK-401");
        target.Status = EquipmentStatus.Alarm;
        SetEquipmentVisualState(target);
        SelectedEquipment = target;
        EquipmentList.SelectedItem = target;
        AddLog("告警", "PK-401 自动封装机：热封温度超上限，产线联锁停止");
    }

    private void ResetButton_Click(object sender, RoutedEventArgs e)
    {
        foreach (var item in Equipment)
        {
            item.Status = item.Id == "PL-501" ? EquipmentStatus.Standby : EquipmentStatus.Running;
            item.Temperature = 31 + _random.NextDouble() * 5;
            SetEquipmentVisualState(item);
        }
        AddLog("操作", "设备故障已复位，产线恢复自动运行");
    }

    private void ImportCadButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "导入三维 CAD 网格模型",
            Filter = "三维模型 (*.obj;*.stl;*.3ds;*.lwo;*.off)|*.obj;*.stl;*.3ds;*.lwo;*.off|所有文件 (*.*)|*.*"
        };
        if (dialog.ShowDialog(this) != true) return;

        try
        {
            var importer = new ModelImporter();
            var model = importer.Load(dialog.FileName);
            var visual = new ModelVisual3D
            {
                Content = model,
                Transform = new TranslateTransform3D(0, 4.5, 0.1)
            };
            Viewport.Children.Add(visual);
            Viewport.ZoomExtents(500);
            AddLog("CAD", $"已导入模型：{System.IO.Path.GetFileName(dialog.FileName)}");
        }
        catch (Exception ex)
        {
            MessageBox.Show(this, $"模型导入失败：\n{ex.Message}", "导入 CAD",
                MessageBoxButton.OK, MessageBoxImage.Warning);
            AddLog("错误", $"CAD 模型导入失败：{ex.Message}");
        }
    }

    private void EquipmentList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (EquipmentList.SelectedItem is not EquipmentItem item) return;
        SelectedEquipment = item;
        Viewport.Camera.LookAt(item.FocusPoint, 450);
    }

    private void HomeViewButton_Click(object sender, RoutedEventArgs e) => SetHomeCamera();

    private void TopViewButton_Click(object sender, RoutedEventArgs e)
    {
        Viewport.Camera = new PerspectiveCamera(new Point3D(0, -0.01, 28),
            new Vector3D(0, 0, -28), new Vector3D(0, 1, 0), 42);
    }

    private void FrontViewButton_Click(object sender, RoutedEventArgs e)
    {
        Viewport.Camera = new PerspectiveCamera(new Point3D(0, -24, 7),
            new Vector3D(0, 24, -4.5), new Vector3D(0, 0, 1), 42);
    }

    private void SetHomeCamera()
    {
        Viewport.Camera = new PerspectiveCamera(new Point3D(16, -21, 11),
            new Vector3D(-16, 21, -8.2), new Vector3D(0, 0, 1), 40);
    }

    private void AddLog(string level, string message)
    {
        EventLogs.Insert(0, new EventLogItem(DateTime.Now, level, message));
        while (EventLogs.Count > 8) EventLogs.RemoveAt(EventLogs.Count - 1);
    }

    private static ModelVisual3D CreateBox(Point3D center, double x, double y, double z,
        Material material, EquipmentItem? stateOwner = null)
    {
        var builder = new MeshBuilder(false, false);
        builder.AddBox(center, x, y, z);
        return CreateMeshVisual(builder, material, stateOwner);
    }

    private static ModelVisual3D CreateCylinder(Point3D p1, Point3D p2, double diameter,
        Material material, EquipmentItem? stateOwner = null)
    {
        var builder = new MeshBuilder(false, false);
        builder.AddCylinder(p1, p2, diameter, 24);
        return CreateMeshVisual(builder, material, stateOwner);
    }

    private static ModelVisual3D CreateMeshVisual(MeshBuilder builder, Material material,
        EquipmentItem? stateOwner)
    {
        var model = new GeometryModel3D(builder.ToMesh(), material) { BackMaterial = material };
        stateOwner?.StateModels.Add(model);
        return new ModelVisual3D { Content = model };
    }

    private static Material CreateMaterial(Color color) =>
        new DiffuseMaterial(new SolidColorBrush(color));

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
