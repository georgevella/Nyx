using System;
using System.Windows;
using System.Windows.Forms;
using Nyx.AppSupport.Wpf.Dialogs.Impl;

namespace Nyx.AppSupport.Wpf.Dialogs
{
    /// <summary>
    /// The OpenFileDialogCommand FrameworkElement provides a way for the View to display the Open File dialog, allow the user to choose a file
    /// and return it as a Path (which can be bound to a ViewModel/Presenter)
    /// 
    /// Sample: <code><dialogs:OpenFileDialogCommand Path="{Binding TempFilePath,Mode=TwoWay,PresentationTraceSources.TraceLevel=High}" x:Name="dlgTempFile" /></code>
    /// 
    /// The Path property is a dependency property and ideally needs to be bound to a backend property which stores the Path.  Its important 
    /// that the Binding Mode is set to TwoWay so that both the source and target of the binding are notified of changes from both parties
    /// 
    /// The Name property is also required since other controls (specifically the button which would trigger the dialog) need to bind 
    /// to the Execute command
    /// </summary>
    internal class OpenFileDialogCommand : BaseFileDialogCommand, IOpenFileDialogCommand
    {
        public bool EnableReadOnlyCheckbox { get; set; }

        protected override DialogResult ExecuteShowDialog(object parameter)
        {
            DialogResult dr;

            using (var d = new System.Windows.Forms.OpenFileDialog())
            {
                d.FileName = Path;
                d.Filter = Filter;
                d.ShowReadOnly = EnableReadOnlyCheckbox;
                d.ShowHelp = EnableHelpButton;
                d.Title = Title;
                d.DefaultExt = DefaultExtension;
                d.AddExtension = AddExtension;

                dr = d.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    Path = d.FileName;
                }
            }

            return dr;
        }
    }
}