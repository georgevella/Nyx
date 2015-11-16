namespace Nyx.AppSupport
{
    public interface IUserNotificationService
    {
        void ShowInformation(string message, string caption);

        void ShowWarning(string message, string caption);

        void ShowError(string message, string caption);
        bool AskConfirmation(string message, string caption);
    }
}