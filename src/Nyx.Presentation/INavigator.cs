namespace Nyx.Presentation
{
    public interface INavigator
    {
        TViewModel NavigateTo<TViewModel>()
            where TViewModel : IViewModel;

        TViewModel NavigateTo<TViewModel>(bool asDialog)
            where TViewModel : IViewModel;
    }
}