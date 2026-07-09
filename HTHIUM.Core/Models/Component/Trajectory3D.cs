using HTHIUM.Core.Common;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace HTHIUM.Core.Models.Component
{
    /// <summary>
    /// 3D轨迹数据模型
    /// </summary>
    public class Trajectory3D : ModelBase
    {
        private string? id;
        private string? name;
        private Color color = Colors.Blue;
        private double thickness = 0.5;
        private bool isVisible = true;
        private bool isDynamic = false;
        private bool showAnimation = false;
        private bool showKeyPoints = true;

        public string? Id
        {
            get => id;
            set => SetProperty(ref id, value);
        }

        public string? Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public ObservableCollection<TrajectoryPoint3D> Points { get; set; } = new();

        public Color Color
        {
            get => color;
            set => SetProperty(ref color, value);
        }

        public double Thickness
        {
            get => thickness;
            set => SetProperty(ref thickness, value);
        }

        public bool IsVisible
        {
            get => isVisible;
            set => SetProperty(ref isVisible, value);
        }

        public bool IsDynamic
        {
            get => isDynamic;
            set => SetProperty(ref isDynamic, value);
        }

        public bool ShowAnimation
        {
            get => showAnimation;
            set => SetProperty(ref showAnimation, value);
        }

        public bool ShowKeyPoints
        {
            get => showKeyPoints;
            set => SetProperty(ref showKeyPoints, value);
        }

        public Dictionary<string, object> Metadata { get; set; } = new();
    }
}
