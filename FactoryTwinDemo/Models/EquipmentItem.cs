using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace FactoryTwinDemo.Models;

public enum EquipmentStatus
{
    Running,
    Standby,
    Alarm,
    Offline
}

public sealed class EquipmentItem : INotifyPropertyChanged
{
    private EquipmentStatus _status;
    private double _speed;
    private double _temperature;
    private int _processed;

    public required string Id { get; init; }
    public required string Name { get; init; }
    public required string Category { get; init; }
    public required Point3D FocusPoint { get; init; }
    public List<GeometryModel3D> StateModels { get; } = [];

    public EquipmentStatus Status
    {
        get => _status;
        set
        {
            if (_status == value) return;
            _status = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(StatusText));
            OnPropertyChanged(nameof(StatusBrush));
        }
    }

    public double Speed
    {
        get => _speed;
        set { _speed = value; OnPropertyChanged(); OnPropertyChanged(nameof(SpeedText)); }
    }

    public double Temperature
    {
        get => _temperature;
        set { _temperature = value; OnPropertyChanged(); OnPropertyChanged(nameof(TemperatureText)); }
    }

    public int Processed
    {
        get => _processed;
        set { _processed = value; OnPropertyChanged(); }
    }

    public string StatusText => Status switch
    {
        EquipmentStatus.Running => "运行",
        EquipmentStatus.Standby => "待机",
        EquipmentStatus.Alarm => "告警",
        _ => "离线"
    };

    public Brush StatusBrush => Status switch
    {
        EquipmentStatus.Running => new SolidColorBrush(Color.FromRgb(45, 212, 191)),
        EquipmentStatus.Standby => new SolidColorBrush(Color.FromRgb(250, 204, 21)),
        EquipmentStatus.Alarm => new SolidColorBrush(Color.FromRgb(248, 113, 113)),
        _ => new SolidColorBrush(Color.FromRgb(100, 116, 139))
    };

    public string SpeedText => $"{Speed:F1} m/min";
    public string TemperatureText => $"{Temperature:F1} °C";

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
