using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace HTHIUM.Core.Common
{
    /// <summary>
    /// MVVM 模式的基类，提供属性通知功能
    /// </summary>
    public abstract class ModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public event PropertyChangingEventHandler? PropertyChanging;

        /// <summary>
        /// 触发属性已更改事件
        /// </summary>
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// 触发属性即将更改事件
        /// </summary>
        protected virtual void OnPropertyChanging([CallerMemberName] string? propertyName = null)
        {
            PropertyChanging?.Invoke(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// 设置属性值并触发通知
        /// </summary>
        protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            OnPropertyChanging(propertyName);
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 设置属性值并触发通知，支持自定义比较器
        /// </summary>
        protected virtual bool SetProperty<T>(ref T field, T value, IEqualityComparer<T> comparer, [CallerMemberName] string? propertyName = null)
        {
            if (comparer.Equals(field, value))
                return false;

            OnPropertyChanging(propertyName);
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        /// <summary>
        /// 设置属性值并执行回调
        /// </summary>
        protected virtual bool SetProperty<T>(ref T field, T value, Action<T> onChanged, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;

            OnPropertyChanging(propertyName);
            field = value;
            OnPropertyChanged(propertyName);
            onChanged?.Invoke(value);
            return true;
        }

        /// <summary>
        /// 通知多个属性发生变化
        /// </summary>
        protected void OnPropertiesChanged(params string[] propertyNames)
        {
            foreach (var propertyName in propertyNames)
            {
                OnPropertyChanged(propertyName);
            }
        }

        /// <summary>
        /// 通知所有属性发生变化
        /// </summary>
        protected void RefreshAll()
        {
            OnPropertyChanged(string.Empty);
        }
    }
}
