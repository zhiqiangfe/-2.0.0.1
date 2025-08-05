using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using SUNWODA_SEVB.Core.Models.Component;

namespace SUNWODA_SEVB.Component.UserControls
{
    /// <summary>
    /// ModelViewerControl.xaml 的交互逻辑
    /// </summary>
    public partial class ModelViewerControl : UserControl
    {
        private readonly Dictionary<Model3D, ModelInfo> _modelInfoMap =
            new Dictionary<Model3D, ModelInfo>();
        private readonly Dictionary<string, ModelVisual3D> _modelVisualMap =
            new Dictionary<string, ModelVisual3D>();
        private readonly Dictionary<string, Point3D> _modelCenterMap =
            new Dictionary<string, Point3D>(); // 存储模型中心点
        private readonly ModelImporter _modelImporter = new ModelImporter();

        private BoundingBoxVisual3D? _selectionBoundingBox;
        private Model3D? _highlightedModel;
        private Material? _originalMaterial;
        private Model3D? _leftClickedModel;

        public event EventHandler<ModelDoubleClickEventArgs>? ModelDoubleClicked;

        public ModelViewerControl()
        {
            InitializeComponent();
        }

        private void BtnLoadModel_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter =
                    "3D Models|*.obj;*.stl;*.3ds;*.ply|"
                    + "OBJ Files|*.obj|"
                    + "STL Files|*.stl|"
                    + "3DS Files|*.3ds|"
                    + "PLY Files|*.ply|"
                    + "All Files|*.*",
                Multiselect = true,
            };

            if (openFileDialog.ShowDialog() == true)
            {
                // 获取位置、旋转和缩放值
                var position = GetPositionFromUI();
                var rotation = GetRotationFromUI();
                var scale = GetScaleFromUI();

                foreach (var fileName in openFileDialog.FileNames)
                {
                    LoadModel(fileName, position: position, rotation: rotation, scale: scale);
                }
            }
        }

        private Vector3D GetPositionFromUI()
        {
            double x = 0,
                y = 0,
                z = 0;
            double.TryParse(TxtPosX.Text, out x);
            double.TryParse(TxtPosY.Text, out y);
            double.TryParse(TxtPosZ.Text, out z);
            return new Vector3D(x, y, z);
        }

        private Vector3D GetRotationFromUI()
        {
            double x = 0,
                y = 0,
                z = 0;
            double.TryParse(TxtRotX.Text, out x);
            double.TryParse(TxtRotY.Text, out y);
            double.TryParse(TxtRotZ.Text, out z);
            return new Vector3D(x, y, z);
        }

        private Vector3D GetScaleFromUI()
        {
            double scale = 1;
            double.TryParse(TxtScale.Text, out scale);
            return new Vector3D(scale, scale, scale);
        }

        // 计算模型的边界框和中心点
        private (Rect3D bounds, Point3D center) CalculateModelBounds(Model3D model)
        {
            var bounds = new Rect3D();
            var meshes = new List<MeshGeometry3D>();

            // 递归收集所有网格
            CollectMeshes(model, meshes);

            if (meshes.Count > 0)
            {
                // 初始化边界框
                var firstMesh = meshes[0];
                if (firstMesh.Positions.Count > 0)
                {
                    var firstPoint = firstMesh.Positions[0];
                    bounds = new Rect3D(firstPoint, new Size3D(0, 0, 0));

                    // 扩展边界框以包含所有顶点
                    foreach (var mesh in meshes)
                    {
                        foreach (var position in mesh.Positions)
                        {
                            bounds.Union(position);
                        }
                    }
                }
            }

            // 计算中心点
            var center = new Point3D(
                bounds.X + bounds.SizeX / 2,
                bounds.Y + bounds.SizeY / 2,
                bounds.Z + bounds.SizeZ / 2
            );

            return (bounds, center);
        }

        // 递归收集所有网格
        private void CollectMeshes(Model3D model, List<MeshGeometry3D> meshes)
        {
            if (model is Model3DGroup group)
            {
                foreach (var child in group.Children)
                {
                    CollectMeshes(child, meshes);
                }
            }
            else if (model is GeometryModel3D geometryModel)
            {
                if (geometryModel.Geometry is MeshGeometry3D mesh)
                {
                    meshes.Add(mesh);
                }
            }
        }

        // 创建围绕模型中心的变换
        private Transform3DGroup CreateModelTransform(
            Point3D modelCenter,
            Vector3D? position,
            Vector3D? rotation,
            Vector3D? scale
        )
        {
            var transformGroup = new Transform3DGroup();

            // 1. 先将模型中心移动到原点
            transformGroup.Children.Add(
                new TranslateTransform3D(-modelCenter.X, -modelCenter.Y, -modelCenter.Z)
            );

            // 2. 应用缩放（在原点处）
            if (scale.HasValue)
            {
                transformGroup.Children.Add(new ScaleTransform3D(scale.Value));
            }

            // 3. 应用旋转（围绕原点，即模型中心）
            if (rotation.HasValue)
            {
                // X轴旋转
                if (rotation.Value.X != 0)
                {
                    transformGroup.Children.Add(
                        new RotateTransform3D(
                            new AxisAngleRotation3D(new Vector3D(1, 0, 0), rotation.Value.X)
                        )
                    );
                }
                // Y轴旋转
                if (rotation.Value.Y != 0)
                {
                    transformGroup.Children.Add(
                        new RotateTransform3D(
                            new AxisAngleRotation3D(new Vector3D(0, 1, 0), rotation.Value.Y)
                        )
                    );
                }
                // Z轴旋转
                if (rotation.Value.Z != 0)
                {
                    transformGroup.Children.Add(
                        new RotateTransform3D(
                            new AxisAngleRotation3D(new Vector3D(0, 0, 1), rotation.Value.Z)
                        )
                    );
                }
            }

            // 4. 将模型移回原位置
            transformGroup.Children.Add(
                new TranslateTransform3D(modelCenter.X, modelCenter.Y, modelCenter.Z)
            );

            // 5. 应用用户指定的位移
            if (position.HasValue)
            {
                transformGroup.Children.Add(new TranslateTransform3D(position.Value));
            }

            return transformGroup;
        }

        public void LoadModel(
            string filePath,
            string? modelName = null,
            Vector3D? position = null,
            Vector3D? rotation = null,
            Vector3D? scale = null
        )
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
                        Model = model3DGroup,
                    };

                    // 计算模型的边界和中心
                    var (bounds, center) = CalculateModelBounds(model3DGroup);
                    _modelCenterMap[modelInfo.Id!] = center;

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

                    // 创建围绕模型中心的变换
                    model3DGroup.Transform = CreateModelTransform(
                        center,
                        position,
                        rotation,
                        scale
                    );

                    // 创建ModelVisual3D并添加到视口
                    var modelVisual = new ModelVisual3D { Content = model3DGroup };

                    // 保存模型信息映射
                    _modelInfoMap[model3DGroup] = modelInfo;
                    _modelVisualMap[modelInfo.Id!] = modelVisual;

                    // 添加到视口
                    viewport.Children.Add(modelVisual);

                    // 自动调整相机以显示所有模型
                    viewport.ZoomExtents(500);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"加载模型失败: {ex.Message}",
                    "错误",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
        }

        // 添加更新模型位置的方法
        public void UpdateModelTransform(
            string modelId,
            Vector3D? position = null,
            Vector3D? rotation = null,
            Vector3D? scale = null
        )
        {
            if (
                _modelVisualMap.TryGetValue(modelId, out var modelVisual)
                && _modelCenterMap.TryGetValue(modelId, out var modelCenter)
            )
            {
                var model = modelVisual.Content as Model3DGroup;
                if (model != null)
                {
                    // 使用模型中心创建新的变换
                    model.Transform = CreateModelTransform(modelCenter, position, rotation, scale);
                }
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

                if (
                    clickedModel != null
                    && _modelInfoMap.TryGetValue(clickedModel, out var modelInfo)
                )
                {
                    // 触发事件
                    ModelDoubleClicked?.Invoke(this, new ModelDoubleClickEventArgs(modelInfo));

                    // 高亮模型
                    HighlightModel(modelInfo.Id!);

                    // 显示模型信息
                    ShowModelInfo(modelInfo);
                }
            }
        }

        private RayMeshGeometry3DHitTestResult? GetHitTestResult(Point point)
        {
            var hitParams = new PointHitTestParameters(point);
            var resultList = new List<RayMeshGeometry3DHitTestResult>();

            VisualTreeHelper.HitTest(
                viewport,
                null,
                result =>
                {
                    if (result is RayMeshGeometry3DHitTestResult rayResult)
                    {
                        resultList.Add(rayResult);
                    }
                    return HitTestResultBehavior.Continue;
                },
                hitParams
            );

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
            // 获取模型中心点信息
            var centerInfo = "";
            if (_modelCenterMap.TryGetValue(modelInfo.Id!, out var originalCenter))
            {
                // 获取变换后的中心点
                var transformedCenter = originalCenter;

                if (_modelVisualMap.TryGetValue(modelInfo.Id!, out var modelVisual))
                {
                    var model = modelVisual.Content;
                    if (model?.Transform != null && model.Transform != Transform3D.Identity)
                    {
                        // 应用变换到中心点
                        transformedCenter = model.Transform.Transform(originalCenter);
                    }
                }

                centerInfo =
                    $"\n原始中心点: ({originalCenter.X:F2}, {originalCenter.Y:F2}, {originalCenter.Z:F2})\n"
                    + $"当前中心点: ({transformedCenter.X:F2}, {transformedCenter.Y:F2}, {transformedCenter.Z:F2})";
            }

            var message =
                $"模型信息:\n"
                + $"名称: {modelInfo.Name}\n"
                + $"文件: {modelInfo.FilePath}\n"
                + $"ID: {modelInfo.Id}"
                + centerInfo;

            MessageBox.Show(message, "模型信息", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private Material GetModelMaterial(Model3D model)
        {
            if (model is Model3DGroup group)
            {
                foreach (var child in group.Children)
                {
                    return GetModelMaterial(child);
                }
            }
            else if (model is GeometryModel3D geometryModel)
            {
                return geometryModel.Material;
            }

            // 为模型创建一个随机颜色
            var random = new Random();
            var color = Color.FromRgb(
                (byte)random.Next(100, 255),
                (byte)random.Next(100, 255),
                (byte)random.Next(100, 255)
            );

            return new DiffuseMaterial(new SolidColorBrush(color));
        }

        private void HighlightModel(string modelId)
        {
            // 恢复之前高亮的模型
            if (_highlightedModel != null && _originalMaterial != null)
            {
                ApplyMaterialToModel(_highlightedModel, _originalMaterial);
                _highlightedModel = null;
                _originalMaterial = null;
            }

            if (_modelVisualMap.TryGetValue(modelId, out var modelVisual))
            {
                _highlightedModel = modelVisual.Content;
                _originalMaterial = GetModelMaterial(_highlightedModel);

                // 创建高亮材质
                var highlightMaterial = new DiffuseMaterial(new SolidColorBrush(Colors.Yellow))
                {
                    AmbientColor = Colors.Yellow,
                };

                ApplyMaterialToModel(modelVisual.Content, highlightMaterial);

                // 3秒后自动恢复
                var timer = new System.Windows.Threading.DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(3),
                };
                timer.Tick += (s, e) =>
                {
                    timer.Stop();
                    ApplyMaterialToModel(_highlightedModel, _originalMaterial);
                    _highlightedModel = null;
                    _originalMaterial = null;
                };
                timer.Start();
            }
        }

        private void BtnClearAll_Click(object sender, RoutedEventArgs e)
        {
            // 移除边界框
            if (_selectionBoundingBox != null)
            {
                viewport.Children.Remove(_selectionBoundingBox);
                _selectionBoundingBox = null;
            }

            // 移除所有模型
            var modelsToRemove = viewport
                .Children.OfType<ModelVisual3D>()
                .Where(m => _modelInfoMap.ContainsKey(m.Content))
                .ToList();

            foreach (var model in modelsToRemove)
            {
                viewport.Children.Remove(model);
            }

            _modelInfoMap.Clear();
            _modelVisualMap.Clear();
            _modelCenterMap.Clear();
            _leftClickedModel = null;
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
                // 如果删除的是当前选中的模型，移除边界框
                if (_leftClickedModel != null)
                {
                    if (_modelInfoMap.TryGetValue(_leftClickedModel, out var modelInfo))
                    {
                        if (modelToRemove.Value.Id == modelInfo.Id)
                        {
                            if (_selectionBoundingBox != null)
                            {
                                viewport.Children.Remove(_selectionBoundingBox);
                                _selectionBoundingBox = null;
                            }
                        }
                    }
                }

                var visualToRemove = viewport
                    .Children.OfType<ModelVisual3D>()
                    .FirstOrDefault(m => m.Content == modelToRemove.Key);

                if (visualToRemove != null)
                {
                    viewport.Children.Remove(visualToRemove);
                }

                _modelInfoMap.Remove(modelToRemove.Key);
                _modelVisualMap.Remove(modelId);
                _modelCenterMap.Remove(modelId);
            }
        }

        private void Viewport_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(viewport);
            var hitResult = GetHitTestResult(point);

            // 先移除之前的边界框
            if (_selectionBoundingBox != null)
            {
                viewport.Children.Remove(_selectionBoundingBox);
                _selectionBoundingBox = null;
            }

            if (hitResult != null)
            {
                // 查找被点击的模型
                _leftClickedModel = FindModelFromHitResult(hitResult);

                if (
                    _leftClickedModel != null
                    && _modelInfoMap.TryGetValue(_leftClickedModel, out var modelInfo)
                )
                {
                    // 为选中的模型添加黄色边界框
                    AddSelectionBoundingBox(modelInfo.Id!);
                }
            }
            else
            {
                // 点击空白处时清除选中状态
                _leftClickedModel = null;
            }
        }

        private void AddSelectionBoundingBox(string modelId)
        {
            if (_modelVisualMap.TryGetValue(modelId, out var modelVisual))
            {
                var model = modelVisual.Content;

                // 计算模型的边界
                var bounds = model.Bounds;

                var maxEdge = double.Max(bounds.SizeX, bounds.SizeY);
                maxEdge = double.Max(maxEdge, bounds.SizeZ);

                // 传入Transform后，模型的边界框自动变换了，因此以下计算不需要了
                // 如果模型有变换，需要将边界框也应用相同的变换
                //if (model.Transform != null && model.Transform != Transform3D.Identity)
                //{
                //    // 创建边界框的8个顶点
                //    var corners = new[]
                //    {
                //        new Point3D(bounds.X, bounds.Y, bounds.Z),
                //        new Point3D(bounds.X + bounds.SizeX, bounds.Y, bounds.Z),
                //        new Point3D(bounds.X, bounds.Y + bounds.SizeY, bounds.Z),
                //        new Point3D(bounds.X + bounds.SizeX, bounds.Y + bounds.SizeY, bounds.Z),
                //        new Point3D(bounds.X, bounds.Y, bounds.Z + bounds.SizeZ),
                //        new Point3D(bounds.X + bounds.SizeX, bounds.Y, bounds.Z + bounds.SizeZ),
                //        new Point3D(bounds.X, bounds.Y + bounds.SizeY, bounds.Z + bounds.SizeZ),
                //        new Point3D(
                //            bounds.X + bounds.SizeX,
                //            bounds.Y + bounds.SizeY,
                //            bounds.Z + bounds.SizeZ
                //        ),
                //    };

                //    // 变换所有顶点
                //    for (int i = 0; i < corners.Length; i++)
                //    {
                //        corners[i] = model.Transform.Transform(corners[i]);
                //    }

                //    // 从变换后的顶点重新计算边界
                //    bounds = new Rect3D(corners[0], new Size3D(0, 0, 0));
                //    foreach (var corner in corners)
                //    {
                //        bounds.Union(corner);
                //    }
                //}

                // 创建黄色边界框
                _selectionBoundingBox = new BoundingBoxVisual3D
                {
                    BoundingBox = bounds,
                    //Diameter = 0.1,
                    Diameter = 0.01* maxEdge,
                    Fill = Brushes.Yellow,
                };

                // 添加到视口
                viewport.Children.Add(_selectionBoundingBox);
            }
        }

        private void BtnRemoveModel_Click(object sender, RoutedEventArgs e)
        {
            if (
                _leftClickedModel != null
                && _modelInfoMap.TryGetValue(_leftClickedModel, out var modelInfo)
            )
            {
                var result = MessageBox.Show(
                    $"确定要删除模型 '{modelInfo.Name}' 吗？",
                    "确认删除",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question
                );

                if (result == MessageBoxResult.Yes)
                {
                    RemoveModel(modelInfo.Id!);
                    _leftClickedModel = null;
                }
            }
        }

        private void BtnCenterModel_Click(object sender, RoutedEventArgs e)
        {
            if (
                _leftClickedModel != null
                && _modelInfoMap.TryGetValue(_leftClickedModel, out var modelInfo)
            )
            {
                _modelCenterMap.TryGetValue(modelInfo.Id!, out var originalCenter);
                // 获取变换后的中心点
                var transformedCenter = originalCenter;

                if (_modelVisualMap.TryGetValue(modelInfo.Id!, out var modelVisual))
                {
                    var model = modelVisual.Content;
                    if (model?.Transform != null && model.Transform != Transform3D.Identity)
                    {
                        // 应用变换到中心点
                        transformedCenter = model.Transform.Transform(originalCenter);
                    }
                }

                viewport.LookAt(transformedCenter, 500);
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
