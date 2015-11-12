namespace Nyx.Presentation
{
    public interface INavigator
    {
        TViewModel NavigateTo<TViewModel>()
            where TViewModel : IViewModel;
    }
}