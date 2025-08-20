using Microsoft.Extensions.DependencyInjection;
using SUNWODA_SEVB.Core.Attributes;
using SUNWODA_SEVB.Core.Common;
using SUNWODA_SEVB.Core.Interfaces;

namespace SUNWODA_SEVB.Services
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMvvmFramework(this IServiceCollection services)
        {
            // 注册核心服务
            services.AddSingleton<IModuleManager, ModuleManager>();
            services.AddSingleton<INavigationService, NavigationService>();

            // 自动注册所有标记了Module特性的Views和ViewModels
            services.AddModuleViewModels();
            services.AddModuleViews();

            return services;
        }

        public static IServiceCollection AddModuleViewModels(this IServiceCollection services)
        {
            var viewModelTypes = AppDomain
                .CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try
                    {
                        return a.GetTypes();
                    }
                    catch
                    {
                        return Type.EmptyTypes;
                    }
                })
                .Where(t =>
                    t.IsClass
                    && !t.IsAbstract
                    && t.IsSubclassOf(typeof(ViewModelBase))
                    && t.GetCustomAttributes(typeof(ModuleAttribute), false).Any()
                );

            foreach (var vmType in viewModelTypes)
            {
                //services.AddTransient(vmType);
                if (vmType is not null)
                {
                    services.AddSingleton(vmType);
                }
            }

            return services;
        }

        public static IServiceCollection AddModuleViews(this IServiceCollection services)
        {
            var viewModelTypes = AppDomain
                .CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try
                    {
                        return a.GetTypes();
                    }
                    catch
                    {
                        return Type.EmptyTypes;
                    }
                })
                .Where(t =>
                    t.IsClass
                    && !t.IsAbstract
                    && t.IsSubclassOf(typeof(ViewModelBase))
                    && t.GetCustomAttributes(typeof(ModuleAttribute), false).Any()
                );

            foreach (var vmType in viewModelTypes)
            {
                // 约定：ViewModel名称去掉"VM_"前缀就是View名称
                var viewName = vmType.Name.Replace("VM_", "");
                var viewNamespace = vmType.Namespace!.Replace(".ViewModels", ".Views");
                var fullViewName = $"{viewNamespace}.{viewName}";

                var viewType =
                    vmType.Assembly.GetType(fullViewName)
                    ?? vmType.Assembly.GetTypes().FirstOrDefault(t => t.Name == viewName);

                if (viewType is not null)
                {
                    services.AddSingleton(viewType);
                }
            }
            return services;
        }
    }
}
