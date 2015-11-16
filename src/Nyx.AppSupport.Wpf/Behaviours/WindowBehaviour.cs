using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Nyx.AppSupport.Behaviours
{
    public class WindowBehaviour
    {
        public WindowBehaviour()
        {

        }

        #region Activated Event
        public static readonly DependencyProperty Activated = DependencyProperty.RegisterAttached(
                "Closing",
                typeof(ICommand),
                typeof(WindowBehaviour),
                new PropertyMetadata(null, ActivatedEventChanged));

        private static void ActivatedEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wnd = d as Window;
            if (wnd == null) return;

            if (e.OldValue != null)
            {
                wnd.Activated -= HandleActivatedEvent;
            }

            if (e.NewValue != null)
            {
                wnd.Activated += HandleActivatedEvent;
            }
        }

        private static void HandleActivatedEvent(object sender, EventArgs e)
        {
            var wnd = (sender as Window);
            if (wnd == null) return;

            var cmd = GetActivated(wnd);

            if (cmd != null)
                cmd.Execute(e);

        }

        public static void SetActivated(DependencyObject o, ICommand value)
        {
            o.SetValue(Activated, value);
        }

        public static ICommand GetActivated(DependencyObject o)
        {
            return o.GetValue(Activated) as ICommand;
        }
        #endregion

        #region Closing Event
        public static readonly DependencyProperty Closing = DependencyProperty.RegisterAttached(
                "Closing",
                typeof(ICommand),
                typeof(WindowBehaviour),
                new PropertyMetadata(null, ClosingEventChanged));

        private static void ClosingEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wnd = d as Window;
            if (wnd == null) return;
            
            if (e.OldValue != null)
            {
                wnd.Closing -= HandleClosingEvent;
            }

            if (e.NewValue != null)
            {
                wnd.Closing += HandleClosingEvent;
            }
        }

        private static void HandleClosingEvent(object sender, CancelEventArgs e)
        {
            var wnd = (sender as Window);
            if (wnd == null) return;

            var cmd = GetClosing(wnd);

            if (cmd != null)
                cmd.Execute(e);

        }

        public static void SetClosing(DependencyObject o, ICommand value)
        {
            o.SetValue(Closing, value);
        }

        public static ICommand GetClosing(DependencyObject o)
        {
            return o.GetValue(Closing) as ICommand;
        }

        #endregion

        #region Closed Event
        public static readonly DependencyProperty Closed = DependencyProperty.RegisterAttached(
                    "Closed",
                    typeof(ICommand),
                    typeof(WindowBehaviour),
                    new PropertyMetadata(null, ClosedEventChanged));

        private static void ClosedEventChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var wnd = d as Window;
            if (wnd == null) return;          

            if (e.OldValue != null)
            {
                wnd.Closed -= HandleClosedEvent;
            }

            if (e.NewValue != null)
            {
                wnd.Closed += HandleClosedEvent;
            }
        }

        private static void HandleClosedEvent(object sender, EventArgs e)
        {
            var wnd = (sender as Window);
            if (wnd == null) return;

            var cmd = GetClosed(wnd);

            if (cmd != null) 
                cmd.Execute(null);
        }


        public static void SetClosed(DependencyObject o, ICommand value)
        {
            o.SetValue(Closed, value);
        }

        public static ICommand GetClosed(DependencyObject o)
        {
            return o.GetValue(Closed) as ICommand;
        }

        #endregion
    }
}