using HelixToolkit.Wpf;
using Microsoft.Win32;
using SUNWODA_SEVB.Component.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SUNWODA_SEVB.Component.UserControls
{
    /// <summary>
    /// ModelViewerControl.xaml 的交互逻辑
    /// </summary>
    public partial class ModelViewerControl : UserControl
    {
        private readonly Dictionary<Model3D, ModelInfo> _modelInfoMap = new Dictionary<Model3D, ModelInfo>();
        private readonly ModelImporter _modelImporter = new ModelImporter();

        public event EventHandler<ModelDoubleClickEventArgs>? ModelDoubleClicked;

        public ModelViewerControl()
        {
            InitializeComponent();
        }

        private void BtnLoadModel_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "3D Models|*.obj;*.stl;*.3ds;*.ply|" +
                        "OBJ Files|*.obj|" +
                        "STL Files|*.stl|" +
                        "3DS Files|*.3ds|" +
                        "PLY Files|*.ply|" +
                        "All Files|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var fileName in openFileDialog.FileNames)
                {
                    LoadModel(fileName);
                }
            }
        }

        public void LoadModel(string filePath, string? modelName = null)
        {
            try
            {
                // 加载模型
                var model3DGroup = _modelImporter.Load(filePath);

                if (model3DGroup != null)
                {
                    // 创建模型信息
                    var modelInfo = new ModelInfo(
                        modelName ?? System.IO.Path.GetFileNameWithoutExtension(filePath),
                        filePath
                    )
                    {
                        Model = model3DGroup
                    };

                    // 为模型创建一个随机颜色
                    var random = new Random();
                    var color = Color.FromRgb(
                        (byte)random.Next(100, 255),
                        (byte)random.Next(100, 255),
                        (byte)random.Next(100, 255)
                    );

                    // 创建材质
                    var material = new DiffuseMaterial(new SolidColorBrush(color));

                    // 应用材质到所有几何体
                    ApplyMaterialToModel(model3DGroup, material);

                    // 创建ModelVisual3D并添加到视口
                    var modelVisual = new ModelVisual3D
                    {
                        Content = model3DGroup
                    };

                    // 保存模型信息映射
                    _modelInfoMap[model3DGroup] = modelInfo;

                    // 添加到视口
                    viewport.Children.Add(modelVisual);

                    // 自动调整相机以显示所有模型
                    viewport.ZoomExtents(500);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"加载模型失败: {ex.Message}", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ApplyMaterialToModel(Model3D model, Material material)
        {
            if (model is Model3DGroup group)
            {
                foreach (var child in group.Children)
                {
                    ApplyMaterialToModel(child, material);
                }
            }
            else if (model is GeometryModel3D geometryModel)
            {
                geometryModel.Material = material;
                geometryModel.BackMaterial = material;
            }
        }

        private void Viewport_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(viewport);
            var hitResult = GetHitTestResult(point);

            if (hitResult != null)
            {
                // 查找被点击的模型
                var clickedModel = FindModelFromHitResult(hitResult);

                if (clickedModel != null && _modelInfoMap.TryGetValue(clickedModel, out var modelInfo))
                {
                    // 触发事件
                    ModelDoubleClicked?.Invoke(this, new ModelDoubleClickEventArgs(modelInfo));

                    // 显示模型信息
                    ShowModelInfo(modelInfo);
                }
            }
        }

        private RayMeshGeometry3DHitTestResult? GetHitTestResult(Point point)
        {
            var hitParams = new PointHitTestParameters(point);
            var resultList = new List<RayMeshGeometry3DHitTestResult>();

            VisualTreeHelper.HitTest(viewport, null, result =>
            {
                if (result is RayMeshGeometry3DHitTestResult rayResult)
                {
                    resultList.Add(rayResult);
                }
                return HitTestResultBehavior.Continue;
            }, hitParams);

            return resultList.OrderBy(r => r.DistanceToRayOrigin).FirstOrDefault();
        }

        private Model3D? FindModelFromHitResult(RayMeshGeometry3DHitTestResult hitResult)
        {
            var visual = hitResult.VisualHit;
            while (visual != null)
            {
                if (visual is ModelVisual3D modelVisual)
                {
                    // 查找包含此内容的顶级Model3D
                    foreach (var kvp in _modelInfoMap)
                    {
                        if (IsModelContainsGeometry(kvp.Key, hitResult.ModelHit))
                        {
                            return kvp.Key;
                        }
                    }
                }
                visual = VisualTreeHelper.GetParent(visual) as Visual3D;
            }
            return null;
        }

        private bool IsModelContainsGeometry(Model3D model, Model3D target)
        {
            if (model == target)
                return true;

            if (model is Model3DGroup group)
            {
                foreach (var child in group.Children)
                {
                    if (IsModelContainsGeometry(child, target))
                        return true;
                }
            }

            return false;
        }

        private void ShowModelInfo(ModelInfo modelInfo)
        {
            var message = $"模型信息:\n" +
                         $"名称: {modelInfo.Name}\n" +
                         $"文件: {modelInfo.FilePath}\n" +
                         $"ID: {modelInfo.Id}";

            MessageBox.Show(message, "模型信息", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            // 移除所有模型
            var modelsToRemove = viewport.Children.OfType<ModelVisual3D>()
                .Where(m => _modelInfoMap.ContainsKey(m.Content)).ToList();

            foreach (var model in modelsToRemove)
            {
                viewport.Children.Remove(model);
            }

            _modelInfoMap.Clear();
        }

        private void BtnResetView_Click(object sender, RoutedEventArgs e)
        {
            viewport.ResetCamera();
            viewport.ZoomExtents(500);
        }

        public IEnumerable<ModelInfo> GetLoadedModels()
        {
            return _modelInfoMap.Values;
        }

        public void RemoveModel(string modelId)
        {
            var modelToRemove = _modelInfoMap.FirstOrDefault(kvp => kvp.Value.Id == modelId);
            if (modelToRemove.Key != null)
            {
                var visualToRemove = viewport.Children.OfType<ModelVisual3D>()
                    .FirstOrDefault(m => m.Content == modelToRemove.Key);

                if (visualToRemove != null)
                {
                    viewport.Children.Remove(visualToRemove);
                }

                _modelInfoMap.Remove(modelToRemove.Key);
            }
        }
    }

    // 双击事件参数
    public class ModelDoubleClickEventArgs : EventArgs
    {
        public ModelInfo ModelInfo { get; }

        public ModelDoubleClickEventArgs(ModelInfo modelInfo)
        {
            ModelInfo = modelInfo;
        }
    }
}
