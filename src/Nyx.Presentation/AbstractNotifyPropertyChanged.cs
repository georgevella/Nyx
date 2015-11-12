﻿using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Nyx.Presentation
{
    public class AbstractNotifyPropertyChanged<TComponent> : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChangedImpl(propertyName);
        }

        private void OnPropertyChangedImpl(string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException(nameof(propertyName));
            }

            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(Expression<Func<TComponent, object>> property)
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

            OnPropertyChangedImpl(prop.Member.Name);
        }
    }
}