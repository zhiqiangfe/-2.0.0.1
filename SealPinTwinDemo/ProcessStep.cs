using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Media3D;

namespace SealPinTwinDemo;

public sealed class ProcessStep : INotifyPropertyChanged
{
    private StepState _state;

    public required string Code { get; init; }
    public required string Name { get; init; }
    public required string Device { get; init; }
    public required Point3D FocusPoint { get; init; }

    public StepState State
    {
        get => _state;
        set
        {
            if (_state == value) return;
            _state = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(StateText));
            OnPropertyChanged(nameof(StateBrush));
        }
    }

    public string StateText => State switch
    {
        StepState.Working => "执行中",
        StepState.Done => "已完成",
        StepState.Alarm => "报警",
        _ => "等待"
    };

    public Brush StateBrush => State switch
    {
        StepState.Working => new SolidColorBrush(Color.FromRgb(38, 217, 199)),
        StepState.Done => new SolidColorBrush(Color.FromRgb(96, 165, 250)),
        StepState.Alarm => new SolidColorBrush(Color.FromRgb(248, 113, 113)),
        _ => new SolidColorBrush(Color.FromRgb(100, 116, 139))
    };

    public event PropertyChangedEventHandler? PropertyChanged;
    private void OnPropertyChanged([CallerMemberName] string? name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}

public enum StepState
{
    Waiting,
    Working,
    Done,
    Alarm
}
