namespace Library.Presentation.MVC.ViewModels
{
    public class PageViewModel
    {
        public PageViewModel(int index, bool isActive, bool isEnabled)
        {
            Index = index;
            IsActive = isActive;
            IsEnabled = isEnabled;
        }

        public int Index { get; }

        public bool IsActive { get; }

        public bool IsEnabled { get; }
    }
}