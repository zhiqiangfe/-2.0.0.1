using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace SUNWODA_SEVB.Core.Common
{
    public class DialogViewModelBase : ViewModelBase
    {
        /// <summary>
        /// UI内容
        /// </summary>
        public FrameworkElement? UIElement { get; set; }

        public DialogViewModelBase()
        {
            GetUIElement();
            if (UIElement != null)
                UIElement.DataContext = this;
        }

        /// <summary>
        /// 获得UI内容
        /// </summary>
        private void GetUIElement()
        {
            var assembly = Assembly.GetEntryAssembly();
            if (assembly == null)
                return;
            var vmClassName = this.GetType().Name;
            var uiClassName = vmClassName.Replace("VM_", "");
            var viewFullClassName = FindFullClassName(uiClassName);
            if (viewFullClassName != null)
            {
                var viewType = assembly.GetType(viewFullClassName);
                if (viewType != null)
                    UIElement = Activator.CreateInstance(viewType) as FrameworkElement;
            }
        }

        /// <summary>
        /// 寻找类全名
        /// </summary>
        /// <param name="className">类名</param>
        /// <returns>类全名</returns>
        public string? FindFullClassName(string className)
        {
            var type = Assembly
                .GetEntryAssembly()
                ?.GetTypes()
                .FirstOrDefault(it => it.Name == className);
            return type?.FullName;
        }
    }
}
