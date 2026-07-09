using System.Windows.Media.Media3D;

namespace HTHIUM.Core.Models.Component
{
    /// <summary>
    /// 轨迹点数据
    /// </summary>
    public class TrajectoryPoint3D
    {
        public Point3D Position { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsKeyPoint { get; set; }
        public string? Label { get; set; }
        public Dictionary<string, object> Attributes { get; set; } = new();
    }
}
