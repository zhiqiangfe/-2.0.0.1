using System.Reflection;
using System.Windows.Media.Media3D;
using SUNWODA_SEVB.Component.UserControls;
using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.ViewModels.Windows.Common;

namespace SUNWODA_SEVB.ViewModels.Pages.Demo
{
    [Module("Demo", "示例业务")]
    public class VM_DemoPage : ViewModelBase
    {
        private readonly ILoggerService<VM_MainWindow> _logger;
        private readonly IModuleManager _moduleManager;
        private bool _initializedView = false;
        private ModelViewerControl? _modelViewerControl;
        public ModelViewerControl? ModelViewerControl
        {
            get => _modelViewerControl;
            set => SetProperty(ref _modelViewerControl, value);
        }

        public VM_DemoPage(
            ILoggerService<VM_MainWindow> logger,
            IModuleManager moduleManager
        )
        {
            _logger = logger;
            _moduleManager = moduleManager;
        }

        protected override async Task OnNavigatedToAsync(object? parameter)
        {
            if (!_initializedView)
            {
                var moduleName = typeof(VM_DemoPage).GetCustomAttribute<ModuleAttribute>()?.Name;
                if (moduleName != null)
                {
                    var view = _moduleManager.GetViewFromService(moduleName);

                    await RunOnUIThreadAsync(() =>
                    {
                        ModelViewerControl = view?.FindName("ModelViewer") as ModelViewerControl;
                    });

                    if (ModelViewerControl is not null)
                        ModelViewerControl.ErrorOccurred += (s, e) =>
                            _logger.Error(e.Message, e.Exception, true);
                    var filePath = "Assets\\model\\C06CT\\C06CT.obj";
                    await RunOnUIThreadAsync(() =>
                    {
                        ModelViewerControl?.LoadModel(filePath, rotation: new Vector3D(90, 0, 0));
                        ModelViewerControl?.CenterModel(
                            ModelViewerControl
                                .GetLoadedModels()
                                .Where(modelinfo => modelinfo.FilePath == filePath)
                                .FirstOrDefault()
                        );
                    });
                }

                _initializedView = true;
            }
            await base.OnNavigatedToAsync(parameter);
        }
    }
}
