namespace Nyx.AppSupport.Wpf.AppServices
{
    public interface ISelectFileService : IApplicationService
    {
        string FileName { get; set; }
        string[] FileNameList { get; }
        SelectFileMode Mode { get; set; }
        string DefaultExtension { get; set; }
        string Filter { get; set; }
        string Title { get; set; }
    }
}