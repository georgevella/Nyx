using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Nyx.Presentation
{
    public abstract class ViewModel<TViewModel> : IViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(Expression<Func<TViewModel, object>> property)
        {
            var prop = property.Body as MemberExpression;
            if (prop == null)
            {
                var body = property.Body as UnaryExpression;
                if (body != null)
                {
                    prop = body.Operand as MemberExpression;
                }
            }

            if (prop == null)
                return;

            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(prop.Member.Name));
        }

    }
}