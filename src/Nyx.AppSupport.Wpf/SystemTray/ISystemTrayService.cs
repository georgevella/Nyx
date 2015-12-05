namespace Nyx.AppSupport.SystemTray
{
    public interface ISystemTrayService : IUserNotificationService
    {
        ISystemTrayServiceConfigurator Configure();
        void Show();
        void Hide();
    }
}