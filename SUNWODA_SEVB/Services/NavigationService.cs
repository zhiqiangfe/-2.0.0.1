using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Interfaces;
using SUNWODA_SEVB.Core.Models;

namespace SUNWODA_SEVB.Services
{
    public class NavigationService : INavigationService
    {
        private readonly IModuleManager _moduleManager;
        private readonly ILoggerService<NavigationService> _logger;
        private Frame? _navigationFrame; // 使用Frame来承载Page
        private readonly Stack<ModuleInfo> _navigationStack = new Stack<ModuleInfo>();
        private ModuleInfo? _currentModule;

        private readonly SemaphoreSlim _navigationLock = new SemaphoreSlim(1, 1);

        private CancellationTokenSource? _navigationCts;
        private TaskCompletionSource<object?>? _navigationTcs;

        public event EventHandler<NavigationEventArgs>? Navigated;
        public event EventHandler<NavigatingEventArgs>? Navigating;

        public bool CanNavigateBack => _navigationStack.Count > 0;
        public string CurrentModuleName => _currentModule?.Name ?? "";

        public NavigationService(
            IModuleManager moduleManager,
            ILoggerService<NavigationService> logger
        )
        {
            _moduleManager = moduleManager;
            _logger = logger;
        }

        public void Initialize(Frame navigationFrame)
        {
            _navigationFrame = navigationFrame;
            // 禁用Frame自带的导航UI
            _navigationFrame.NavigationUIVisibility = System
                .Windows
                .Navigation
                .NavigationUIVisibility
                .Hidden;

            // 订阅Frame的导航完成事件
            _navigationFrame.Navigated += OnFrameNavigated;
            _navigationFrame.NavigationFailed += OnFrameNavigationFailed;

            _logger?.Info("Frame导航服务初始化完成");
        }

        public async void NavigateTo(string moduleName, object? parameter = null)
        {
            // 如果正在导航，取消之前的导航
            _navigationCts?.Cancel();
            _navigationCts?.Dispose();
            _navigationCts = new CancellationTokenSource();

            // 使用异步方法进行导航
            await NavigateToAsync(moduleName, parameter, _navigationCts.Token);
        }

        private async Task NavigateToAsync(
            string moduleName,
            object? parameter,
            CancellationToken cancellationToken
        )
        {
            // 防止重复导航到相同模块
            if (_currentModule?.Name == moduleName)
            {
                _logger?.Debug($"已在模块 {moduleName} 中，跳过导航");
                return;
            }

            await _navigationLock.WaitAsync(cancellationToken);
            try
            {
                if (cancellationToken.IsCancellationRequested)
                    return;

                // 创建新的导航完成信号源，替代轮询等待
                _navigationTcs?.TrySetCanceled();
                _navigationTcs = new TaskCompletionSource<object?>();

                _logger?.Info($"开始导航到业务模块: {moduleName}");

                var module = _moduleManager.GetModule(moduleName);
                if (module == null)
                {
                    _logger?.Warn($"业务模块未找到: {moduleName}", true);
                    return;
                }

                // 触发导航前事件
                var navigatingArgs = new NavigatingEventArgs
                {
                    ModuleName = moduleName,
                    Parameter = parameter,
                };
                Navigating?.Invoke(this, navigatingArgs);

                if (navigatingArgs.Cancel)
                {
                    _logger?.Info($"导航到 {moduleName} 被取消");
                    return;
                }

                // 处理当前ViewModel的离开逻辑
                if (_currentModule != null)
                {
                    var currentVm = _moduleManager.GetViewModel(_currentModule) as ViewModelBase;
                    if (currentVm != null && !currentVm.CanNavigateFrom())
                    {
                        _logger?.Info($"当前业务模块 {_currentModule.Name} 阻止导航");
                        return;
                    }
                    currentVm?.OnNavigatedFrom();
                    _navigationStack.Push(_currentModule);
                }

                // 获取或创建页面
                var page = _moduleManager.GetView(module);
                if (page == null)
                {
                    _logger?.Error($"无法创建页面: {moduleName}", true);
                    return;
                }

                var viewModel = page.DataContext as ViewModelBase;

                // 在UI线程执行导航 - 使用同步方式避免持续渲染
                bool navigateStarted = false;
                await Application.Current.Dispatcher.InvokeAsync(
                    () =>
                    {
                        if (_navigationFrame != null && page != null)
                        {
                            navigateStarted = _navigationFrame.Navigate(page);
                        }
                    },
                    DispatcherPriority.Normal
                );

                if (!navigateStarted)
                {
                    _logger?.Warn($"导航被Frame拒绝: {moduleName}");
                    return;
                }

                // 等待Frame导航完成事件，而不是轮询标志位
                using (cancellationToken.Register(() => _navigationTcs?.TrySetCanceled()))
                {
                    await _navigationTcs.Task;
                }

                _currentModule = module;

                viewModel?.OnNavigatedTo(parameter);

                // 触发导航完成事件
                Navigated?.Invoke(
                    this,
                    new NavigationEventArgs { ModuleName = moduleName, Parameter = parameter }
                );

                _logger?.Info($"成功导航到业务模块: {moduleName}");
            }
            catch (OperationCanceledException)
            {
                _logger?.Info($"导航到 {moduleName} 被取消");
            }
            catch (Exception ex)
            {
                _logger?.Error($"导航到 {moduleName} 时发生错误", ex, true);
            }
            finally
            {
                _navigationLock.Release();
            }
        }

        private void OnFrameNavigated(
            object? sender,
            System.Windows.Navigation.NavigationEventArgs e
        )
        {
            _navigationTcs?.TrySetResult(null);
        }

        private void OnFrameNavigationFailed(
            object? sender,
            System.Windows.Navigation.NavigationFailedEventArgs e
        )
        {
            if (e.Exception != null)
            {
                _navigationTcs?.TrySetException(e.Exception);
                _logger?.Error($"Frame导航失败: {e.Exception?.Message}", e.Exception!, true);
            }
            else
            {
                _navigationTcs?.TrySetException(new InvalidOperationException("Frame导航失败"));
                _logger?.Error($"Frame导航失败", true);
            }
        }

        private async Task<Page?> GetPageAsync(ModuleInfo module)
        {
            if (module.Name == null)
                return null;

            return await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                var page = _moduleManager.GetView(module);
                return page;
            });
        }

        public void NavigateBack()
        {
            if (!CanNavigateBack)
            {
                _logger?.Warn("不能回退：导航堆栈为空");
                return;
            }

            var previousModule = _navigationStack.Pop();
            NavigateTo(previousModule.Name ?? "");
        }

        public void Dispose()
        {
            _navigationCts?.Cancel();
            _navigationCts?.Dispose();
            _navigationLock?.Dispose();

            if (_navigationFrame != null)
            {
                _navigationFrame.Navigated -= OnFrameNavigated;
                _navigationFrame.NavigationFailed -= OnFrameNavigationFailed;
            }
        }
    }
}
