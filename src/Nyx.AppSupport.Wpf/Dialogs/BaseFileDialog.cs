using System.Windows;

namespace Nyx.AppSupport.Wpf.Dialogs
{
    public abstract class BaseFileDialog : BaseDialogCommand
    {
        public static readonly DependencyProperty PathProperty = DependencyProperty.RegisterAttached(
            "Path",
            typeof(string),
            typeof(BaseFileDialog),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );
        public static void SetPath(FrameworkElement dObject, string path)
        {
            dObject.SetValue(PathProperty, path);
        }

        public static string GetPath(FrameworkElement dObject)
        {
            return dObject.GetValue(PathProperty) as string;
        }


        public string Path
        {
            get
            {
                return (string)GetValue(PathProperty);
            }
            set
            {
                SetValue(PathProperty, value);

                OnPropertyChanged("Path");
            }
        }

        public string Filter { get; set; }
        public string Title { get; set; }
        public string DefaultExtension { get; set; }
        public bool EnableHelpButton { get; set; }
        public bool AddExtension { get; set; }


    }
}