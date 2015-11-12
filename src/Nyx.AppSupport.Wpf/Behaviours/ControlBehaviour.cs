using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Nyx.AppSupport.Wpf.Behaviours
{
    public class ControlBehaviour
    {
        #region DoubleClick Event
        public static readonly DependencyProperty DoubleClickCommand = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
            Control.MouseDoubleClickEvent,
            "DoubleClickCommand",
            typeof(ControlBehaviour));

        public static void SetDoubleClickCommand(DependencyObject o, ICommand value)
        {            
            o.SetValue(DoubleClickCommand, value);
        }

        public static ICommand GetDoubleClickCommand(DependencyObject o)
        {
            return o.GetValue(DoubleClickCommand) as ICommand;
        }
        #endregion

        #region MouseEnter Event
        public static readonly DependencyProperty MouseEnter = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
            UIElement.MouseEnterEvent,
            "MouseEnter",
            typeof(ControlBehaviour));

        public static void SetMouseEnter(DependencyObject o, ICommand value)
        {
            o.SetValue(MouseEnter, value);
        }

        public static ICommand GetMouseEnter(DependencyObject o)
        {
            return o.GetValue(MouseEnter) as ICommand;
        }
        #endregion

        #region MouseLeave Event
        public static readonly DependencyProperty MouseLeave = EventBehaviourFactory.CreateCommandExecutionEventBehaviour(
            UIElement.MouseLeaveEvent,
            "MouseLeave",
            typeof(ControlBehaviour));

        public static void SetMouseLeave(DependencyObject o, ICommand value)
        {
            o.SetValue(MouseLeave, value);
        }

        public static ICommand GetMouseLeave(DependencyObject o)
        {
            return o.GetValue(MouseLeave) as ICommand;
        }
        #endregion
    }
}
