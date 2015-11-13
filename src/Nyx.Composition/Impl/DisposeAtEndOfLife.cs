using System;
using System.Collections.Generic;

namespace Nyx.Composition.Impl
{
    internal class DisposeAtEndOfLife : AbstractLifeTime
    {
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        public override void Dispose()
        {
            foreach (var item in _disposables)
            {
                item.Dispose();
            }

            _disposables.Clear();
        }

        public override void Register(object instance)
        {
            var disposable = instance as IDisposable;
            if (disposable != null)
            {
                _disposables.Add(disposable);
            }
        }
    }
}