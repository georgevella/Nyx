using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Nyx.Presentation
{
    public abstract class ViewModel<TViewModel> : AbstractNotifyPropertyChanged<TViewModel>, IViewModel
    {
        protected ViewModel(IUserInterfaceThread userInterfaceThread) : base(userInterfaceThread) { }
    }
}