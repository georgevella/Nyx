namespace Nyx.AppSupport.Wpf.AppServices
{
    public interface ISelectFolderService : IApplicationService
    {
        string Path { get; set; }
        string Title { get; set; }
    }
}