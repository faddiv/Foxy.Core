using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CommonLibraries.Core.Cleanup
{
    /// <summary>
    /// A class that holds multiple <see cref="IDisposable"/> object that needs to disposed together 
    /// in safe manner. If a Dispose throws an exception it doesn't stop the cleanup just raise 
    /// a <see cref="DisposeFailed"/> event. This should be used in a class constructor and should 
    /// used in the Dispose method.
    /// </summary>
    public class DisposableContainer : IDisposable
    {
        private readonly HashSet<IDisposable> _managedResources;

        /// <summary>
        /// Raised at Dispose when the Dispose method of an IDisposable throws an exception.
        /// </summary>
        public event Action<IDisposable, Exception> DisposeFailed;

        /// <summary>
        /// Creates an instance from the DisposableContainer.
        /// </summary>
        public DisposableContainer()
        {
            _managedResources = new HashSet<IDisposable>();
        }

        /// <summary>
        /// Adds an IDisposable instance to the container which is disposed in the <see cref="Dispose"/>.
        /// </summary>
        /// <param name="disposabe"></param>
        public void AddManagedResource(IDisposable disposabe)
        {
            _managedResources.Add(disposabe);
        }

        #region IDisposable Support
        /// <summary>
        /// True if the Dispose already called.
        /// </summary>
        public bool Disposed { get; private set; } = false;

        [SuppressMessage("Design", "CA1031:Do not catch general exception types",
            Justification = "The exception handled in the eventhandler.")]
        protected virtual void Dispose(bool disposing)
        {
            if (!Disposed)
            {
                if (disposing)
                {
                    foreach (var disposable in _managedResources)
                    {
                        try
                        {
                            disposable.Dispose();
                            GC.SuppressFinalize(disposable);
                        }
                        catch (Exception ex)
                        {
                            DisposeFailed?.Invoke(disposable, ex);
                        }
                    }
                }

                Disposed = true;
            }
        }

        ~DisposableContainer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes the added items then calls the <see cref="GC.SuppressFinalize"/> on each. 
        /// If one throws an exception then it doesn't stop just raises a <see cref="DisposeFailed"/> 
        /// event.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
