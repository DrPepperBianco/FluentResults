using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FluentResults;

/// <summary>
/// Extensions methods for <see cref="IError"/>
/// </summary>
public static class ErrorExtensions
{
    /// <remarks/>
    extension<TError>(TError self) where TError : IError
    {
        #region Overloads of "CausedBy"
        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public TError CausedBy(IError error)
        {
            if(error == null)
                throw new ArgumentNullException(nameof(error));

            self.Reasons.Add(error);
            return self;
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public TError CausedBy(Exception exception)
        {
            if(exception == null)
                throw new ArgumentNullException(nameof(exception));

            self.Reasons.Add(Result.Settings.ExceptionalErrorFactory(null, exception));
            return self;
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public TError CausedBy(string message, Exception exception)
        {
            if(exception == null)
                throw new ArgumentNullException(nameof(exception));

            self.Reasons.Add(Result.Settings.ExceptionalErrorFactory(message, exception));
            return self;
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public TError CausedBy(string message)
        {
            self.Reasons.Add(Result.Settings.ErrorFactory(message));
            return self;
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public TError CausedBy(IEnumerable<IError> errors)
        {
            if(errors == null)
                throw new ArgumentNullException(nameof(errors));

            self.Reasons.AddRange(errors);
            return self;
        }

        /// <summary>
        /// Set the root cause of the error
        /// </summary>
        public TError CausedBy(IEnumerable<string> errors)
        {
            if(errors == null)
                throw new ArgumentNullException(nameof(errors));

            self.Reasons.AddRange(errors.Select(errorMessage => Result.Settings.ErrorFactory(errorMessage)));
            return self;
        }
        #endregion
    }

}
