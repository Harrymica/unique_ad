namespace uniquead_App.Data
{
    public class AppInstallModalState
    {
        public bool IsOpen { get; set; }

        public event Action OnChange;

        public void NotifyStateChanged() => OnChange?.Invoke();
    }
}




