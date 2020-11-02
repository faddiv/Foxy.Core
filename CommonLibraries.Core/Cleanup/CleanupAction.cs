using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace CommonLibraries.Core.Cleanup
{
    /// <summary>
    /// Wrap a method in a <see cref="IDisposable"/> class so it is called on Dispose.
    /// </summary>
    [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly",
        Justification = "Not neccessary")]
    public class CleanupAction : IDisposable
    {
        private readonly Action _action;

        /// <summary>
        /// Creates an instace from the CleanupAction.
        /// </summary>
        /// <param name="action">An action which is called at Dispose.</param>
        public CleanupAction(Action action)
        {
            _action = action ?? throw new ArgumentNullException(nameof(action));
        }

        /// <summary>
        /// Calls the provided action.
        /// </summary>
        public void Dispose()
        {
            _action();
        }

        /// <summary>
        /// Two CleanupAction equals if the given action is the same.
        /// </summary>
        /// <param name="obj">object to compare to.</param>
        /// <returns>true if the obj is CleanupAction with the same action.</returns>
        public override bool Equals(object obj)
        {
            return obj is CleanupAction action &&
                   EqualityComparer<Action>.Default.Equals(_action, action._action);
        }

        /// <summary>
        /// Creates a hashcode based on the provided action.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return -450230908 + EqualityComparer<Action>.Default.GetHashCode(_action);
        }
    }
}
