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
        private readonly INavigationService _navigationService;
        private readonly IModuleManager _moduleManager;
        private ModelViewerControl? _modelViewerControl;
        public ModelViewerControl? ModelViewerControl
        {
            get => _modelViewerControl;
            set => SetProperty(ref _modelViewerControl, value);
        }

        public VM_DemoPage(
            ILoggerService<VM_MainWindow> logger,
            INavigationService navigationService,
            IModuleManager moduleManager
        )
        {
            _logger = logger;
            _navigationService = navigationService;
            _moduleManager = moduleManager;
        }

        public override void OnInitialize()
        {
            base.OnInitialize();
            _navigationService.Navigated += DemoPage_Navigated;
        }

        private async void DemoPage_Navigated(object? sender, NavigationEventArgs e)
        {
            if (
                e.ModuleName is not null
                && e.ModuleName == typeof(VM_DemoPage).GetCustomAttribute<ModuleAttribute>()?.Name
            )
            {
                var view = _moduleManager.GetViewFromService(e.ModuleName);

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
        }
    }
}
