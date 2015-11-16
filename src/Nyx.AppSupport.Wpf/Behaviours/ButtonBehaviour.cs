using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Nyx.AppSupport.Behaviours
{
    public class ButtonBehaviour
    {
        public static readonly DependencyProperty ButtonClickCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
            ButtonBase.ClickEvent, 
            "ButtonClickCommand",
            typeof(ButtonBehaviour));

        public static void SetButtonClickCommand(DependencyObject o, ICommand value)
        {
            o.SetValue(ButtonClickCommand, value);
        }

        public static ICommand GetButtonClickCommand(DependencyObject o)
        {
            return o.GetValue(ButtonClickCommand) as ICommand;
        }   
    }
}
