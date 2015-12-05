using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Nyx.Composition;
using Nyx.Messaging;

namespace Nyx.AppSupport.SystemTray
{
    class SystemTrayService : ISystemTrayService, ISystemTrayMenuConfigurator, ISystemTrayServiceConfigurator
    {
        private readonly IContainer _container;
        private readonly IMessageRouter _messageRouter;

        private readonly NotifyIcon _systemTrayIcon = new NotifyIcon()
        {
            ContextMenuStrip = new ContextMenuStrip()
        };

        #region ISystemTraceServiceConfigurator

        ISystemTrayServiceConfigurator ISystemTrayServiceConfigurator.SetIcon(Uri packUri)
        {
            if (packUri == null)
            {
                throw new ArgumentNullException(nameof(packUri));
            }

            ChangeIcon(packUri);
            return this;
        }

        ISystemTrayServiceConfigurator ISystemTrayServiceConfigurator.RightclickMenu(Action<ISystemTrayMenuConfigurator> c)
        {
            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }

            foreach (var item in _systemTrayIcon.ContextMenuStrip.Items.Cast<IDisposable>())
            {
                item.Dispose();
            }
            _systemTrayIcon.ContextMenuStrip.Items.Clear();

            c(this);

            return this;
        }


        ISystemTrayMenuConfigurator ISystemTrayMenuConfigurator.MenuItem<T>(string text)
        {
            AddMenuItem<T>(text, null);
            return this;
        }

        ISystemTrayMenuConfigurator ISystemTrayMenuConfigurator.MenuItem<T>(string text, Uri iconUri)
        {
            AddMenuItem<T>(text, iconUri);
            return this;
        }

        ISystemTrayMenuConfigurator ISystemTrayMenuConfigurator.Seperator()
        {
            _systemTrayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            return this;
        }

        #endregion

        public ISystemTrayServiceConfigurator Configure()
        {
            return this;
        }

        public SystemTrayService(IContainer container, IMessageRouter messageRouter)
        {
            _container = container;
            _messageRouter = messageRouter;
            _systemTrayIcon.ContextMenuStrip.ItemClicked += (sender, args) =>
            {
                var metadata = (MenuItemCommandData)args.ClickedItem.Tag;

                if (metadata == null)
                    return;

                var message = _container.Get(metadata.MessageType);
                _messageRouter.Post(message);

            };
        }

        public void Show()
        {
            _systemTrayIcon.Visible = true;
        }

        public void Hide()
        {
            _systemTrayIcon.Visible = false;
        }

        #region IUserNotificationService
        public void ShowInformation(string message, string caption)
        {
            ShowSystrayTooltip(ToolTipIcon.Info, message, caption);
        }

        private void ShowSystrayTooltip(ToolTipIcon toolTipIcon, string message, string caption)
        {
            _systemTrayIcon.BalloonTipIcon = toolTipIcon;
            _systemTrayIcon.BalloonTipText = message;
            _systemTrayIcon.BalloonTipTitle = caption;
            _systemTrayIcon.ShowBalloonTip(10 * 1000);
        }

        public void ShowWarning(string message, string caption)
        {
            ShowSystrayTooltip(ToolTipIcon.Warning, message, caption);
        }

        public void ShowError(string message, string caption)
        {
            ShowSystrayTooltip(ToolTipIcon.Error, message, caption);
        }

        public bool AskConfirmation(string message, string caption)
        {
            throw new NotImplementedException();
        }
        #endregion


        public void ChangeIcon(Uri packUri)
        {
            if (packUri == null) throw new ArgumentNullException(nameof(packUri));

            var streamResourceInfo = System.Windows.Application.GetResourceStream(packUri);
            if (streamResourceInfo != null)
            {
                using (var stream = streamResourceInfo.Stream)
                {
                    _systemTrayIcon.Icon = new Icon(stream);
                }

            }
            else
            {
                throw new InvalidOperationException("Invalid icon path");
            }
        }


        class MenuItemCommandData
        {
            public MenuItemCommandData(Type messageType)
            {
                MessageType = messageType;
            }

            public Type MessageType { get; }
        }

        public void AddMenuItem<T>(string caption, Uri iconUri)
            where T : class, new()
        {
            var item = new ToolStripMenuItem(caption)
            {
                Tag = new MenuItemCommandData(typeof(T))
            };

            if (iconUri != null)
            {
                var streamResourceInfo = System.Windows.Application.GetResourceStream(iconUri);
                using (var stream = streamResourceInfo.Stream)
                {
                    var image = Image.FromStream(stream);
                    item.Image = image;
                }
            }

            _systemTrayIcon.ContextMenuStrip.Items.Add(item);
        }
    }
}