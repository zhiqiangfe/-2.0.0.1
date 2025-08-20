namespace SUNWODA_SEVB.Core.Interfaces
{
    public interface INavigationService
    {
        void NavigateTo(string moduleName, object? parameter = null);
        void NavigateBack();
        bool CanNavigateBack { get; }
        string CurrentModuleName { get; }
        event EventHandler<NavigationEventArgs> Navigated;
        event EventHandler<NavigatingEventArgs> Navigating;
    }

    public class NavigationEventArgs : EventArgs
    {
        public string? ModuleName { get; set; }
        public object? Parameter { get; set; }
    }

    public class NavigatingEventArgs : EventArgs
    {
        public string? ModuleName { get; set; }
        public object? Parameter { get; set; }
        public bool Cancel { get; set; }
    }
}
