using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Nyx.AppSupport.SystemTray
{
    internal class SystemTrayServiceConfigurator : ISystemTrayServiceConfigurator, ISystemTrayMenuConfigurator
    {
        private readonly NotifyIcon _systemTrayIcon;

        public SystemTrayServiceConfigurator(NotifyIcon systemTrayIcon)
        {
            _systemTrayIcon = systemTrayIcon;
        }

        public ISystemTrayServiceConfigurator UsingIcon(Uri packUri)
        {
            if (packUri == null)
            {
                throw new ArgumentNullException(nameof(packUri));
            }

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

            return this;
        }

        public ISystemTrayServiceConfigurator Menu(Action<ISystemTrayMenuConfigurator> c)
        {
            if (c == null)
            {
                throw new ArgumentNullException(nameof(c));
            }
            if (_systemTrayIcon.ContextMenuStrip != null)
            {
                foreach (var item in _systemTrayIcon.ContextMenuStrip.Items.Cast<IDisposable>())
                {
                    item.Dispose();
                }
                _systemTrayIcon.ContextMenuStrip.Items.Clear();
                _systemTrayIcon.ContextMenuStrip.Dispose();
            }


            _systemTrayIcon.ContextMenuStrip = new ContextMenuStrip();
            _systemTrayIcon.ContextMenuStrip.ItemClicked += (sender, args) =>
            {
                var messageId = args.ClickedItem.Tag;
            };

            c(this);

            return this;
        }

        public ISystemTrayMenuConfigurator MenuItem(string text, object messageId)
        {
            var item = new ToolStripMenuItem(text)
            {
                Tag = messageId
            };
            _systemTrayIcon.ContextMenuStrip.Items.Add(item);
            return this;
        }
        public ISystemTrayMenuConfigurator MenuItem(string text, Uri packUri, object messageId)
        {

            var streamResourceInfo = System.Windows.Application.GetResourceStream(packUri);
            using (var stream = streamResourceInfo.Stream)
            {
                var image = Image.FromStream(stream);
                var item = new ToolStripMenuItem(text, image)
                {
                    Tag = messageId
                };
                _systemTrayIcon.ContextMenuStrip.Items.Add(item);
            }
            return this;
        }

        public ISystemTrayMenuConfigurator Seperator()
        {
            _systemTrayIcon.ContextMenuStrip.Items.Add(new ToolStripSeparator());
            return this;
        }
    }
}