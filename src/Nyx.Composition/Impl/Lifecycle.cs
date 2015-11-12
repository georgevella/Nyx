using System;
using System.Collections.Generic;

namespace Nyx.Composition.Impl
{
    internal class Lifecycle : ILifecycle
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        public void Dispose()
        {
            foreach (var item in _disposables)
            {
                item.Dispose();
            }

            _disposables.Clear();
        }

        public void Register(object instance)
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
            {
                _disposables.Add(disposable);
            }
        }
    }
}